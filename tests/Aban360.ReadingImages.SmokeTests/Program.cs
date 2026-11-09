using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.Api.Controllers.V1.BlobController.OpenKm.Commands;
using Aban360.Api.Controllers.V1.BlobController.OpenKm.Queries;
using Aban360.BlobPool.Application.Extenstions;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.BlobPool.Domain.Features.DmsServices.Dto.Commands;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;
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
async Task Rejected(Func<Task> action)
{
    try { await action(); }
    catch (BaseException) { return; }
    throw new Exception("Expected validation failure");
}
var user = new Reader();
string billId = TransactionIdGenerator.GenerateBillId("44417070", "163");
var folders = new HashSet<string>();
var documents = new List<Document>();
int archiveCalls = 0, creates = 0, uploads = 0, logCalls = 0, memberCalls = 0;
bool memberExists = true, allowed = true, logFails = false, uploadFails = false;
string? uploadPath = null, uploadName = null, listPath = null, contentType = null;
byte[] savedImage = Array.Empty<byte>();
string? stampedName = null, stampedBill = null;
DateTime stampedAt = default;
var members = Proxy.For<ICommonMemberQueryService>((method, args) =>
{
    if (method.Name != "Get" || args[0] is not string) throw new Exception("Unexpected member query");
    memberCalls++;
    return Task.FromResult<ZoneIdAndCustomerNumber?>(memberExists ? new(101001, 44417070) : null);
});
var zones = Proxy.For<ICommonZoneService>((method, args) =>
{
    if (method.Name != "IsUserInZone" || !ReferenceEquals(args[0], user)) throw new Exception("Wrong zone validation");
    if (!allowed) throw new BaseException("Access denied");
    return Task.FromResult<IEnumerable<int>>(new[] { 101001 });
});
var archive = Proxy.For<IOpenKmQueryService>((method, args) =>
{
    archiveCalls++;
    switch (method.Name)
    {
        case "CheckFolderExists": return Task.FromResult(folders.Contains((string)args[0]!));
        case "CreateFolder":
            string parent = (string?)args[1] ?? "";
            string path = string.IsNullOrEmpty(parent) ? (string)args[0]! : parent + "/" + args[0];
            if (parent.Length != 0 && !folders.Contains(parent)) throw new Exception("Parent folder missing");
            folders.Add(path);
            creates++;
            return Task.FromResult("folder-id");
        case "MarkNodeAsMetadatable": return Task.CompletedTask;
        case "AddFile": return Upload((string)args[0]!, (StreamContent)args[1]!, (string)args[2]!);
        case "GetFilesByBillId":
            listPath = (string)args[0]!;
            return Task.FromResult(new FileListResponse { Documents = documents.ToList() });
        default: throw new Exception("Unexpected archive operation: " + method.Name);
    }
});
async Task<AddFileDto> Upload(string path, StreamContent content, string name)
{
    if (uploadFails) throw new HttpRequestException("Upload failed");
    uploadPath = path;
    uploadName = name;
    contentType = content.Headers.ContentType?.MediaType;
    savedImage = await content.ReadAsByteArrayAsync();
    string uuid = "reading-image-" + ++uploads;
    documents.Add(new Document { Uuid = uuid, Path = path, Title = name, MimeType = "image/jpeg" });
    return new AddFileDto { Uuid = uuid, Size = savedImage.Length };
}
var log = Proxy.For<IOpLogCommandService>((_, _) =>
{
    logCalls++;
    if (logFails) throw new InvalidOperationException("Audit unavailable");
    return Task.CompletedTask;
});
var markerType = typeof(IReadingImageWatermarker).Assembly.GetType(
    "Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Implementations.ReadingImageWatermarker", true)!;
var markerServices = new ServiceCollection();
markerServices.AddBlobPoolApplicationInjections();
using var markerProvider = markerServices.BuildServiceProvider();
using var markerScope = markerProvider.CreateScope();
var realMarker = markerScope.ServiceProvider.GetRequiredService<IReadingImageWatermarker>();
Check(realMarker.GetType() == markerType, "Actual watermarker resolves through default registration without test overrides");
var marker = Proxy.For<IReadingImageWatermarker>((_, args) =>
{
    stampedBill = (string)args[1]!;
    stampedName = (string)args[2]!;
    stampedAt = (DateTime)args[3]!;
    return realMarker.Apply((IFormFile)args[0]!, stampedBill, stampedName, stampedAt, (CancellationToken)args[4]!);
});
var services = new ServiceCollection();
services.AddLogging();
services.AddBlobPoolApplicationInjections();
services.AddScoped<ICommonMemberQueryService>(_ => members);
services.AddScoped<ICommonZoneService>(_ => zones);
services.AddScoped<IOpenKmQueryService>(_ => archive);
services.AddScoped<IOpLogCommandService>(_ => log);
services.AddScoped<IReadingImageWatermarker>(_ => marker);
using var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();
var service = scope.ServiceProvider.GetRequiredService<IReadingArchiveService>();
Check(service is not null, "Reading archive service resolves through existing registration");
IFormFile File(byte[] bytes, string name = "camera.png", long? length = null) => new FormFile(new MemoryStream(bytes), 0,
    length ?? bytes.Length, "file", name) { Headers = new HeaderDictionary(), ContentType = "image/png" };
var tinyPayload = File(new byte[] { 1, 2, 3 });
foreach (string invalid in new[] { "", "../123456", billId[..^1] + ((billId[^1] - '0' + 1) % 10), "۰۱۲۳۴۵۶۷۸۹" })
{
    int before = archiveCalls, queriesBefore = memberCalls;
    await Rejected(() => service.Add(new() { BillId = invalid, File = tinyPayload }, user, CancellationToken.None));
    Check(archiveCalls == before && memberCalls == queriesBefore, "Invalid bill structure/checksum rejected before external reads or writes");
}
memberExists = false;
await Rejected(() => service.Add(new() { BillId = billId, File = tinyPayload }, user, CancellationToken.None));
Check(archiveCalls == 0, "Nonexistent subscriber is rejected before archive access");
memberExists = true;
allowed = false;
await Rejected(() => service.Add(new() { BillId = billId, File = tinyPayload }, user, CancellationToken.None));
Check(archiveCalls == 0, "User outside subscriber zone cannot upload");
allowed = true;
await Rejected(() => service.Add(new() { BillId = billId, File = File(new byte[] { 1 }, length: 21 * 1024 * 1024) }, user, CancellationToken.None));
Check(archiveCalls == 0, "Oversized file is rejected without creating folders");
var empty = await service.Get(billId, user, CancellationToken.None);
Check(empty.Documents.Count == 0 && creates == 0 && listPath is null, "GET on a missing reading folder is empty and does not create folders");
using var canceled = new CancellationTokenSource();
canceled.Cancel();
int beforeCancel = archiveCalls;
bool cancellationObserved = false;
try { await service.Get(billId, user, canceled.Token); }
catch (OperationCanceledException) { cancellationObserved = true; }
Check(cancellationObserved && archiveCalls == beforeCancel, "Canceled reading request stops before archive access");

if (OperatingSystem.IsWindowsVersionAtLeast(6, 1))
{
    await Rejected(() => service.Add(new() { BillId = billId, File = tinyPayload }, user, CancellationToken.None));
    Check(creates == 0 && uploads == 0, "Invalid image bytes are rejected before any archive write");
    using var bitmap = new Bitmap(1200, 800);
    using (var graphics = Graphics.FromImage(bitmap)) graphics.Clear(Color.LightGray);
    using var png = new MemoryStream();
    bitmap.Save(png, ImageFormat.Png);
    var imageFile = File(png.ToArray(), "../../untrusted-camera-name.png");
    var added = await service.Add(new() { BillId = " " + billId + " ", File = imageFile }, user, CancellationToken.None);
    Check(folders.SetEquals(new[] { billId, billId + "/reading" }) && creates == 2,
        "Missing bill root and nested reading folder are created through the existing folder handler");
    Check(uploadPath == billId + "/reading/" + uploadName && !uploadName!.Contains("untrusted"),
        "Image is saved only under the canonical bill/reading path with a server-generated filename");
    Check(Regex.IsMatch(uploadName!, @"^\d{4}-\d{2}-\d{2}_\d{2}-\d{2}-\d{2}-\d{3}_" + Regex.Escape(billId) + @"_[0-9a-f]{32}\.jpg$")
        && uploadName!.StartsWith(stampedAt.ToShortPersianDateString().Replace('/', '-'))
        && uploadName.Contains(stampedAt.ToString("HH-mm-ss-fff", CultureInfo.InvariantCulture)),
        "Filename contains the DNT Persian date, seconds, milliseconds and bill ID from the same watermark timestamp");
    Check(stampedBill == billId && stampedName == user.FullName, "Watermark uses the validated bill and authenticated uploader name");
    using (var output = Image.FromStream(new MemoryStream(savedImage)))
    {
        Check(output.RawFormat.Guid == ImageFormat.Jpeg.Guid && output.Width == 1200 && output.Height == 800 && contentType == "image/jpeg",
            "Actual PNG input becomes a valid JPEG without reducing the image dimensions");
        using var pixels = new Bitmap(output);
        Check(pixels.GetPixel(50, 50).R > 190 && Enumerable.Range(0, 200).Any(i => pixels.GetPixel(1150, 600 + i).R < 170),
            "Actual watermark changes pixels in the lower image while preserving its upper area");
    }
    string preview = Path.Combine(Directory.GetCurrentDirectory(), "artifacts", "reading-images", "watermark-preview.jpg");
    Directory.CreateDirectory(Path.GetDirectoryName(preview)!);
    await System.IO.File.WriteAllBytesAsync(preview, savedImage);
    string firstName = uploadName!;
    await service.Add(new() { BillId = billId, File = imageFile }, user, CancellationToken.None);
    Check(creates == 2 && uploadName != firstName && uploads == 2 && logCalls == 2,
        "Existing folders are reused and repeated images get distinct filenames with operation logs");
    var listed = await service.Get(billId, user, CancellationToken.None);
    Check(listPath == billId + "/reading" && listed.Documents.Count == 2 && listed.Documents[0].Uuid == added.Uuid
        && listed.Documents.All(item => item.FileTitle == "تصویر قرائت کنتور"),
        "Reading listing uses only the nested folder and retains UUIDs for the existing binary API");
    logFails = true;
    var loggedFailure = await service.Add(new() { BillId = billId, File = imageFile }, user, CancellationToken.None);
    Check(!string.IsNullOrEmpty(loggedFailure.Uuid) && uploads == 3, "Audit logging failure does not turn a saved upload into a false failure");
    logFails = false;
    uploadFails = true;
    bool uploadRejected = false;
    try { await service.Add(new() { BillId = billId, File = imageFile }, user, CancellationToken.None); }
    catch (HttpRequestException) { uploadRejected = true; }
    Check(uploadRejected && uploads == 3, "Archive upload failure is returned without a false success");
    uploadFails = false;
    using var jpeg = new MemoryStream();
    bitmap.Save(jpeg, ImageFormat.Jpeg);
    jpeg.Position = 0;
    using var oriented = Image.FromStream(jpeg);
    var orientation = (PropertyItem)RuntimeHelpers.GetUninitializedObject(typeof(PropertyItem));
    orientation.Id = 0x0112;
    orientation.Type = 3;
    orientation.Len = 2;
    orientation.Value = new byte[] { 6, 0 };
    oriented.SetPropertyItem(orientation);
    using var phoneJpeg = new MemoryStream();
    oriented.Save(phoneJpeg, ImageFormat.Jpeg);
    await service.Add(new() { BillId = billId, File = File(phoneJpeg.ToArray(), "phone.jpg") }, user, CancellationToken.None);
    using var corrected = Image.FromStream(new MemoryStream(savedImage));
    Check(corrected.Width == 800 && corrected.Height == 1200 && !corrected.PropertyIdList.Contains(0x0112),
        "Phone JPEG orientation is applied before stamping and is not applied twice by the viewer");
}
else
{
    Console.WriteLine("SKIP: Native watermark integration requires Windows, matching the project's existing drawing library.");
}
var addAction = typeof(AddFileController).GetMethod("AddReadingImage")!;
Check(addAction.GetCustomAttribute<RouteAttribute>()?.Template == "add-reading-image"
    && addAction.GetParameters()[0].GetCustomAttribute<FromFormAttribute>() is not null
    && addAction.GetParameters()[1].GetCustomAttribute<FromServicesAttribute>() is not null
    && typeof(AddReadingImageInput).GetProperties().Select(property => property.Name).Order().SequenceEqual(new[] { "BillId", "File" }),
    "Upload endpoint accepts exactly billId and IFormFile multipart fields with action-level service injection");
Check(typeof(GetDirectoryTree2Controller).GetMethod("GetReadingDirectoryTree")!.GetCustomAttribute<RouteAttribute>()?.Template == "reading-directory-tree2"
    && typeof(AddFileController).GetConstructors().Single().GetParameters().Length == 1,
    "Reading listing is added while the existing upload controller constructor remains unchanged");
Console.WriteLine($"{checks} checks passed.");

sealed class Reader : IAppUser
{
    public Guid UserId => Guid.Parse("a3f8972b-a9ed-4548-9e3d-691804730006");
    public string Username => "reader1";
    public string FullName => "علی رضایی";
    public ICollection<string> Roles => Array.Empty<string>();
    public bool IsAdmin => false;
}
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
