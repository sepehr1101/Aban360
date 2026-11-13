using System.Collections;
using System.Reflection;
using System.Security.Claims;
using Aban360.Common.ApplicationUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

static class CrudChecks
{
    public static async Task<int> Run(Contract[] contracts, ControllerActionDescriptor[] actions)
    {
        int checks = 0, covered = 0, scenarios = 0;
        void Check(bool condition, string text) { if (!condition) throw new InvalidOperationException(text); checks++; }
        foreach (var expected in contracts.Where(c => c.Project == "Api" && c.Action is "Create" or "Update" or "Delete"))
        {
            var action = actions.Single(a => a.AttributeRouteInfo?.Template == expected.Route && a.ActionName == expected.Action);
            var parameters = action.MethodInfo.GetParameters();
            if (parameters.Length != 2 || parameters[1].ParameterType != typeof(CancellationToken)
                || action.MethodInfo.ReturnType != typeof(Task<IActionResult>) || !parameters[0].ParameterType.Name.EndsWith("Dto")) continue;
            var ctor = action.ControllerTypeInfo.GetConstructors().Single();
            var dependencies = ctor.GetParameters();
            // Complex notification/file/job workflows have their own focused tests.
            if (dependencies.Any(p => !p.ParameterType.Name.EndsWith("Handler") && p.ParameterType.Name != "IUnitOfWork")) continue;
            var handlers = dependencies.Select(p => p.ParameterType).Where(t => t.GetMethods().Any(m => m.Name == "Handle" && m.ReturnType == typeof(Task)
                && m.GetParameters().Any(p => p.ParameterType == parameters[0].ParameterType))).Distinct().ToArray();
            if (handlers.Length != 1) continue;
            var responseType = action.MethodInfo.GetCustomAttributes<ProducesResponseTypeAttribute>().FirstOrDefault(r => r.Type.IsGenericType
                && r.Type.Name.StartsWith("ApiResponseEnvelope") && r.Type.GetGenericArguments().Single() == parameters[0].ParameterType);
            if (responseType == null) continue;
            bool saves = dependencies.Any(p => p.ParameterType.Name == "IUnitOfWork");
            int steps = saves ? 2 : 1;
            var handler = handlers.Single();
            covered++;
            foreach (var mode in new[] { "success", "handler-failure", "handler-cancel", "await-handler" }
                .Concat(saves ? new[] { "save-failure", "save-cancel", "await-save" } : Array.Empty<string>()))
            {
                scenarios++;
                using var cancellation = new CancellationTokenSource();
                if (mode.EndsWith("cancel")) cancellation.Cancel();
                var token = cancellation.Token;
                var payload = Sample(parameters[0].ParameterType)!;
                var calls = new List<string>();
                var gate = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
                var marker = new InvalidOperationException("intentional-crud-test-failure");
                object Return(MethodInfo method, int step)
                {
                    bool generic = method.ReturnType.IsGenericType;
                    bool fail = mode == (step == 1 ? "handler-failure" : "save-failure");
                    bool cancel = mode == (step == 1 ? "handler-cancel" : "save-cancel");
                    bool delayed = mode == (step == 1 ? "await-handler" : "await-save");
                    async Task Complete() { if (delayed) await gate.Task; if (fail) throw marker; if (cancel) throw new OperationCanceledException(token); }
                    async Task<int> Save() { await Complete(); return 1; }
                    return generic ? Save() : Complete();
                }
                var state = new InvocationState((contract, method, args) =>
                {
                    if (contract == handler && method.Name == "Handle")
                    {
                        Check(calls.Count == 0, "Handler ordering changed: " + expected.Route);
                        Check(args.Any(a => ReferenceEquals(a, payload)), "Wrong input object: " + expected.Route);
                        Check(args.OfType<CancellationToken>().Single() == token, "Handler lost cancellation: " + expected.Route);
                        foreach (var user in args.OfType<IAppUser>()) Check(user.UserId == Guid.Parse("22222222-2222-2222-2222-222222222222"), "Current user changed");
                        calls.Add("handle"); return Return(method, 1);
                    }
                    if (contract.Name == "IUnitOfWork" && method.Name == "SaveChangesAsync")
                    {
                        Check(calls.SequenceEqual(new[] { "handle" }), "Save happened before/without handler: " + expected.Route);
                        Check(args.OfType<CancellationToken>().Single() == token, "Save lost cancellation: " + expected.Route);
                        calls.Add("save"); return Return(method, 2);
                    }
                    throw new Exception("Unexpected CRUD dependency: " + contract.Name + "." + method.Name + " " + expected.Route);
                });
                var controller = (ControllerBase)ctor.Invoke(dependencies.Select(p =>
                {
                    var proxy = (WorkflowProxy)DispatchProxy.Create(p.ParameterType, typeof(WorkflowProxy)); proxy.Contract = p.ParameterType; proxy.State = state; return (object)proxy;
                }).ToArray());
                controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    { new Claim(ClaimTypes.UserData, "22222222-2222-2222-2222-222222222222") }, "regression")) } };
                var pending = (Task<IActionResult>)action.MethodInfo.Invoke(controller, new[] { payload, (object)token })!;
                if (mode.StartsWith("await-"))
                {
                    if (pending.IsFaulted) await pending;
                    Check(!pending.IsCompleted && calls.Count == (mode == "await-handler" ? 1 : 2), "Action did not await dependency: " + expected.Route);
                    gate.SetResult(true);
                }
                if (mode.EndsWith("failure"))
                {
                    try { await pending; throw new Exception("Failure swallowed: " + expected.Route); }
                    catch (InvalidOperationException e) when (ReferenceEquals(e, marker)) { checks++; }
                }
                else if (mode.EndsWith("cancel"))
                {
                    try { await pending; throw new Exception("Cancellation swallowed: " + expected.Route); }
                    catch (OperationCanceledException) { checks++; }
                }
                else
                {
                    var result = await pending;
                    Check(result is OkObjectResult { StatusCode: 200 }, "CRUD status changed: " + expected.Route);
                    var value = ((OkObjectResult)result).Value;
                    if (typeof(Aban360.Api.Controllers.V1.BaseController).IsAssignableFrom(action.ControllerTypeInfo.AsType()))
                        Check(value != null && ReferenceEquals(value.GetType().GetProperty("Data")?.GetValue(value), payload)
                            && value.GetType().GetProperty("Success")?.GetValue(value) is true
                            && Equals(value.GetType().GetProperty("HttpStatusCode")?.GetValue(value), 200), "CRUD envelope changed: " + expected.Route);
                    else Check(ReferenceEquals(value, payload), "Raw CRUD result changed: " + expected.Route);
                }
                Check(calls.Count == (mode.StartsWith("handler-") ? 1 : steps), "Unexpected write/retry after handler failure: " + expected.Route);
            }
        }
        Check(covered > 0, "No simple CRUD actions were tested.");
        Console.WriteLine($"PASS: {checks} CRUD checks; {covered} actions; {scenarios} success/error/cancellation/await scenarios.");
        return checks;
    }
    static object? Sample(Type type, int depth = 0)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;
        if (type == typeof(string)) return "sample";
        if (type == typeof(int)) return 17;
        if (type == typeof(short)) return (short)17;
        if (type == typeof(long)) return 17L;
        if (type == typeof(bool)) return true;
        if (type == typeof(Guid)) return Guid.Parse("22222222-2222-2222-2222-222222222222");
        if (type.IsEnum) return Enum.GetValues(type).GetValue(0);
        if (type.IsValueType) return Activator.CreateInstance(type);
        if (depth > 3) return null;
        if (type.IsArray) return Array.CreateInstance(type.GetElementType()!, 0);
        if (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type))
            return Activator.CreateInstance(typeof(List<>).MakeGenericType(type.GetGenericArguments()[0]));
        if (type.IsAbstract || type.IsInterface) return null;
        var ctor = type.GetConstructors().OrderBy(c => c.GetParameters().Length).First();
        var value = ctor.Invoke(ctor.GetParameters().Select(p => Sample(p.ParameterType, depth + 1)).ToArray());
        foreach (var property in type.GetProperties().Where(p => p.CanWrite && p.SetMethod?.IsPublic == true && p.GetIndexParameters().Length == 0))
            property.SetValue(value, Sample(property.PropertyType, depth + 1));
        return value;
    }
}

