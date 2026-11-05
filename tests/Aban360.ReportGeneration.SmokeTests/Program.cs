using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Aban360.Api.Cronjobs;
using Aban360.Api.Hubs.Contracts;
using Aban360.Api.Hubs.Implementations;
using Aban360.Api.Controllers.V1.ReportPool.FlatReports.Queries;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;
using Aban360.ReportPool.Application.Features.FlatReports.Handler.Commands.Contracts;
using Aban360.ReportPool.Application.Features.FlatReports.Handler.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Commands;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;
using Aban360.ReportPool.Persistence.Features.FlatReports.Queries.Contracts;
using Hangfire;
using Hangfire.Common;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;

int checks = 0;
void Check(bool condition, string name)
{
    if (!condition) throw new Exception("FAIL: " + name);
    checks++;
    Console.WriteLine("PASS: " + name);
}
async Task<Exception> Failure(Func<Task> run)
{
    try { await run(); }
    catch (Exception exception) { return exception; }
    throw new Exception("Expected operation to fail");
}

string artifactParent = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "tests", ".report-artifacts"));
string artifactRoot = Path.Combine(artifactParent, Guid.NewGuid().ToString("N"));
string originalPath = ExcelManagement.pathBase;
int originalSize = ExcelManagement.maxDataCount;
ExcelManagement.pathBase = artifactRoot;
ExcelManagement.maxDataCount = 2;
try
{
    using var fixture = new Pipeline();
    using var request = new CancellationTokenSource();
    var input = new ReportInput { BillId = "1234567890123", Count = 4 };
    await fixture.Enqueue(input, request.Token);
    Check(fixture.LastJob?.Type == typeof(IReportGenerator) && fixture.Handler.Calls == 0,
        "Request persists metadata and queues work without running the report handler");
    var report = fixture.Record;
    Check(report.HandlerKey == typeof(IFakeReportHandler).FullName,
        "Handler selection finds the matching method contract instead of the first unrelated interface");
    Check(report.ReportInputJson!.Contains("bill_identifier"), "Input snapshot preserves Newtonsoft JSON property names");
    input.BillId = "changed-after-enqueue";
    request.Cancel();
    fixture.SaveGate = new(TaskCreationOptions.RunContinuationsAsynchronously);
    var work = fixture.Execute();
    await fixture.SaveEntered.Task.WaitAsync(TimeSpan.FromSeconds(20));
    Check(!work.IsCompleted && fixture.Notifications.Count == 0,
        "No SignalR notification is sent while report completion persistence is pending");
    fixture.SaveGate.SetResult();
    await work;
    Check(fixture.Handler.Input?.BillId == "1234567890123" && !fixture.Handler.Token.CanBeCanceled,
        "Queued job restores the original JSON input and survives HTTP request cancellation");
    Check(File.Exists(report.ReportPath) && Directory.Exists(artifactRoot),
        "A real Excel workbook is created even when its output directory did not exist");
    Check(fixture.Notifications.Count == 1 && fixture.TargetUser == report.UserId.ToString()
        && fixture.Notifications[0].ReportId == report.Id,
        "Successful persisted workbook notifies the report owner using the same report ID");
    using (var archive = ZipFile.OpenRead(report.ReportPath!))
    {
        using var workbookStream = archive.GetEntry("xl/workbook.xml")!.Open();
        var workbook = XDocument.Load(workbookStream);
        Check(workbook.Descendants().Count(element => element.Name.LocalName == "sheet") == 3,
            "Exact page-size multiples produce only a header sheet and the required data sheets");
        string text = string.Join("\n", archive.Entries.Where(entry => entry.FullName.EndsWith(".xml"))
            .Select(entry => { using var reader = new StreamReader(entry.Open()); return reader.ReadToEnd(); }));
        Check(!text.Contains("hidden-marker"), "Excluded fields are not written into the real workbook");
    }
    int calls = fixture.Handler.Calls;
    string path = report.ReportPath!;
    await fixture.Execute();
    Check(fixture.Handler.Calls == calls && report.ReportPath == path && fixture.Notifications.Count == 2,
        "Retry of an already saved report reuses the file and can reissue notification without regenerating data");

    using var summary = new Pipeline();
    await summary.Enqueue(new ReportInput { BillId = "summary", Count = 1 }, summary: true);
    await summary.Execute();
    Check(summary.Handler.SummaryCalls == 1 && summary.Notifications.Count == 1,
        "Explicit alternate handler method works alongside overloaded Handle methods");

    foreach (string scenario in new[] { "handler", "null", "type", "method", "save" })
    {
        using var broken = new Pipeline();
        await broken.Enqueue(new ReportInput { BillId = scenario, Count = 1 });
        broken.Handler.Throw = scenario == "handler";
        broken.Handler.ReturnNull = scenario == "null";
        broken.FailSave = scenario == "save";
        if (scenario == "type") broken.Record.HandlerKey = "Missing.Report.Contract";
        await Failure(() => broken.Execute(scenario == "method" ? "NotAReportMethod" : null));
        Check(broken.Notifications.Count == 0 && broken.Failures == 1 && broken.Record.ErrorDateTimeJalali is not null,
            "Failure is recorded without a false success notification: " + scenario);
    }

    using var transport = new Pipeline { FailTransport = true };
    await transport.Enqueue(new ReportInput { BillId = "offline", Count = 1 });
    await transport.Execute();
    Check(File.Exists(transport.Record.ReportPath) && transport.Failures == 0 && transport.Handler.Calls == 1,
        "SignalR transport failure does not fail or repeat successfully completed Excel generation");

    using var queue = new Pipeline { FailQueue = true };
    await Failure(() => queue.Enqueue(new ReportInput { BillId = "queue", Count = 1 }));
    Check(queue.Failures == 1 && queue.Handler.Calls == 0 && queue.Notifications.Count == 0,
        "Enqueue failure is marked in the existing report row instead of leaving an unexplained pending request");

    var files = await Task.WhenAll(Enumerable.Range(0, 2).Select(_ => ExcelManagement.ExportToExcelAsync(
        new ReportHeader { Name = "تست" }, new[] { new ReportRow { BillId = "1" } }, "هم‌نام/همزمان:*?")));
    Check(files.Distinct().Count() == 2 && files.All(File.Exists),
        "Concurrent reports with the same title get distinct safe filenames");

    var downloadHandler = Proxy.For<IServerReportsGetFilePathHandler>((_, _) => Task.FromResult(
        new ServerReportsGetFilePathDto { Id = report.Id, ReportPath = report.ReportPath! }));
    var download = (Microsoft.AspNetCore.Mvc.FileStreamResult)await new ServerReportsDownloadController(downloadHandler)
        .Download(report.Id, CancellationToken.None);
    using (download.FileStream)
    {
        Check(download.FileDownloadName == Path.GetFileName(report.ReportPath) && download.FileStream.Length > 0,
            "Existing download controller can open and serve the newly generated file path");
        Check(download.EnableRangeProcessing && download.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Existing Excel content type and resumable download behavior are preserved");
    }

    using var direct = new Pipeline();
    await direct.Generator.DirectExecute(new ReportInput { BillId = "direct", Count = 1 }, CancellationToken.None,
        direct.Handler.Handle, new AppUserManual(), "مستقیم", "connection");
    Check(File.Exists(direct.Record.ReportPath) && direct.Notifications.Count == 0,
        "DirectExecute still saves its workbook without introducing a background notification");

    using var empty = new Pipeline();
    await empty.Enqueue(new ReportInput { BillId = "empty", Count = 0 });
    await empty.Execute();
    Check(File.Exists(empty.Record.ReportPath) && empty.Notifications.Count == 1,
        "Empty report data still produces a downloadable workbook and completion notification");

    var legacyMethod = fixture.Generator.GetType().GetMethod("DoFireAndInform",
        new[] { typeof(ServerReportsCreateDto), typeof(string), typeof(string[]) });
    Check(legacyMethod is not null, "Previously stored concrete Hangfire job method identity and parameters remain available");

    foreach (var name in typeof(IReportGenerator).Assembly.GetReferencedAssemblies()
        .Where(name => name.Name!.StartsWith("Aban360.") && name.Name.EndsWith(".Application"))) Assembly.Load(name);
    var metadataMethod = fixture.Generator.GetType().GetMethod("CreateServerReportDto", BindingFlags.Instance | BindingFlags.NonPublic)!;
    var resolveType = fixture.Generator.GetType().GetMethod("ResolveReportType", BindingFlags.Static | BindingFlags.NonPublic)!;
    int realMethods = 0;
    var incompatible = new List<string>();
    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()
        .Where(assembly => assembly.GetName().Name!.StartsWith("Aban360.") && assembly.GetName().Name!.EndsWith(".Application")))
    foreach (var targetType in assembly.GetTypes().Where(type => type.IsClass && !type.IsAbstract && !type.ContainsGenericParameters))
    foreach (var method in targetType.GetInterfaces().SelectMany(contract => targetType.GetInterfaceMap(contract).TargetMethods)
        .Distinct().Where(method => !method.ContainsGenericParameters && method.ReturnType.IsGenericType
            && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)
            && method.ReturnType.GenericTypeArguments[0].IsGenericType
            && method.ReturnType.GenericTypeArguments[0].GetGenericTypeDefinition() == typeof(ReportOutput<,>)
            && method.GetParameters().Length == 2 && method.GetParameters()[1].ParameterType == typeof(CancellationToken)))
    {
        realMethods++;
        try
        {
            var inputType = method.GetParameters()[0].ParameterType;
            var outputTypes = method.ReturnType.GenericTypeArguments[0].GenericTypeArguments;
            var target = RuntimeHelpers.GetUninitializedObject(targetType);
            var delegateType = typeof(Func<,,>).MakeGenericType(inputType, typeof(CancellationToken), method.ReturnType);
            var handlerDelegate = method.CreateDelegate(delegateType, target);
            var metadata = (ServerReportsCreateDto)metadataMethod.MakeGenericMethod(inputType, outputTypes[0], outputTypes[1])
                .Invoke(fixture.Generator, new object?[] { Guid.NewGuid(), inputType.IsValueType ? Activator.CreateInstance(inputType) : null,
                    handlerDelegate, new AppUserManual(), "compatibility", "connection", method.Name.Split('.').Last() })!;
            foreach (var item in new[]
            {
                (metadata.HandlerKey, 255), (metadata.HeaderType, 255), (metadata.DataType, 1023), (metadata.ReportInputType!, 255)
            })
            {
                if (item.Item1.Length > item.Item2) throw new Exception("Stored type name exceeds its existing schema limit");
            }
            if (resolveType.Invoke(null, new object?[] { metadata.ReportInputType }) as Type != inputType
                || resolveType.Invoke(null, new object?[] { metadata.HeaderType }) as Type != outputTypes[0]
                || resolveType.Invoke(null, new object?[] { metadata.DataType }) as Type != outputTypes[1])
                throw new Exception("Stored type metadata cannot be restored to its original runtime type");
        }
        catch (Exception exception)
        {
            incompatible.Add($"{targetType.FullName}.{method.Name}: {(exception.InnerException ?? exception).Message}");
        }
    }
    Check(realMethods > 0, $"Examined metadata compatibility for {realMethods} actual report handler methods without executing constructors or business queries");
    Check(incompatible.Count == 0, "Actual report contracts remain compatible" +
        (incompatible.Count == 0 ? "" : ":\n" + string.Join("\n", incompatible)));
    Console.WriteLine($"{checks} checks passed.");
}
finally
{
    ExcelManagement.pathBase = originalPath;
    ExcelManagement.maxDataCount = originalSize;
    if (Path.GetFullPath(artifactRoot).StartsWith(artifactParent + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
        && Directory.Exists(artifactRoot)) Directory.Delete(artifactRoot, recursive: true);
}

public interface IUnrelated { void Ping(); }
public interface IFakeReportHandler
{
    Task<ReportOutput<ReportHeader, ReportRow>> Handle(ReportInput input, CancellationToken token);
    Task<ReportOutput<ReportHeader, ReportRow>> Handle(string input, CancellationToken token);
    Task<ReportOutput<ReportHeader, ReportRow>> HandleSummary(ReportInput input, CancellationToken token);
}
public sealed class ReportInput
{
    [JsonProperty("bill_identifier")]
    public string BillId { get; set; } = "";
    public int Count { get; set; }
}
public sealed class ReportHeader { public string Name { get; set; } = ""; }
public sealed class ReportRow
{
    public string BillId { get; set; } = "";
    public string Hidden { get; set; } = "hidden-marker";
}
public sealed class FakeReportHandler : IUnrelated, IFakeReportHandler
{
    public int Calls, SummaryCalls;
    public bool Throw, ReturnNull;
    public ReportInput? Input;
    public CancellationToken Token;
    public void Ping() { }
    public Task<ReportOutput<ReportHeader, ReportRow>> Handle(string input, CancellationToken token) => throw new Exception("Wrong overload");
    public Task<ReportOutput<ReportHeader, ReportRow>> Handle(ReportInput input, CancellationToken token)
    {
        Calls++;
        Input = input;
        Token = token;
        if (Throw) throw new InvalidOperationException("handler-failed");
        return Task.FromResult(ReturnNull ? null! : new ReportOutput<ReportHeader, ReportRow>("تست", new() { Name = "تست" },
            Enumerable.Range(0, input.Count).Select(_ => new ReportRow { BillId = input.BillId })));
    }
    public Task<ReportOutput<ReportHeader, ReportRow>> HandleSummary(ReportInput input, CancellationToken token)
    {
        SummaryCalls++;
        return Handle(input, token);
    }
}

sealed class Pipeline : IDisposable
{
    private readonly ServiceProvider _provider;
    public readonly FakeReportHandler Handler = new();
    public IReportGenerator Generator { get; }
    public Job? LastJob;
    public ServerReportsGetByIdDto Record = null!;
    public List<ReportCompletionNotification> Notifications = new();
    public string? TargetUser;
    public bool FailSave, FailQueue, FailTransport;
    public int Failures;
    public TaskCompletionSource? SaveGate;
    public TaskCompletionSource SaveEntered = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public Pipeline()
    {
        _provider = new ServiceCollection().AddSingleton<IFakeReportHandler>(Handler).BuildServiceProvider();
        var create = Proxy.For<IServerReportsCreateHandler>((_, args) =>
        {
            var input = (ServerReportsCreateDto)args[0]!;
            Record = new() { Id = input.Id, UserId = input.UserId, ConnectionId = input.ConnectionId, ReportName = input.ReportName,
                HeaderType = input.HeaderType, DataType = input.DataType, ReportInputType = input.ReportInputType,
                ReportInputJson = input.ReportInputJson, HandlerKey = input.HandlerKey };
            return Task.CompletedTask;
        });
        var update = Proxy.For<IServerReportsUpdateHandler>((method, args) =>
        {
            if (method.Name == "Handle") return Save((ServerReportsUpdateDto)args[0]!);
            Failures++;
            Record.ErrorDateTimeJalali = "recorded";
            return Task.CompletedTask;
        });
        var query = Proxy.For<IServerReportsGetByIdService>((_, _) => Task.FromResult(Record));
        var duplicateQuery = Proxy.For<IServerReportsQueryService>((_, _) => throw new Exception("Unexpected query"));
        var background = Proxy.For<IBackgroundJobClient>((method, args) =>
        {
            if (FailQueue) throw new InvalidOperationException("queue-failed");
            LastJob = (Job)args[0]!;
            return "test-job";
        });
        var client = Proxy.For<INotifyHub>((_, args) =>
        {
            if (!File.Exists(Record.ReportPath)) throw new Exception("Notification preceded file persistence");
            if (FailTransport) return Task.FromException(new InvalidOperationException("transport-failed"));
            Notifications.Add((ReportCompletionNotification)args[0]!);
            return Task.CompletedTask;
        });
        var clients = Proxy.For<IHubClients<INotifyHub>>((method, args) =>
        {
            if (method.Name != "User") throw new Exception("Wrong recipient routing");
            TargetUser = (string)args[0]!;
            return client;
        });
        var hub = Proxy.For<IHubContext<NotifyHub, INotifyHub>>((_, _) => clients);
        var notifier = new ReportCompletionNotifier(hub, NullLogger<ReportCompletionNotifier>.Instance);
        var type = typeof(IReportGenerator).Assembly.GetType("Aban360.Api.Cronjobs.ReportGenerator", true)!;
        var logger = Activator.CreateInstance(typeof(NullLogger<>).MakeGenericType(type))!;
        Generator = (IReportGenerator)Activator.CreateInstance(type, create, update, query, duplicateQuery, _provider, notifier, background, logger)!;
    }
    public Task Enqueue(ReportInput input, CancellationToken token = default, bool summary = false) => summary
        ? Generator.FireAndInform(input, token, Handler.HandleSummary, new AppUserManual(), "گزارش تست", "old-connection", "HandleSummary", new[] { "Hidden" })
        : Generator.FireAndInform(input, token, Handler.Handle, new AppUserManual(), "گزارش تست", "old-connection", excludedProperties: new[] { "Hidden" });
    public Task Execute(string? method = null) => Generator.DoFireAndInform((ServerReportsCreateDto)LastJob!.Args[0],
        method ?? (string)LastJob.Args[1], (string[]?)LastJob.Args[2]);
    private async Task Save(ServerReportsUpdateDto input)
    {
        SaveEntered.TrySetResult();
        if (SaveGate is not null) await SaveGate.Task;
        if (FailSave) throw new InvalidOperationException("save-failed");
        if (!File.Exists(input.ReportPath)) throw new Exception("Saved path does not exist");
        Record.ReportPath = input.ReportPath;
        Record.ErrorDateTimeJalali = null;
    }
    public void Dispose() => _provider.Dispose();
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
