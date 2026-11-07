using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.Api.Controllers.V1.BlobController.OpenKm.Queries;
using Aban360.BlobPool.Application.Extenstions;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Implementations;
using Aban360.BlobPool.Domain.Features.OpenKm;
using Aban360.BlobPool.Domain.Features.DmsServices.Entities;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.BlobPool.Persistence.Features.DmsServices.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

int checks = 0;
void Check(bool value, string name)
{
    if (!value) throw new Exception("FAIL: " + name);
    checks++;
    Console.WriteLine("PASS: " + name);
}
var original = new FileListResponse
{
    Documents = Enumerable.Range(0, 9).Select(i => new Document
    {
        Uuid = "uuid-" + i, Title = "file-" + i + ".jpg", Path = "/archive/file-" + i + ".jpg", MimeType = "image/jpeg",
        Author = "author", Permissions = 7, Subscribed = true, TenantId = 2, CheckedOut = true,
        Created = new DateTime(2026, 11, 6), LastModified = new DateTime(2026, 11, 6), ConvertibleToPdf = true,
        ConvertibleToSwf = true, Locked = true, Signed = true,
        ActualVersion = new ActualVersion { Actual = true, Author = "author", Checksum = "hash", Created = new DateTime(2026,11,6), Name = 1, Size = 42 }
    }).ToList()
};
original.Documents.Add(original.Documents[0]);
string originalJson = JsonSerializer.Serialize(original);
int listCalls = 0, referenceCalls = 0, metadataCalls = 0, active = 0, peak = 0;
bool failList = false;
bool failReference = false, whitespaceTitle = false;
string? receivedInput = null, receivedTrack = null;
CancellationToken receivedToken = default;
IAppUser? receivedUser = null;
var files = Proxy.For<IGetFilesByBillId>((_, args) =>
{
    listCalls++;
    receivedInput = (string)args[0]!;
    receivedTrack = (string?)args[1];
    receivedUser = (IAppUser)args[2]!;
    receivedToken = (CancellationToken)args[3]!;
    if (failList) throw new InvalidOperationException("Original list failure");
    return Task.FromResult(original);
});
var reference = Proxy.For<IOpenKmMetaDataQueryServices>((method, _) =>
{
    if (method.Name != "Get") throw new Exception("Unexpected reference query");
    referenceCalls++;
    if (failReference) throw new InvalidOperationException("Reference lookup failed");
    return Task.FromResult<IEnumerable<OpenKmMetaData>>(new[]
    {
        new OpenKmMetaData { Id = 1, Section = "okp:moshtarakin.title", Value = 7, KeyLabel = "نوع فایل", Label = whitespaceTitle ? "   " : "سند مالکیت" },
        new OpenKmMetaData { Id = 2, Section = "other", Value = 7, KeyLabel = "دیگر", Label = "نباید عنوان شود" }
    });
});
var openKm = Proxy.For<IOpenKmQueryService>((method, args) =>
{
    if (method.Name != "GetMetaDataProperties") throw new Exception("Binary or unrelated OpenKM call was made");
    return ReadMetadata((string)args[0]!);
});
async Task<MetaDataProperties> ReadMetadata(string uuid)
{
    Interlocked.Increment(ref metadataCalls);
    int current = Interlocked.Increment(ref active);
    int observed;
    do { observed = peak; if (current <= observed) break; }
    while (Interlocked.CompareExchange(ref peak, current, observed) != observed);
    try
    {
        await Task.Delay(15);
        if (uuid == "uuid-2") throw new HttpRequestException("Metadata unavailable");
        if (uuid == "uuid-3") throw new TaskCanceledException("Upstream timeout");
        return uuid == "uuid-1" ? new MetaDataProperties { RawMetaDatas = null } : new MetaDataProperties
        {
            RawMetaDatas = new()
            {
                new PropertyGroupItem { Name = "other", Value = 7 },
                new PropertyGroupItem { Name = "okp:moshtarakin.title", Value = 7 }
            }
        };
    }
    finally { Interlocked.Decrement(ref active); }
}

var categoryCalls = new Dictionary<string, int>();
var categoryArguments = new Dictionary<string, object?[]>();
string? failCategory = null;
T Category<T>(string name) where T : class => Proxy.For<T>((_, args) =>
{
    categoryCalls[name] = categoryCalls.GetValueOrDefault(name) + 1;
    categoryArguments[name] = args;
    if (failCategory == name) throw new InvalidOperationException("Category listing failed");
    return Task.FromResult(original);
});
var services = new ServiceCollection();
services.AddLogging();
services.AddBlobPoolApplicationInjections();
services.AddScoped<IGetFilesByBillId>(_ => files);
services.AddScoped<IOpenKmQueryService>(_ => openKm);
services.AddScoped<IOpenKmMetaDataQueryServices>(_ => reference);
services.AddScoped<IGetFilesDiscount>(_ => Category<IGetFilesDiscount>("discount"));
services.AddScoped<IGetFilesRequest>(_ => Category<IGetFilesRequest>("request"));
services.AddScoped<IDisplayRemovedFilesHandler>(_ => Category<IDisplayRemovedFilesHandler>("removed"));
using var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();
var handler = scope.ServiceProvider.GetRequiredService<IGetFilesByBillIdWithTitles>();
Check(handler is not null, "New handler resolves through existing BlobPool registration");
using var cancellation = new CancellationTokenSource();
var user = new AppUserManual();
var result = await handler!.Handle("123", "100", user, cancellation.Token);
Check(listCalls == 1 && receivedInput == "123" && receivedTrack == "100" && ReferenceEquals(receivedUser, user) && receivedToken == cancellation.Token,
    "Existing directory listing and its validation receive unchanged arguments exactly once");
Check(referenceCalls == 1 && metadataCalls == 9, "Reference metadata is read once and document metadata once per distinct UUID");
Check(peak > 1 && peak <= 4, "Metadata lookup runs concurrently with at most four requests in flight");
Check(result.Documents.Select(document => document.Uuid).SequenceEqual(original.Documents.Select(document => document.Uuid)),
    "Document count and order are retained, including duplicate list entries");
Check(result.Documents[0].FileTitle == "سند مالکیت" && result.Documents[^1].FileTitle == "سند مالکیت",
    "File title is translated from metadata without replacing the existing file name");
Check(result.Documents.Skip(1).Take(3).All(document => document.FileTitle == ""),
    "Missing metadata, individual failures and upstream timeouts keep their files with empty titles");
Check(result.Documents[4].FileTitle == "سند مالکیت", "A failed document title does not prevent other titles from loading");
Check(JsonSerializer.Serialize(original) == originalJson && !originalJson.Contains("fileTitle"),
    "Old DTO and original listing objects are not mutated or given new JSON fields");
var oldDocument = JsonDocument.Parse(originalJson).RootElement.GetProperty("document")[0];
var newDocument = JsonDocument.Parse(JsonSerializer.Serialize(result)).RootElement.GetProperty("document")[0];
Check(oldDocument.EnumerateObject().All(property => newDocument.GetProperty(property.Name).GetRawText() == property.Value.GetRawText())
    && newDocument.GetProperty("fileTitle").GetString() == "سند مالکیت",
    "New response retains every old document field and adds fileTitle");

var controller = ActivatorUtilities.CreateInstance<GetDirectoryTree2Controller>(scope.ServiceProvider);
controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext
{
    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.UserData, user.UserId.ToString()) }, "test"))
} };
var response = (ObjectResult)await controller.Get("123", null, CancellationToken.None);
Check(((ApiResponseEnvelope<FileListWithTitlesResponse>)response.Value!).Data?.Documents.Count == original.Documents.Count,
    "New endpoint returns the standard API envelope");
Check(typeof(GetDirectoryTree2Controller).GetMethod("Get")!.GetCustomAttribute<RouteAttribute>()?.Template == "directory-tree2",
    "New route has the exact requested directory-tree2 name");
Check(typeof(GetFilesListController).GetMethod("GetDirectoryTree")!.GetCustomAttribute<RouteAttribute>()?.Template == "directory-tree",
    "Existing directory-tree route remains available");

var oldController = ActivatorUtilities.CreateInstance<GetFilesListController>(scope.ServiceProvider);
var removedInput = new RemovedFilesInput { FolderName = "100", IsBillId = true };
var endpoints = new (string Kind, string Method, object Input, Func<Task<IActionResult>> NewRun, Func<Task<IActionResult>> OldRun)[]
{
    ("discount", "GetDiscountDirectoryTree", "discount-id",
        () => controller.GetDiscountDirectoryTree("discount-id", cancellation.Token),
        () => oldController.GetDiscountDirectoryTree("discount-id", cancellation.Token)),
    ("request", "GetRequestDirectoryTree", "request-id",
        () => controller.GetRequestDirectoryTree("request-id", cancellation.Token),
        () => oldController.GetRequestDirectoryTree("request-id", cancellation.Token)),
    ("removed", "GetRemovedTree", removedInput,
        () => controller.GetRemovedTree(removedInput, cancellation.Token),
        () => oldController.GetRemovedTree(removedInput, cancellation.Token))
};
foreach (var endpoint in endpoints)
{
    var currentResponse = (ObjectResult)await endpoint.NewRun();
    var currentData = ((ApiResponseEnvelope<FileListWithTitlesResponse>)currentResponse.Value!).Data!;
    Check(currentData.Documents.Count == original.Documents.Count && currentData.Documents[0].FileTitle == "سند مالکیت",
        endpoint.Kind + " endpoint includes titles and all files in the standard envelope");
    var priorResponse = (ObjectResult)await endpoint.OldRun();
    var priorData = ((ApiResponseEnvelope<FileListResponse>)priorResponse.Value!).Data;
    Check(JsonSerializer.Serialize(priorData) == originalJson,
        endpoint.Kind + " original endpoint still returns the original JSON shape without titles");
    Check(categoryCalls[endpoint.Kind] == 2 && Equals(categoryArguments[endpoint.Kind][0], endpoint.Input)
        && (CancellationToken)categoryArguments[endpoint.Kind][1]! == cancellation.Token,
        endpoint.Kind + " forwards the same original input and cancellation token");
    var newMethod = typeof(GetDirectoryTree2Controller).GetMethod(endpoint.Method)!;
    var oldMethod = typeof(GetFilesListController).GetMethod(endpoint.Method)!;
    Check(newMethod.GetCustomAttribute<RouteAttribute>()!.Template == oldMethod.GetCustomAttribute<RouteAttribute>()!.Template + "2"
        && (newMethod.GetCustomAttribute<HttpPostAttribute>() is not null) == (oldMethod.GetCustomAttribute<HttpPostAttribute>() is not null),
        endpoint.Kind + " new route adds suffix 2 and preserves the original HTTP method");
    failCategory = endpoint.Kind;
    int referencesBeforeFailure = referenceCalls;
    bool categoryRejected = false;
    try { await endpoint.NewRun(); }
    catch (InvalidOperationException) { categoryRejected = true; }
    Check(categoryRejected && referenceCalls == referencesBeforeFailure,
        endpoint.Kind + " original handler errors propagate before title lookup");
    failCategory = null;
}
var trackRemovedInput = new RemovedFilesInput { FolderName = "100", IsBillId = false };
await controller.GetRemovedTree(trackRemovedInput, cancellation.Token);
Check(ReferenceEquals(categoryArguments["removed"][0], trackRemovedInput)
    && typeof(GetDirectoryTree2Controller).GetMethod("GetRemovedTree")!.GetParameters()[0].GetCustomAttribute<FromBodyAttribute>() is not null,
    "Removed-files endpoint preserves the JSON body and IsBillId=false flag");

int beforeReference = referenceCalls, beforeMetadata = metadataCalls;
var savedDocuments = original.Documents;
original.Documents = new();
Check((await handler.Handle("123", null, user, CancellationToken.None)).Documents.Count == 0
    && referenceCalls == beforeReference && metadataCalls == beforeMetadata,
    "Empty archive returns an empty document list without metadata requests");
original.Documents = savedDocuments;
using var alreadyCanceled = new CancellationTokenSource();
alreadyCanceled.Cancel();
int beforeList = listCalls;
bool canceled = false;
try { await handler.Handle("123", null, user, alreadyCanceled.Token); }
catch (OperationCanceledException) { canceled = true; }
Check(canceled && listCalls == beforeList, "Cancellation is respected before starting archive work");
failList = true;
bool rejected = false;
beforeReference = referenceCalls;
try { await handler.Handle("123", null, user, CancellationToken.None); }
catch (InvalidOperationException) { rejected = true; }
Check(rejected && referenceCalls == beforeReference, "Original listing errors propagate without reading document metadata");

failList = false;
var folderCalls = new List<string>();
var routingOpenKm = Proxy.For<IOpenKmQueryService>((method, args) =>
{
    if (method.Name == "GetMetaDataProperties") return ReadMetadata((string)args[0]!);
    if (method.Name is "GetFilesDiscount" or "GetFilesTracknumber" or "GetRemovedFiles")
    {
        folderCalls.Add(method.Name + ":" + args[0]);
        return Task.FromResult(original);
    }
    throw new Exception("Unexpected archive operation: " + method.Name);
});
var realServices = new ServiceCollection();
realServices.AddLogging();
realServices.AddBlobPoolApplicationInjections();
realServices.AddScoped<IGetFilesByBillId>(_ => files);
realServices.AddScoped<IOpenKmQueryService>(_ => routingOpenKm);
realServices.AddScoped<IOpenKmMetaDataQueryServices>(_ => reference);
using var realProvider = realServices.BuildServiceProvider();
using var realScope = realProvider.CreateScope();
var realNew = ActivatorUtilities.CreateInstance<GetDirectoryTree2Controller>(realScope.ServiceProvider);
var realOld = ActivatorUtilities.CreateInstance<GetFilesListController>(realScope.ServiceProvider);
realNew.ControllerContext = controller.ControllerContext;
realOld.ControllerContext = controller.ControllerContext;
var comparisons = new (string Name, string? ExpectedCall, Func<Task<IActionResult>> NewRun, Func<Task<IActionResult>> OldRun)[]
{
    ("bill", null, () => realNew.Get("123", "100", cancellation.Token),
        () => realOld.GetDirectoryTree("123", "100", cancellation.Token)),
    ("discount", "GetFilesDiscount:discount-id", () => realNew.GetDiscountDirectoryTree("discount-id", cancellation.Token),
        () => realOld.GetDiscountDirectoryTree("discount-id", cancellation.Token)),
    ("request", "GetFilesTracknumber:r_100", () => realNew.GetRequestDirectoryTree("100", cancellation.Token),
        () => realOld.GetRequestDirectoryTree("100", cancellation.Token)),
    ("removed-bill", "GetRemovedFiles:123", () => realNew.GetRemovedTree(new() { FolderName = "123", IsBillId = true }, cancellation.Token),
        () => realOld.GetRemovedTree(new() { FolderName = "123", IsBillId = true }, cancellation.Token)),
    ("removed-request", "GetRemovedFiles:r_100", () => realNew.GetRemovedTree(new() { FolderName = "100", IsBillId = false }, cancellation.Token),
        () => realOld.GetRemovedTree(new() { FolderName = "100", IsBillId = false }, cancellation.Token))
};
bool SameFiles(FileListResponse oldFiles, FileListWithTitlesResponse newFiles)
{
    var oldJson = JsonDocument.Parse(JsonSerializer.Serialize(oldFiles)).RootElement.GetProperty("document");
    var newJson = JsonDocument.Parse(JsonSerializer.Serialize(newFiles)).RootElement.GetProperty("document");
    return oldJson.GetArrayLength() == newJson.GetArrayLength() && Enumerable.Range(0, oldJson.GetArrayLength())
        .All(index => oldJson[index].EnumerateObject().All(property =>
            newJson[index].GetProperty(property.Name).GetRawText() == property.Value.GetRawText()));
}
foreach (var comparison in comparisons)
{
    folderCalls.Clear();
    var previous = ((ApiResponseEnvelope<FileListResponse>)((ObjectResult)await comparison.OldRun()).Value!).Data!;
    var oldCalls = folderCalls.ToArray();
    folderCalls.Clear();
    var current = ((ApiResponseEnvelope<FileListWithTitlesResponse>)((ObjectResult)await comparison.NewRun()).Value!).Data!;
    Check(folderCalls.SequenceEqual(oldCalls)
        && (comparison.ExpectedCall is null || oldCalls.SequenceEqual(new[] { comparison.ExpectedCall })),
        comparison.Name + " preserves the original listing call and exact OpenKM folder selection");
    Check(SameFiles(previous, current), comparison.Name + " preserves every field and the order of every file");

    failReference = true;
    int metadataBeforeFailure = metadataCalls;
    try
    {
        var withoutTitles = ((ApiResponseEnvelope<FileListWithTitlesResponse>)((ObjectResult)await comparison.NewRun()).Value!).Data!;
        Check(SameFiles(previous, withoutTitles) && withoutTitles.Documents.All(document => document.FileTitle == "")
            && metadataCalls == metadataBeforeFailure,
            comparison.Name + " still displays the original files when title reference lookup fails");
    }
    finally { failReference = false; }
}
whitespaceTitle = true;
try
{
    var oldTitle = await realScope.ServiceProvider.GetRequiredService<IGetMetaDataPropertiesHandler>().Handle("uuid-0", true, cancellation.Token);
    var newTitles = await realScope.ServiceProvider.GetRequiredService<IArchiveFileTitleEnricher>().Enrich(original, cancellation.Token);
    Check(oldTitle.First().ValueTitle == "" && newTitles.Documents[0].FileTitle == "",
        "Whitespace-only file title matches the existing file-title API's empty-title behavior");
}
finally { whitespaceTitle = false; }
Console.WriteLine($"{checks} checks passed.");

class Proxy : DispatchProxy
{
    public Func<MethodInfo, object?[], object?> Callback = null!;
    protected override object? Invoke(MethodInfo? method, object?[]? args) => Callback(method!, args!);
    public static T For<T>(Func<MethodInfo, object?[], object?> callback) where T : class
    {
        var proxy = Create<T, Proxy>();
        ((Proxy)(object)proxy).Callback = callback;
        return proxy;
    }
}
