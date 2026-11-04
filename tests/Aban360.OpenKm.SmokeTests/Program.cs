using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Querys.Contracts;
using Aban360.BlobPool.Domain.Features.OpenKm;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Authentication;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

int checks = 0;
void Check(bool condition, string label)
{
    if (!condition) throw new Exception(label);
    checks++;
}
async Task Reject(Fixture fixture, CopyTrackImagesToBillInputDto input)
{
    try { await fixture.Run(input); }
    catch (BaseException) { Check(fixture.OpenKmCalls == 0, "rejected before accessing OpenKM"); return; }
    throw new Exception("Expected rejection");
}

await Reject(new(), new() { TrackNumber = 0, BillId = "123" });
await Reject(new(), new() { TrackNumber = 10, BillId = "../123" });
await Reject(new(), new() { TrackNumber = 10, BillId = "null" });
await Reject(new() { LinkedBill = null }, new() { TrackNumber = 10, BillId = "123" });
await Reject(new() { LinkedBill = "999" }, new() { TrackNumber = 10, BillId = "123" });
await Reject(new() { Denied = true }, new() { TrackNumber = 10, BillId = "123" });

var missing = new Fixture { SourceExists = false };
try { await missing.Run(); throw new Exception("Missing source accepted"); }
catch (BaseException) { Check(!missing.DestinationExists && missing.Uploads == 0, "missing source makes no writes"); }

var fixture = new Fixture();
fixture.Image("photo.png", "one");
fixture.Source.Add(new Document { Uuid = "pdf", Path = "r_10/document.pdf", MimeType = "application/pdf" });
var first = await fixture.Run();
Check(first.IsLinked && first.IsComplete && first.CopiedCount == 1, "linked image copied");
Check(fixture.DestinationExists && fixture.Uploads == 1 && fixture.Source.Count == 2, "destination created; source preserved");
Check(first.Files.Single().DestinationUuid == "copy-1", "new UUID returned");
var xml = XDocument.Parse(fixture.Metadata.Single().Value);
Check(xml.Descendants("name").Single().Value == "okp:moshtarakin.title" && xml.Descendants("value").Single().Value == "7", "document type metadata retained");
Check(fixture.Audit.Count == 2, "start and completion audited");
var repeat = await fixture.Run();
Check(repeat.SkippedCount == 1 && repeat.IsComplete && fixture.Uploads == 1, "repeat avoids duplicate upload");

var conflict = new Fixture();
conflict.Image("photo.png", "source");
conflict.Existing("photo.png", "different");
var collision = await conflict.Run();
Check(collision.ConflictCount == 1 && !collision.IsComplete && conflict.Uploads == 0, "same name different bytes never overwritten");

var recovery = new Fixture { FailMetadataOnce = true };
recovery.Image("photo.png", "source");
var failed = await recovery.Run();
Check(failed.FailedCount == 1 && !failed.IsComplete && failed.Files[0].DestinationUuid != null, "metadata failure reports persisted destination");
var repaired = await recovery.Run();
Check(repaired.IsComplete && repaired.SkippedCount == 1 && recovery.Uploads == 1 && recovery.Metadata.Count == 1, "retry repairs metadata without duplicate");

var continued = new Fixture { FailBinaryUuid = "source-1" };
continued.Image("broken.png", "one");
continued.Image("working.png", "two");
var partial = await continued.Run();
Check(partial.FailedCount == 1 && partial.CopiedCount == 1 && !partial.IsComplete, "file error does not hide later files");

var uploadFailure = new Fixture { FailUploadOnce = true };
uploadFailure.Image("photo.png", "one");
Check((await uploadFailure.Run()).Files[0].ErrorCode == "Upload", "upload failure reported by stage");
Check((await uploadFailure.Run()).IsComplete && uploadFailure.Destination.Count == 1, "upload retry succeeds");

var lostResponse = new Fixture { LoseUploadResponseOnce = true };
lostResponse.Image("photo.png", "one");
Check((await lostResponse.Run()).FailedCount == 1 && lostResponse.Destination.Count == 1, "lost upload response reported as incomplete");
Check((await lostResponse.Run()).SkippedCount == 1 && lostResponse.Uploads == 1, "retry finds upload whose response was lost");

var readMetadataFailure = new Fixture { FailMetadataReadOnce = true };
readMetadataFailure.Image("photo.png", "one");
Check((await readMetadataFailure.Run()).FailedCount == 1 && readMetadataFailure.Uploads == 0, "source metadata read failure prevents upload");

var timeout = new Fixture { TimeoutBinaryUuid = "source-1" };
timeout.Image("slow.png", "one");
timeout.Image("working.png", "two");
var afterTimeout = await timeout.Run();
Check(afterTimeout.FailedCount == 1 && afterTimeout.CopiedCount == 1, "remote timeout does not cancel the batch");

var trimmed = new Fixture { LinkedBill = " 123 " };
Check((await trimmed.Run(new() { TrackNumber = 10, BillId = " 123 " })).BillId == "123", "surrounding whitespace normalized");

var noImages = new Fixture();
Check((await noImages.Run()).IsComplete && noImages.Uploads == 0, "empty source handled");
var unsafeName = new Fixture();
unsafeName.Image("..", "one");
Check((await unsafeName.Run()).FailedCount == 1 && unsafeName.Uploads == 0, "unsafe filename blocked");

var cancelled = new Fixture();
using var cancellation = new CancellationTokenSource();
cancellation.Cancel();
try { await cancelled.Run(token: cancellation.Token); throw new Exception("Cancellation ignored"); }
catch (OperationCanceledException) { Check(cancelled.OpenKmCalls == 0, "cancelled request has no side effects"); }

var parallel = new Fixture();
parallel.Image("photo.png", "one");
var concurrent = await Task.WhenAll(parallel.Run(), parallel.Run());
Check(parallel.Uploads == 1 && concurrent.Sum(result => result.SkippedCount) == 1, "same destination copies serialized");

foreach (bool listShape in new[] { true, false })
{
    foreach (string jsonValue in new[] { "7", "\"7\"", "null" })
    {
        string property = "{\"name\":\"okp:moshtarakin.title\",\"value\":" + jsonValue + "}";
        string json = "{\"simplePropertyGroup\":" + (listShape ? "[" + property + "]" : property) + "}";
        var parsed = await ReadMetadataResponse(json);
        Check(parsed.RawMetaDatas!.Single().Value == (jsonValue == "null" ? (int?)null : 7),
            $"HTTP metadata response: list={listShape}, value={jsonValue}");
    }
}
try
{
    await ReadMetadataResponse("{\"simplePropertyGroup\":{\"name\":\"okp:moshtarakin.title\",\"value\":\"invalid\"}}");
    throw new Exception("Invalid numeric metadata accepted");
}
catch (JsonException) { checks++; }
Check(JsonSerializer.Serialize(new PropertyGroupItem { Name = "title", Value = 7 }).Contains("\"value\":7"),
    "metadata serialization remains numeric");

var auditFailure = new Fixture { FailCompletionAudit = true };
auditFailure.Image("photo.png", "one");
var auditedCopy = await auditFailure.Run();
Check(auditedCopy.IsComplete && auditedCopy.CopiedCount == 1 && auditedCopy.FailedCount == 0 && auditFailure.Uploads == 1,
    "completion audit failure preserves copy outcome");
Check(auditedCopy.Warnings.Single().Code == "CompletionAuditFailed" && auditedCopy.Files.Single().DestinationUuid == "copy-1",
    "completion audit warning returned alongside destination UUID");
Check((await auditFailure.Run()).SkippedCount == 1 && auditFailure.Uploads == 1,
    "retry after audit failure does not upload again");

var partialAuditFailure = new Fixture { FailCompletionAudit = true, FailBinaryUuid = "source-1" };
partialAuditFailure.Image("broken.png", "one");
partialAuditFailure.Image("working.png", "two");
var auditedPartial = await partialAuditFailure.Run();
Check(!auditedPartial.IsComplete && auditedPartial.CopiedCount == 1 && auditedPartial.FailedCount == 1 && auditedPartial.Warnings.Count == 1,
    "completion audit failure preserves partial outcomes");
Check(first.Warnings.Count == 0, "successful audit produces no warnings");

var startAuditFailure = new Fixture { FailStartAudit = true };
startAuditFailure.Image("photo.png", "one");
try { await startAuditFailure.Run(); throw new Exception("Starting audit failure ignored"); }
catch (InvalidOperationException)
{
    Check(!startAuditFailure.DestinationExists && startAuditFailure.Uploads == 0,
        "starting audit failure still prevents remote writes");
}
Console.WriteLine($"Passed {checks} OpenKM checks.");

static async Task<MetaDataProperties> ReadMetadataResponse(string json)
{
    using var client = new HttpClient(new MetadataResponseHandler(json)) { BaseAddress = new Uri("https://openkm.test/") };
    var factory = Proxy.Create<IHttpClientFactory>((_, _) => client);
    var token = Proxy.Create<IEsbTokenProvider>((_, _) => Task.FromResult(new AuthenticationHeaderValue("Bearer", "test")));
    var serviceType = typeof(IOpenKmQueryService).Assembly.GetType("Aban260.BlobPool.Infrastructure.Features.DmsServices.Implementations.OpenKmQueryService", true)!;
    var service = (IOpenKmQueryService)Activator.CreateInstance(serviceType, factory,
        Options.Create(new OpenKmOptions { GeMetadataEndpoint = "metadata" }), token)!;
    return await service.GetMetaDataProperties("document");
}

sealed class Fixture
{
    public string? LinkedBill = "123";
    public bool Denied, DestinationExists, FailMetadataOnce, FailUploadOnce, LoseUploadResponseOnce, FailMetadataReadOnce;
    public bool FailStartAudit, FailCompletionAudit;
    public bool SourceExists = true;
    public string? FailBinaryUuid, TimeoutBinaryUuid;
    public int OpenKmCalls, Uploads;
    public List<Document> Source = new(), Destination = new();
    public Dictionary<string, byte[]> Binaries = new();
    public Dictionary<string, string> Metadata = new();
    public List<string> Audit = new();

    public void Image(string name, string content)
    {
        string uuid = $"source-{Source.Count + 1}";
        Source.Add(new Document { Uuid = uuid, Path = $"r_10/{name}", MimeType = "image/png" });
        Binaries[uuid] = Encoding.UTF8.GetBytes(content);
    }

    public void Existing(string name, string content)
    {
        DestinationExists = true;
        Destination.Add(new Document { Uuid = "existing", Path = $"123/{name}", MimeType = "image/png" });
        Binaries["existing"] = Encoding.UTF8.GetBytes(content);
    }

    public Task<CopyTrackImagesToBillOutputDto> Run(CopyTrackImagesToBillInputDto? input = null, CancellationToken token = default)
    {
        var openKm = Proxy.Create<IOpenKmQueryService>((method, args) =>
        {
            OpenKmCalls++;
            switch (method.Name)
            {
                case "CheckFolderExists":
                    if ((string)args[0]! != "r_10") throw new Exception("Unexpected source folder");
                    return Task.FromResult(SourceExists);
                case "GetFilesTracknumber": return Task.FromResult(new FileListResponse { Documents = Source.ToList() });
                case "GetFilesByBillId": return Task.FromResult(new FileListResponse { Documents = Destination.ToList() });
                case "GetFileBinary":
                    if ((string)args[0]! == FailBinaryUuid) throw new HttpRequestException("download failure");
                    if ((string)args[0]! == TimeoutBinaryUuid) throw new TaskCanceledException("remote timeout");
                    return Task.FromResult(Binaries[(string)args[0]!]);
                case "GetMetaDataProperties":
                    if (FailMetadataReadOnce) { FailMetadataReadOnce = false; throw new HttpRequestException("metadata read failure"); }
                    return Task.FromResult(new MetaDataProperties { RawMetaDatas = new() { new PropertyGroupItem { Name = "okp:moshtarakin.title", Value = 7 } } });
                case "AddFile": return Upload(args);
                case "MarkNodeAsMetadatable": return Task.CompletedTask;
                case "AddOrUpdateMetadata":
                    if (FailMetadataOnce) { FailMetadataOnce = false; throw new HttpRequestException("metadata failure"); }
                    Metadata[(string)args[1]!] = (string)args[0]!;
                    return Task.CompletedTask;
                default: throw new Exception("Unexpected OpenKM operation: " + method.Name);
            }
        });
        var tracking = Proxy.Create<ITrackingQueryService>((method, _) => method.Name == "GetLatest"
            ? Task.FromResult(new TrackingOutputDto { BillId = LinkedBill, ZoneId = 1 }) : throw new Exception(method.Name));
        var zones = Proxy.Create<ICommonZoneService>((method, _) =>
        {
            if (Denied) throw new BaseException("Access denied");
            return Task.FromResult<IEnumerable<int>>(new[] { 1 });
        });
        var folders = Proxy.Create<ICreateFolderHandler>((_, _) => { DestinationExists = true; return Task.FromResult("folder"); });
        var log = Proxy.Create<IOpLogCommandService>((_, args) =>
        {
            bool isCompletion = ((string)args[0]!).Contains("پایان کپی");
            if ((isCompletion && FailCompletionAudit) || (!isCompletion && FailStartAudit))
                throw new InvalidOperationException("audit failure");
            Audit.Add((string)args[0]!);
            return Task.CompletedTask;
        });
        var type = typeof(ICopyTrackImagesToBillHandler).Assembly.GetType("Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Implementations.CopyTrackImagesToBillHandler", true)!;
        var logger = Activator.CreateInstance(typeof(NullLogger<>).MakeGenericType(type))!;
        var handler = (ICopyTrackImagesToBillHandler)Activator.CreateInstance(type, openKm, tracking, zones, folders, log, logger)!;
        return handler.Handle(input ?? new() { TrackNumber = 10, BillId = "123" }, new User(), token);
    }

    private async Task<AddFileDto> Upload(object?[] args)
    {
        await Task.Yield();
        if (FailUploadOnce) { FailUploadOnce = false; throw new HttpRequestException("upload failure"); }
        string uuid = $"copy-{++Uploads}";
        var bytes = await ((StreamContent)args[1]!).ReadAsByteArrayAsync();
        Destination.Add(new Document { Uuid = uuid, Path = (string)args[0]!, MimeType = "image/png" });
        Binaries[uuid] = bytes;
        if (LoseUploadResponseOnce) { LoseUploadResponseOnce = false; throw new HttpRequestException("response lost after upload"); }
        return new AddFileDto { Uuid = uuid, Size = bytes.Length };
    }
}

class Proxy : DispatchProxy
{
    public Func<MethodInfo, object?[], object> InvokeMethod = null!;
    protected override object? Invoke(MethodInfo? method, object?[]? args) => InvokeMethod(method!, args!);
    public static T Create<T>(Func<MethodInfo, object?[], object> handler) where T : class
    {
        T proxy = Create<T, Proxy>();
        ((Proxy)(object)proxy).InvokeMethod = handler;
        return proxy;
    }
}

sealed class User : IAppUser
{
    public Guid UserId => Guid.Empty;
    public string Username => "test";
    public string FullName => "test";
    public ICollection<string> Roles => Array.Empty<string>();
    public bool IsAdmin => false;
}

sealed class MetadataResponseHandler(string json) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
        Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        });
}
