using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Querys.Contracts;
using Aban360.BlobPool.Domain.Features.OpenKm;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Implementations
{
    internal sealed class CopyTrackImagesToBillHandler : ICopyTrackImagesToBillHandler
    {
        // Bound lock storage while serializing copies to the same bill within this process.
        private static readonly SemaphoreSlim[] DestinationLocks = Enumerable.Range(0, 64).Select(_ => new SemaphoreSlim(1, 1)).ToArray();
        private readonly IOpenKmQueryService _openKm;
        private readonly ITrackingQueryService _tracking;
        private readonly ICommonZoneService _zones;
        private readonly ICreateFolderHandler _folders;
        private readonly IOpLogCommandService _opLog;
        private readonly ILogger<CopyTrackImagesToBillHandler> _logger;

        public CopyTrackImagesToBillHandler(IOpenKmQueryService openKm, ITrackingQueryService tracking,
            ICommonZoneService zones, ICreateFolderHandler folders, IOpLogCommandService opLog,
            ILogger<CopyTrackImagesToBillHandler> logger)
        {
            _openKm = openKm;
            _tracking = tracking;
            _zones = zones;
            _folders = folders;
            _opLog = opLog;
            _logger = logger;
        }

        public async Task<CopyTrackImagesToBillOutputDto> Handle(CopyTrackImagesToBillInputDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            if (input is null || input.TrackNumber <= 0)
                throw new BaseException("شماره پیگیری معتبر وارد نشده است");
            string billId = input.BillId?.Trim() ?? string.Empty;
            if (billId.Length == 0 || billId.Any(character => character < '0' || character > '9'))
                throw new BaseException("شناسه قبض معتبر وارد نشده است");

            var destinationLock = DestinationLocks[(uint)StringComparer.Ordinal.GetHashCode(billId) % DestinationLocks.Length];
            await destinationLock.WaitAsync(cancellationToken);
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var tracking = await _tracking.GetLatest(input.TrackNumber);
                await _zones.IsUserInZone(appUser, tracking.ZoneId);
                if (string.IsNullOrWhiteSpace(tracking.BillId) || !string.Equals(tracking.BillId.Trim(), billId, StringComparison.Ordinal))
                    throw new BaseException("شماره پیگیری و شناسه قبض به یکدیگر مرتبط نیستند");

                string sourceFolder = "r_" + input.TrackNumber.ToString(CultureInfo.InvariantCulture);
                if (!await _openKm.CheckFolderExists(sourceFolder))
                    throw new BaseException("فولدر تصاویر شماره پیگیری یافت نشد");
                var source = await _openKm.GetFilesTracknumber(sourceFolder);
                var images = (source.Documents ?? new List<Document>())
                    .Where(document => document.MimeType?.StartsWith("image/", StringComparison.OrdinalIgnoreCase) == true).ToList();
                var result = new CopyTrackImagesToBillOutputDto { TrackNumber = input.TrackNumber, BillId = billId, IsLinked = true };

                cancellationToken.ThrowIfCancellationRequested();
                await _opLog.Insert($"OpenKM: شروع کپی تصاویر از {sourceFolder} به {billId}", appUser);
                await _folders.Handle(billId, cancellationToken);
                var destination = await _openKm.GetFilesByBillId(billId);
                var existing = destination.Documents ?? new List<Document>();

                foreach (var image in images)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var file = new CopyTrackImageFileOutputDto { SourceUuid = image.Uuid, FileName = GetFileName(image) };
                    result.Files.Add(file);
                    string stage = "ReadSource";
                    try
                    {
                        ValidateFileName(file.FileName);
                        var matchingNames = existing.Where(document => string.Equals(GetFileName(document), file.FileName, StringComparison.Ordinal)).ToList();
                        var binary = await _openKm.GetFileBinary(image.Uuid);
                        var sourceHash = SHA256.HashData(binary);
                        Document? match = null;
                        foreach (var candidate in matchingNames)
                        {
                            var destinationBinary = await _openKm.GetFileBinary(candidate.Uuid);
                            if (CryptographicOperations.FixedTimeEquals(sourceHash, SHA256.HashData(destinationBinary)))
                                match = candidate;
                            else
                            {
                                file.Status = "Conflict";
                                file.DestinationUuid = candidate.Uuid;
                                file.ErrorCode = "FileNameConflict";
                                file.Message = "فایلی با همین نام و محتوای متفاوت در مقصد وجود دارد";
                                break;
                            }
                        }
                        if (file.Status == "Conflict") continue;

                        // Read metadata before uploading; retries repair metadata on an identical existing file.
                        var metadata = await _openKm.GetMetaDataProperties(image.Uuid)
                            ?? throw new InvalidOperationException("OpenKM returned no metadata response.");
                        cancellationToken.ThrowIfCancellationRequested();
                        if (match is null)
                        {
                            stage = "Upload";
                            using var stream = new MemoryStream(binary, writable: false);
                            using var content = new StreamContent(stream);
                            content.Headers.ContentType = new MediaTypeHeaderValue(image.MimeType);
                            var added = await _openKm.AddFile($"{billId}/{file.FileName}", content, file.FileName);
                            if (string.IsNullOrWhiteSpace(added?.Uuid))
                                throw new InvalidOperationException("OpenKM returned an empty document UUID.");
                            file.DestinationUuid = added.Uuid;
                            existing.Add(new Document { Uuid = added.Uuid, Path = $"{billId}/{file.FileName}" });
                        }
                        else
                        {
                            file.DestinationUuid = match.Uuid;
                        }

                        stage = "Metadata";
                        var properties = metadata.RawMetaDatas?.Where(property => property is not null && property.Value.HasValue).ToList();
                        if (properties?.Count > 0)
                        {
                            await _openKm.MarkNodeAsMetadatable(file.DestinationUuid, true);
                            string xml = new XElement("simplePropertiesGroup", properties.Select(property =>
                                new XElement("simplePropertyGroup", new XElement("name", property.Name),
                                    new XElement("value", property.Value!.Value.ToString(CultureInfo.InvariantCulture))))).ToString();
                            await _openKm.AddOrUpdateMetadata(xml, file.DestinationUuid, true);
                        }
                        file.Status = match is null ? "Copied" : "Skipped";
                    }
                    catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) { throw; }
                    catch (Exception exception)
                    {
                        _logger.LogError(exception, "OpenKM copy failed at {Stage} for {SourceUuid}, destination {DestinationUuid}", stage, file.SourceUuid, file.DestinationUuid);
                        file.Status = "Failed";
                        file.ErrorCode = stage;
                        file.Message = stage == "Metadata" ? "فایل در مقصد موجود است اما ثبت متادیتا کامل نشد؛ درخواست را مجدداً اجرا کنید"
                            : stage == "Upload" ? "بارگذاری کامل نشد؛ پیش از تلاش مجدد وضعیت مقصد بررسی می‌شود" : "خواندن یا بررسی اطلاعات تصویر انجام نشد";
                    }
                }

                try
                {
                    await _opLog.Insert($"OpenKM: پایان کپی تصاویر از {sourceFolder} به {billId}؛ کپی: {result.CopiedCount}، تکراری: {result.SkippedCount}، تعارض: {result.ConflictCount}، خطا: {result.FailedCount}", appUser);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "OpenKM completion audit failed for track {TrackNumber}, bill {BillId}", input.TrackNumber, billId);
                    result.Warnings.Add(new CopyTrackImagesToBillWarningDto
                    {
                        Code = "CompletionAuditFailed",
                        Message = "ثبت گزارش پایانی عملیات انجام نشد؛ نتیجه کپی فایل‌ها در پاسخ موجود است"
                    });
                }
                return result;
            }
            finally { destinationLock.Release(); }
        }

        private static string GetFileName(Document document) => (document.Path ?? string.Empty).Split('/').Last();

        private static void ValidateFileName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name is "." or ".." || name.Contains('\\') || name.Any(char.IsControl))
                throw new BaseException("نام فایل مبدأ معتبر نیست");
        }
    }
}
