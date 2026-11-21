using System.Collections;
using System.IO.Compression;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Application.Features.CollectBills.Services;
using Aban360.CalculationPool.Application.Extensions;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.CollectBills.Inputs;
using Aban360.CalculationPool.Domain.Features.CollectBills.Outputs;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Hangfire;
using Hangfire.Common;
using Hangfire.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal static class CollectBillsRuntimeChecks
{
    public static async Task<int> Run()
    {
        int checks = 0;
        void Check(bool value, string message) { if (!value) throw new Exception(message); checks++; }
        var services = new ServiceCollection(); services.AddLogging(); services.AddCalculationPoolApplicationInjections();
        var descriptor = services.Single(s => s.ServiceType == typeof(ICollectBillsApplicationService));
        Check(descriptor.ImplementationType == typeof(CollectBillsApplicationService) && descriptor.Lifetime == ServiceLifetime.Scoped, "Incorrect production registration");
        var rows = new List<CollectBillsDataDto> { new() { Row = "first" }, new() { Row = "second" } };
        var reportRows = new List<CollectBillsDetailWithLastStepDataOutputDto> { new() };
        Func<object?> queryResult = () => rows;
        Func<object?> reportResult = () => reportRows;
        object? seenInput = null;
        services.AddScoped<ICollectBillsQueryService>(_ => CollectBillsProbe.Make<ICollectBillsQueryService>((m, a) => { seenInput = a![0]; return CollectBillsProbe.Result(m.ReturnType, queryResult()); }));
        services.AddScoped<ICollectBillsDetailQueryService>(_ => CollectBillsProbe.Make<ICollectBillsDetailQueryService>((m, a) => { seenInput = a![0]; return CollectBillsProbe.Result(m.ReturnType, reportResult()); }));
        services.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
        foreach (var p in typeof(CollectBillsApplicationService).GetConstructors().Single().GetParameters().Where(p => p.ParameterType != typeof(IConfiguration) && p.ParameterType != typeof(ICollectBillsQueryService) && p.ParameterType != typeof(ICollectBillsDetailQueryService)))
            services.AddScoped(p.ParameterType, _ => CollectBillsProbe.Make(p.ParameterType, (m, a) => throw new Exception("Unexpected external call " + m.Name)));
        using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
        using var scope = provider.CreateScope(); using var secondScope = provider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICollectBillsApplicationService>();
        Check(ReferenceEquals(service, scope.ServiceProvider.GetRequiredService<ICollectBillsApplicationService>()) && !ReferenceEquals(service, secondScope.ServiceProvider.GetRequiredService<ICollectBillsApplicationService>()), "Scoped lifetime changed");
        var legacyScoped = scope.ServiceProvider.GetRequiredService<ICollectBillsDetailJobService>();
        Check(legacyScoped is CollectBillsDetailJobService && ReferenceEquals(typeof(CollectBillsDetailJobService).GetField("_service", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(legacyScoped), service), "Saved-job activation does not use the scoped unified service");
        var input = new CollectBillsDetialReportInputDto();
        var report = await service.GetDetailWithLastStepAsync(input, default);
        Check(ReferenceEquals(seenInput, input) && report.ReportHeader.RecordCount == 1 && report.ReportData.SequenceEqual(reportRows), "Report data/count/input changed");
        reportResult = () => new List<CollectBillsDetailWithLastStepDataOutputDto>();
        Check((await service.GetDetailWithLastStepAsync(input, default)).ReportHeader.RecordCount == 0, "Empty report changed");
        var failure = new InvalidOperationException("query-failed"); reportResult = () => throw failure;
        try { await service.GetDetailWithLastStepAsync(input, default); throw new Exception("Failure swallowed"); } catch (InvalidOperationException e) { Check(ReferenceEquals(e, failure), "Failure changed"); }
        var pending = new TaskCompletionSource<IEnumerable<CollectBillsDetailWithLastStepDataOutputDto>>(TaskCreationOptions.RunContinuationsAsynchronously);
        reportResult = () => pending.Task; var waiting = service.GetDetailWithLastStepAsync(input, default);
        Check(!waiting.IsCompleted, "Query was not awaited"); pending.SetResult(reportRows); Check((await waiting).ReportHeader.RecordCount == 1, "Pending query result changed");
        reportResult = () => Task.FromCanceled<IEnumerable<CollectBillsDetailWithLastStepDataOutputDto>>(new CancellationToken(true));
        try { await service.GetDetailWithLastStepAsync(input, default); throw new Exception("Cancellation swallowed"); } catch (OperationCanceledException) { Check(true, "Query cancellation"); }

        string temp = Path.Combine(Path.GetTempPath(), "Aban360-CollectBills-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(temp);
        foreach (string field in new[] { "_basePath", "_CollectBillsDetailFileDownloadfolderPath" }) typeof(CollectBillsApplicationService).GetField(field, BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(service, temp);
        try
        {
            var zip = await service.CreateZip(new[] { "first", "second" }, "1405/01/01", "1405/01/01");
            CheckZip(zip, "first" + Environment.NewLine + "second" + Environment.NewLine);
            var download = service.CollectBillsDetailFileDownloadAsync(zip.FileName, default);
            Check(download.FilePath == zip.FilePath && download.FileName == zip.FileName, "Download path changed");
            var controllerInstance = new Aban360.Api.Controllers.V1.CalculationPool.CollectBills.CollectBillsController(service);
            var response = controllerInstance.DownloadByFileName(zip.FileName, default);
            Check(response.ContentType == "application/zip" && response.FileDownloadName == zip.FileName, "Download HTTP metadata changed");
            using (response.FileStream)
            using (var copy = new MemoryStream()) { await response.FileStream.CopyToAsync(copy); Check(copy.ToArray().SequenceEqual(File.ReadAllBytes(zip.FilePath)), "Controller download bytes changed"); }
            var generated = await service.CollectBillsFileCreateAsync("1405/01/02", null!, default);
            CheckZip(generated, "first" + Environment.NewLine + "second" + Environment.NewLine);
            Check(seenInput is Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands.CollectBillsGetDataToSendInputDto dates && dates.FromDateJalali == "1405/01/02" && dates.ToDateJalali == dates.FromDateJalali, "File query date changed");
            queryResult = () => throw failure;
            try { await service.CollectBillsFileCreateAsync("1405/01/03", null!, default); throw new Exception("File query failure swallowed"); } catch (InvalidOperationException e) { Check(ReferenceEquals(e, failure), "File query failure changed"); }
        }
        finally { foreach (var path in Directory.GetFiles(temp)) File.Delete(path); Directory.Delete(temp); }
        void CheckZip(CollectBillsGetZipFileInfo info, string expected)
        {
            Check(File.Exists(info.FilePath), "ZIP missing");
            using var zip = ZipFile.OpenRead(info.FilePath); Check(zip.Entries.Count == 1, "ZIP entry count changed");
            using var stream = zip.Entries.Single().Open(); using var bytes = new MemoryStream(); stream.CopyTo(bytes);
            var contents = bytes.ToArray(); Check(contents.Take(3).SequenceEqual(new byte[] { 239, 187, 191 }), "UTF8 BOM changed");
            Check(Encoding.UTF8.GetString(contents[3..]) == expected, "ZIP text content changed");
        }

        // Previously serialized Hangfire jobs must still resolve to the old concrete type,
        // then delegate the exact arguments/task to the sole business implementation.
        foreach (string oldName in new[] { "Initialize", "CreateZip", "CreateFile", "Upload", "SetFileDetail", "Confirm" })
        {
            var legacyMethod = typeof(CollectBillsDetailJobService).GetMethod(oldName)!;
            var newMethod = typeof(ICollectBillsApplicationService).GetMethod(oldName)!;
            Check(legacyMethod.ReturnType == newMethod.ReturnType && legacyMethod.GetParameters().Select(p => p.ParameterType).SequenceEqual(newMethod.GetParameters().Select(p => p.ParameterType)), "Legacy job signature changed");
            object?[] arguments = legacyMethod.GetParameters().Select(p => p.ParameterType == typeof(Guid) ? (object)Guid.NewGuid() : p.ParameterType == typeof(string) ? "sample" : p.ParameterType == typeof(DateTime?) ? DateTime.Today : p.ParameterType == typeof(CancellationToken) ? CancellationToken.None : (object)new List<string> { "row" }).ToArray();
            Task forwardedTask = legacyMethod.ReturnType == typeof(Task) ? Task.CompletedTask : Task.FromResult(new CollectBillsGetZipFileInfo("file", "name"));
            var proxy = CollectBillsProbe.Make<ICollectBillsApplicationService>((m, a) => { Check(m.Name == legacyMethod.Name && a!.SequenceEqual(arguments), "Legacy job arguments changed"); return forwardedTask; });
            var legacy = new CollectBillsDetailJobService(proxy);
            Check(ReferenceEquals(legacyMethod.Invoke(legacy, arguments), forwardedTask), "Legacy job task not forwarded");
            var job = new Job(typeof(CollectBillsDetailJobService), legacyMethod, arguments);
            var restored = InvocationData.SerializeJob(job).DeserializeJob();
            Check(restored.Type == typeof(CollectBillsDetailJobService) && restored.Method == legacyMethod, "Legacy Hangfire deserialization changed");
            if (typeof(ICollectBillsDetailJobService).GetMethod(oldName) is { } interfaceMethod)
            {
                var interfaceJob = InvocationData.SerializeJob(new Job(typeof(ICollectBillsDetailJobService), interfaceMethod, arguments)).DeserializeJob();
                Check(interfaceJob.Type == typeof(ICollectBillsDetailJobService) && interfaceJob.Method == interfaceMethod, "Legacy interface job deserialization changed");
            }
            var adapterPending = legacyMethod.ReturnType == typeof(Task) ? (Task)new TaskCompletionSource().Task : new TaskCompletionSource<CollectBillsGetZipFileInfo>().Task;
            var pendingProxy = CollectBillsProbe.Make<ICollectBillsApplicationService>((m, a) => adapterPending);
            Check(ReferenceEquals(legacyMethod.Invoke(new CollectBillsDetailJobService(pendingProxy), arguments), adapterPending), "Legacy job lost pending completion");
            var failedTask = legacyMethod.ReturnType == typeof(Task) ? Task.FromException(failure) : (Task)Task.FromException<CollectBillsGetZipFileInfo>(failure);
            var failureProxy = CollectBillsProbe.Make<ICollectBillsApplicationService>((m, a) => failedTask);
            try { await (Task)legacyMethod.Invoke(new CollectBillsDetailJobService(failureProxy), arguments)!; throw new Exception("Legacy failure swallowed"); } catch (InvalidOperationException e) { Check(ReferenceEquals(e, failure), "Legacy failure changed"); }
            var cancelled = legacyMethod.ReturnType == typeof(Task) ? Task.FromCanceled(new CancellationToken(true)) : (Task)Task.FromCanceled<CollectBillsGetZipFileInfo>(new CancellationToken(true));
            var cancelProxy = CollectBillsProbe.Make<ICollectBillsApplicationService>((m, a) => cancelled);
            try { await (Task)legacyMethod.Invoke(new CollectBillsDetailJobService(cancelProxy), arguments)!; throw new Exception("Legacy cancellation swallowed"); } catch (OperationCanceledException) { Check(true, "Legacy cancellation propagated"); }
        }
        Console.WriteLine($"PASS CollectBills runtime: {checks} checks; scoped DI, report/file behavior, ZIP bytes and six legacy Hangfire signatures. No database/external calls.");
        return checks;
    }
}
public class CollectBillsProbe : DispatchProxy
{
    public Func<MethodInfo, object?[]?, object?> Callback = null!;
    protected override object? Invoke(MethodInfo? method, object?[]? args) => Callback(method!, args);
    public static T Make<T>(Func<MethodInfo, object?[]?, object?> callback) => (T)Make(typeof(T), callback);
    public static object Make(Type type, Func<MethodInfo, object?[]?, object?> callback) { var value = Create(type, typeof(CollectBillsProbe)); ((CollectBillsProbe)value).Callback = callback; return value; }
    public static object? Result(Type type, object? value)
    {
        if (value is Task) return value;
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>) ? typeof(Task).GetMethod(nameof(Task.FromResult))!.MakeGenericMethod(type.GenericTypeArguments[0]).Invoke(null, new[] { value }) : value;
    }
}
