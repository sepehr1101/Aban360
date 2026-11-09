using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Querys.Contracts;
using Aban360.BlobPool.Domain.Features.DmsServices.Dto.Commands;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Implementations
{
    internal sealed class ReadingArchiveService : IReadingArchiveService
    {
        public const long MaxFileBytes = 20 * 1024 * 1024;
        private const string Folder = "reading";
        private static readonly SemaphoreSlim[] FolderLocks = Enumerable.Range(0, 64).Select(_ => new SemaphoreSlim(1, 1)).ToArray();
        private readonly ICommonMemberQueryService _members;
        private readonly ICommonZoneService _zones;
        private readonly IOpenKmQueryService _archive;
        private readonly ICreateFolderHandler _folders;
        private readonly IReadingImageWatermarker _watermarker;
        private readonly IOpLogCommandService _opLog;
        private readonly ILogger<ReadingArchiveService> _logger;

        public ReadingArchiveService(ICommonMemberQueryService members, ICommonZoneService zones, IOpenKmQueryService archive,
            ICreateFolderHandler folders, IReadingImageWatermarker watermarker, IOpLogCommandService opLog, ILogger<ReadingArchiveService> logger)
        {
            _members = members;
            _zones = zones;
            _archive = archive;
            _folders = folders;
            _watermarker = watermarker;
            _opLog = opLog;
            _logger = logger;
        }

        public async Task<AddFileDto> Add(AddReadingImageInput input, IAppUser appUser, CancellationToken cancellationToken)
        {
            if (input?.File is null || input.File.Length <= 0 || input.File.Length > MaxFileBytes)
                throw new CustomValidationException("تصویر الزامی است و حجم آن نباید بیشتر از ۲۰ مگابایت باشد.");
            string billId = await ValidateBill(input.BillId, appUser, cancellationToken);
            DateTime capturedAt = DateTime.Now;
            string name = string.IsNullOrWhiteSpace(appUser.FullName) ? appUser.Username : appUser.FullName;
            byte[] image = await _watermarker.Apply(input.File, billId, name, capturedAt, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            string fileName = $"{capturedAt.ToShortPersianDateString().Replace('/', '-')}_{capturedAt.ToString("HH-mm-ss-fff", CultureInfo.InvariantCulture)}_{billId}_{Guid.NewGuid():N}.jpg";

            var folderLock = FolderLocks[(uint)StringComparer.Ordinal.GetHashCode(billId) % FolderLocks.Length];
            await folderLock.WaitAsync(cancellationToken);
            try
            {
                await EnsureFolder(billId, string.Empty, cancellationToken);
                await EnsureFolder(Folder, billId, cancellationToken);
            }
            finally { folderLock.Release(); }

            cancellationToken.ThrowIfCancellationRequested();
            using var stream = new MemoryStream(image, writable: false);
            using var content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            var result = await _archive.AddFile($"{billId}/{Folder}/{fileName}", content, fileName);
            if (string.IsNullOrWhiteSpace(result?.Uuid)) throw new InvalidOperationException("Archive upload returned no document UUID.");
            try
            {
                await _opLog.Insert($"بارگذاری تصویر قرائت کنتور؛ شناسه قبض: {billId}؛ فایل: {fileName}؛ شناسه سند: {result.Uuid}", appUser);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Reading image {DocumentId} for bill {BillId} was saved, but its operation log failed for user {UserId}.", result.Uuid, billId, appUser.UserId);
            }
            return result;
        }

        public async Task<FileListWithTitlesResponse> Get(string billId, IAppUser appUser, CancellationToken cancellationToken)
        {
            billId = await ValidateBill(billId, appUser, cancellationToken);
            string path = $"{billId}/{Folder}";
            if (!await _archive.CheckFolderExists(path)) return new FileListWithTitlesResponse();
            cancellationToken.ThrowIfCancellationRequested();
            var files = await _archive.GetFilesByBillId(path);
            cancellationToken.ThrowIfCancellationRequested();
            return new FileListWithTitlesResponse
            {
                Documents = files.Documents?.Select(file => new DocumentWithFileTitle(file)
                {
                    FileTitle = "تصویر قرائت کنتور"
                }).ToList() ?? new()
            };
        }

        private async Task<string> ValidateBill(string? input, IAppUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string billId = input?.Trim() ?? string.Empty;
            if (!TransactionIdGenerator.IsBillIdStructureValid(billId)) throw new CustomValidationException("شناسه قبض معتبر نیست.");
            var member = await _members.Get(billId, false);
            if (member is null || member.ZoneId <= 0) throw new CustomValidationException("شناسه قبض در اطلاعات مشترکین یافت نشد.");
            await _zones.IsUserInZone(user, member.ZoneId);
            cancellationToken.ThrowIfCancellationRequested();
            return billId;
        }

        private async Task EnsureFolder(string folder, string parent, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try { await _folders.Handle(folder, cancellationToken, parent); }
            catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.Conflict)
            {
                // Another API process may have created the same folder after the existence check.
                string path = string.IsNullOrEmpty(parent) ? folder : $"{parent}/{folder}";
                if (!await _archive.CheckFolderExists(path)) throw;
            }
        }
    }
}
