using System.Collections;
using System.Reflection;
using System.Security.Claims;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

static class WorkflowChecks
{
    public static async Task<int> Run(ControllerActionDescriptor[] actions)
    {
        int checks = 0;
        void Check(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); checks++; }
        ControllerActionDescriptor Action(string route) => actions.Single(a => a.AttributeRouteInfo?.Template == route);
        var directory = Path.Combine(Path.GetTempPath(), "aban360-regression-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            var bytes = new byte[] { 80, 75, 3, 4, 11, 22, 33 };
            var path = Path.Combine(directory, "archive.zip"); await File.WriteAllBytesAsync(path, bytes);
            using var cancellation = new CancellationTokenSource(); var token = cancellation.Token;
            var download = Action("v1/collect-bills/download/{fileName}");
            var state = new InvocationState((contract, method, values) =>
            {
                Check(contract.Name == "ICollectBillsDetailFileDownloadHandler" && method.Name == "Handle", "Unexpected download dependency");
                Check((string)values[0]! == "archive.zip" && (CancellationToken)values[1]! == token, "Download arguments/token changed");
                return new CollectBillsGetZipFileInfo(path, "archive.zip");
            });
            var result = (FileStreamResult)Call(download, Create(download, state), "archive.zip", token)!;
            Check(result.ContentType == "application/zip" && result.FileDownloadName == "archive.zip", "ZIP response headers changed");
            using (result.FileStream)
            using (var memory = new MemoryStream()) { await result.FileStream.CopyToAsync(memory); Check(memory.ToArray().SequenceEqual(bytes), "ZIP bytes changed"); }
            Check(state.Calls == 1, "Download handler invoked more than once");
            state = new InvocationState((_, _, _) => new CollectBillsGetZipFileInfo(Path.Combine(directory, "missing.zip"), "missing.zip"));
            try { Call(download, Create(download, state), "missing.zip", token); throw new Exception("Missing download was accepted"); }
            catch (InvalidBillCommandException) { checks++; }
            Check(state.Calls == 1, "Missing download caused another dependency call");
            var marker = new InvalidOperationException("controlled-test-failure");
            state = new InvocationState((_, _, _) => throw marker);
            try { Call(download, Create(download, state), "archive.zip", token); throw new Exception("Download failure was swallowed"); }
            catch (InvalidOperationException e) when (ReferenceEquals(e, marker)) { checks++; }

            var job = Action("v1/collect-bills/send-file/{day}");
            var release = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var before = DateTime.Now.AddDays(-3);
            state = new InvocationState((contract, method, values) =>
            {
                Check(contract.Name == "ICollectBillsDetailJobService" && method.Name == "Initialize", "Wrong job dependency");
                Check((CancellationToken)values[1]! == token, "Job cancellation token was lost");
                Check((DateTime)values[0]! >= before && (DateTime)values[0]! <= DateTime.Now.AddDays(-3), "Job date offset changed");
                return release.Task;
            });
            var pending = (Task<IActionResult>)Call(job, Create(job, state), -3, token)!;
            Check(!pending.IsCompleted && state.Calls == 1, "Response returned before job initialization finished");
            release.SetResult(true); Check(await pending is OkResult, "Job response changed");
            cancellation.Cancel();
            state = new InvocationState((_, _, _) => Task.FromCanceled(token));
            try { await (Task<IActionResult>)Call(job, Create(job, state), -3, token)!; throw new Exception("Job cancellation was swallowed"); }
            catch (OperationCanceledException) { checks++; }

            var sms = Action("v1/sms/test-batch");
            var send = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            object? payload = null;
            state = new InvocationState((contract, method, values) =>
            {
                Check(contract.Name == "ISmsOldHandler" && method.Name == "Send", "Unexpected SMS dependency");
                Check(values.Length == 2 && values[1]?.ToString() == "Notification", "SMS overload/message type changed");
                payload = values[0]; Check(((IEnumerable)payload!).Cast<object>().Count() == 3, "SMS batch changed");
                return send.Task;
            });
            var smsTask = (Task<IActionResult>)Call(sms, Create(sms, state), CancellationToken.None)!;
            if (smsTask.IsFaulted) await smsTask;
            Check(!smsTask.IsCompleted && state.Calls == 1, "SMS action stopped awaiting its handler");
            send.SetResult(true); var response = (OkObjectResult)await smsTask;
            Check(response.StatusCode == 200 && ReferenceEquals(response.Value!.GetType().GetProperty("Data")!.GetValue(response.Value), payload), "SMS envelope/data changed");
            state = new InvocationState((_, _, _) => Task.FromException(marker));
            try { await (Task<IActionResult>)Call(sms, Create(sms, state), CancellationToken.None)!; throw new Exception("SMS failure was swallowed"); }
            catch (InvalidOperationException e) when (ReferenceEquals(e, marker)) { checks++; }
            Check(state.Calls == 1, "SMS failure caused an unexpected retry");
        }
        finally
        {
            // Only this test's uniquely named directory is removed.
            foreach (var file in Directory.GetFiles(directory)) File.Delete(file);
            Directory.Delete(directory);
        }
        Console.WriteLine($"PASS: {checks} focused workflow checks (ZIP, missing file, job date/await/cancellation, isolated SMS success/failure).");
        return checks;
    }
    static ControllerBase Create(ControllerActionDescriptor action, InvocationState state)
    {
        var ctor = action.ControllerTypeInfo.GetConstructors().Single();
        var controller = (ControllerBase)ctor.Invoke(ctor.GetParameters().Select(p =>
        {
            var value = DispatchProxy.Create(p.ParameterType, typeof(WorkflowProxy));
            var proxy = (WorkflowProxy)value; proxy.Contract = p.ParameterType; proxy.State = state; return value;
        }).ToArray());
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.UserData, "22222222-2222-2222-2222-222222222222") }, "regression")) } };
        return controller;
    }
    static object? Call(ControllerActionDescriptor action, object controller, params object[] values)
    {
        try { return action.MethodInfo.Invoke(controller, values); }
        catch (TargetInvocationException e) when (e.InnerException != null) { System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(e.InnerException).Throw(); throw; }
    }
}
public sealed class InvocationState(Func<Type, MethodInfo, object?[], object?> callback)
{
    public int Calls;
    public object? Invoke(Type contract, MethodInfo method, object?[] values) { Calls++; return callback(contract, method, values); }
}
public class WorkflowProxy : DispatchProxy
{
    public Type Contract = null!; public InvocationState State = null!;
    protected override object? Invoke(MethodInfo? method, object?[]? args) => State.Invoke(Contract, method!, args ?? []);
}

