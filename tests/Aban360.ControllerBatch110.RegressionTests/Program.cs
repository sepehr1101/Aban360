using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Aban360.Api.Filters;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;
using Aban360.Api.Controllers.V1;
using Aban360.Common.ApplicationUser;
using Hangfire.Common;
using Hangfire.States;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Aban360.Batch110ControllerTests;

public static class Regression
{
    public static int Checks;
    public static readonly Guid UserId = Guid.Parse("2a7416be-9db9-49bc-ac12-e698e1c7d970");
    public static ClaimsPrincipal Principal() => new(new ClaimsIdentity(new[]
    {
        new Claim(ClaimTypes.UserData, UserId.ToString()), new Claim(ClaimTypes.Name, "reviewer"),
        new Claim(ClaimTypes.GivenName, "Regression Reviewer"), new Claim(ClaimTypes.Role, "reviewer")
    }, "regression"));
    public static void Check(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
        Checks++;
    }

    public static async Task Main()
    {
        var names = new HashSet<string> { "RequestSiphonController", "RequestSubscriptionController", "RequestTrackingController", "RequestUserController", "RequestWaterMeterController", "RequestWaterMeterSiphonController", "RequestWaterMeterTagController", "EstateController", "InstallationDateAssignmentController", "SubscriptionAssignmentController", "ChangeMeterReasonController", "WaterMeterTagController", "DiscountTypeController", "IndividualTagController", "BillReturnCauseController", "VirtualCategoryController", "AbonmanController", "NerkhController", "SController", "Table1Controller" };
        var current = typeof(BaseController).Assembly.GetTypes().Where(t => names.Contains(t.Name)).ToArray();
        var original = typeof(Regression).Assembly.GetTypes().Where(t => t.Namespace?.Contains(".BeforeBatch110.") == true
            && typeof(ControllerBase).IsAssignableFrom(t) && !t.IsAbstract).ToArray();
        Check(current.Length == 19 && original.Length == 69, "Missing current controllers or pinned original snapshots.");
        var currentMethods = Endpoints(current).ToArray();
        var cases = Endpoints(original).Select(old =>
        {
            var matches = currentMethods.Where(n => n.Route == old.Route && n.Verbs.SequenceEqual(old.Verbs)).ToArray();
            Check(matches.Length == 1, "Missing/ambiguous merged action: " + old.Route);
            return new Comparison(old, matches.Single());
        }).ToArray();
        Check(cases.Length == 72 && currentMethods.Length == 72, "Expected all 72 batch actions.");

        int behavioralCases = 0;
        foreach (var pair in cases)
        {
            using var cancellation = new CancellationTokenSource();
            var token = cancellation.Token;
            var before = await Execute(pair.Old, new ProbeState { Caller = token }, token);
            var after = await Execute(pair.New, new ProbeState { Caller = token }, token);
            Equal(before, after, pair.Old.Route + " success");
            behavioralCases++;
            AssertSuccessfulBehavior(pair.New, after);

            Equal(await Execute(pair.Old, new ProbeState { Caller = token, EmptyCollections = true }, token),
                await Execute(pair.New, new ProbeState { Caller = token, EmptyCollections = true }, token),
                pair.Old.Route + " empty collections");
            behavioralCases++;
            for (int failure = 1; failure <= before.Calls.Length; failure++)
            {
                Equal(await Execute(pair.Old, new ProbeState { Caller = token, FailAt = failure }, token),
                    await Execute(pair.New, new ProbeState { Caller = token, FailAt = failure }, token),
                    pair.Old.Route + " failure at call " + failure);
                behavioralCases++;
            }
            cancellation.Cancel();
            Equal(await Execute(pair.Old, new ProbeState { Caller = token, CancelAt = 1 }, token),
                await Execute(pair.New, new ProbeState { Caller = token, CancelAt = 1 }, token),
                pair.Old.Route + " cancellation");
            behavioralCases++;

            // Keep each awaited dependency pending to detect premature saves, responses or background jobs.
            foreach (var step in before.AsyncSteps)
            {
                foreach (var endpoint in new[] { pair.Old, pair.New })
                {
                    var state = new ProbeState { GateAt = step };
                    var pending = Execute(endpoint, state, CancellationToken.None);
                    Check(!pending.IsCompleted && state.Calls.Count == step,
                        "Action did not await dependency: " + endpoint.Route + " / " + step);
                    state.Release.TrySetResult(true);
                    Check((await pending).Error == null, "Delayed dependency failed: " + endpoint.Route);
                }
                behavioralCases++;
            }
            Console.WriteLine("PASS behavior: " + pair.Old.Route);
        }

        VerifyMvcAndPermissions(cases, current);
        Console.WriteLine($"PASS: {Checks} checks; {current.Length} controllers; 72 actions; {behavioralCases} before/after behavioral scenarios. No database access.");
    }

    static void VerifyMvcAndPermissions(Comparison[] cases, Type[] current)
    {
        using var provider = new ServiceCollection().AddLogging().AddControllers()
            .ConfigureApplicationPartManager(manager => manager.ApplicationParts.Clear())
            .AddApplicationPart(typeof(BaseController).Assembly).Services.BuildServiceProvider();
        var descriptors = provider.GetRequiredService<IActionDescriptorCollectionProvider>().ActionDescriptors.Items
            .OfType<ControllerActionDescriptor>().ToArray();
        foreach (var pair in cases)
        {
            var matches = descriptors.Where(a => a.ControllerTypeInfo.AsType() == pair.New.Type
                && a.AttributeRouteInfo?.Template == pair.New.Route && a.ActionName == pair.Old.Method.Name).ToArray();
            Check(matches.Length == 1, "Missing MVC action: " + pair.New.Route);
            var descriptor = matches.Single();
            var verbs = descriptor.ActionConstraints!.OfType<Microsoft.AspNetCore.Mvc.ActionConstraints.HttpMethodActionConstraint>()
                .SelectMany(c => c.HttpMethods).Distinct().Order().ToArray();
            Check(verbs.SequenceEqual(pair.Old.Verbs), "HTTP methods changed: " + pair.Old.Route);
            foreach (var verb in verbs)
                Check(descriptors.Count(d => string.Equals(d.AttributeRouteInfo?.Template, pair.New.Route, StringComparison.OrdinalIgnoreCase)
                    && d.ActionConstraints!.OfType<Microsoft.AspNetCore.Mvc.ActionConstraints.HttpMethodActionConstraint>().Any(c => c.HttpMethods.Contains(verb))) == 1,
                    "Ambiguous API route: " + verb + " " + pair.New.Route);
            Check(pair.Old.Type.BaseType == pair.New.Type.BaseType, "Base controller changed: " + pair.New.Route);
            string Signature(ParameterInfo p) => p.ParameterType.FullName + " " + p.Name + " " + p.IsOptional
                + " " + string.Join("|", p.GetCustomAttributes().Select(a => a.GetType().FullName).Order());
            Check(pair.Old.Method.GetParameters().Select(Signature).SequenceEqual(pair.New.Method.GetParameters().Select(Signature)),
                "Parameters or explicit bindings changed: " + pair.New.Route);
            string[] Responses(MethodInfo m) => m.GetCustomAttributes<ProducesResponseTypeAttribute>()
                .Select(a => a.Type.FullName + ":" + a.StatusCode).Order().ToArray();
            Check(Responses(pair.Old.Method).SequenceEqual(Responses(pair.New.Method)), "Response metadata changed: " + pair.New.Route);
            bool Has<T>(Endpoint e) where T : Attribute => e.Type.IsDefined(typeof(T), true) || e.Method.IsDefined(typeof(T), true);
            Check(Has<AuthorizeAttribute>(pair.Old) == Has<AuthorizeAttribute>(pair.New)
                && Has<AllowAnonymousAttribute>(pair.Old) == Has<AllowAnonymousAttribute>(pair.New), "Authorization metadata changed: " + pair.New.Route);
            var oldFilter = pair.Old.Method.GetCustomAttributes<TypeFilterAttribute>().SingleOrDefault(a => a.ImplementationType == typeof(EndpointAuthorizationFilter));
            var newFilter = pair.New.Method.GetCustomAttribute<EndpointAuthorizationAttribute>();
            Check((oldFilter != null) == (newFilter != null), "Permission filter missing or added: " + pair.New.Route);
            if (oldFilter != null)
            {
                string identity = pair.Old.Type.Name.Replace("Controller", "");
                Check(newFilter!.ControllerName == identity, "Permission identity changed: " + pair.New.Route);
                foreach (bool allowed in new[] { false, true })
                foreach (bool before in new[] { true, false })
                {
                    var service = DispatchProxy.Create<IUserClaimQueryService, PermissionProbe>();
                    var probe = (PermissionProbe)(object)service;
                    probe.Allowed = allowed;
                    using var sp = new ServiceCollection().AddSingleton(service).BuildServiceProvider();
                    var filter = (IAsyncAuthorizationFilter)(before ? oldFilter : newFilter).CreateInstance(sp);
                    var ad = new ControllerActionDescriptor { ControllerName = before ? identity : pair.New.Type.Name.Replace("Controller", ""), ActionName = pair.Old.Method.Name };
                    var http = new DefaultHttpContext { User = Principal() };
                    http.SetEndpoint(new Microsoft.AspNetCore.Http.Endpoint(_ => Task.CompletedTask, new EndpointMetadataCollection(ad), pair.New.Route));
                    var context = new AuthorizationFilterContext(new ActionContext(http, new RouteData(), ad), new List<IFilterMetadata>());
                    filter.OnAuthorizationAsync(context).GetAwaiter().GetResult();
                    Check(probe.Key == identity + "." + pair.Old.Method.Name, "Wrong permission lookup: " + pair.New.Route);
                    Check(allowed ? context.Result == null : context.Result is ObjectResult { StatusCode: 403 }, "Wrong permission allow/deny: " + pair.New.Route);
                }
            }
        }
        string exportName = "batch110-regression.csv";
        var path = Path.Combine(AppContext.BaseDirectory, "AppData", "Excels", exportName);
        try
        {
            Aban360.Api.Tools.ControllerInspector.ExportControllerActions(exportName);
            var rows = File.ReadAllLines(path);
            foreach (var pair in cases.Where(p => typeof(BaseController).IsAssignableFrom(p.New.Type)))
            {
                string controller = pair.New.Method.GetCustomAttribute<EndpointAuthorizationAttribute>()?.ControllerName ?? pair.New.Type.Name.Replace("Controller", "");
                Check(rows.Any(row => row.EndsWith($", {controller}, {pair.New.Method.Name}, {pair.New.Route}", StringComparison.Ordinal)), "Export changed: " + pair.New.Route);
            }
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    static IEnumerable<Endpoint> Endpoints(IEnumerable<Type> types)
    {
        foreach (var type in types)
        foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
        {
            var route = method.GetCustomAttribute<RouteAttribute>();
            if (route == null) continue;
            var verbs = method.GetCustomAttributes<Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute>()
                .SelectMany(a => a.HttpMethods).Distinct().OrderBy(x => x).ToArray();
            var prefix = type.GetCustomAttribute<RouteAttribute>()?.Template ?? "";
            var template = route.Template;
            string fullRoute = template.StartsWith("~/") ? template[2..] : prefix + "/" + template;
            fullRoute = fullRoute.Replace("v1/Abonman", "v1/abonman", StringComparison.Ordinal);
            yield return new Endpoint(type, method, fullRoute, verbs);
        }
    }

    static object CreateController(Endpoint endpoint, ProbeState state)
    {
        var constructor = endpoint.Type.GetConstructors().Single();
        var controller = (ControllerBase)constructor.Invoke(constructor.GetParameters().Select(p => state.Proxy(p.ParameterType)).ToArray());
        Check(state.Calls.Count == 0, "Constructor called a dependency: " + endpoint.Type.Name);
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = Principal() } };
        return controller;
    }

    static async Task<Outcome> Execute(Endpoint endpoint, ProbeState state, CancellationToken token)
    {
        object controller = CreateController(endpoint, state);
        try
        {
            var args = endpoint.Method.GetParameters().Select(p => p.ParameterType == typeof(CancellationToken)
                ? (object)token : Samples.Create(p.ParameterType)).ToArray();
            var result = await (Task<IActionResult>)endpoint.Method.Invoke(controller, args)!;
            var ok = (ObjectResult)result;
            return new Outcome(ok.StatusCode ?? 200, Samples.Json(ok.Value), null, state.Calls.ToArray(), state.AsyncSteps.ToArray());
        }
        catch (Exception error)
        {
            while (error is TargetInvocationException { InnerException: not null }) error = error.InnerException;
            return new Outcome(0, "", error.GetType().FullName + ":" + error.Message, state.Calls.ToArray(), state.AsyncSteps.ToArray());
        }
    }

    static void AssertSuccessfulBehavior(Endpoint endpoint, Outcome outcome)
    {
        Check(outcome.Status == 200 && outcome.Error == null, "Successful action failed: " + endpoint.Route);
    }

    static void Equal(Outcome a, Outcome b, string scenario)
    {
        Check(a.Status == b.Status && a.Json == b.Json && a.Error == b.Error && a.Calls.SequenceEqual(b.Calls),
            "Behavior mismatch: " + scenario + "\nOLD " + JsonSerializer.Serialize(a) + "\nNEW " + JsonSerializer.Serialize(b));
    }
}

public record Endpoint(Type Type, MethodInfo Method, string Route, string[] Verbs);
public record Comparison(Endpoint Old, Endpoint New);
public record Outcome(int Status, string Json, string? Error, string[] Calls, int[] AsyncSteps);

public class ProbeProxy : DispatchProxy
{
    public ProbeState State = null!;
    public Type Contract = null!;
    protected override object? Invoke(MethodInfo? method, object?[]? args) => State.Invoke(Contract, method!, args ?? []);
}

public sealed class ProbeState
{
    public readonly List<string> Calls = [];
    public readonly List<int> AsyncSteps = [];
    public CancellationToken Caller;
    public int FailAt, CancelAt, GateAt, TokenValidations;
    public bool EmptyCollections, RejectToken;
    public readonly TaskCompletionSource<bool> Release = new(TaskCreationOptions.RunContinuationsAsynchronously);
    public object Proxy(Type type)
    {
        var proxy = DispatchProxy.Create(type, typeof(ProbeProxy));
        ((ProbeProxy)proxy).State = this; ((ProbeProxy)proxy).Contract = type;
        return proxy;
    }
    public void Reset() { Calls.Clear(); AsyncSteps.Clear(); TokenValidations = 0; RejectToken = false; }
    string Argument(object? arg) => arg switch
    {
        CancellationToken c => !c.CanBeCanceled ? "none" : Caller.CanBeCanceled && c == Caller ? "caller" : "request",
        IAppUser user => "user:" + user.UserId + ":" + user.Username + ":" + user.FullName,
        Job job => "job:" + job.Type.FullName + ":" + job.Method.Name + ":" + JsonSerializer.Serialize(job.Args.Select(Argument)),
        IState state => state.Name,
        _ => Samples.Json(arg)
    };
    public object? Invoke(Type contract, MethodInfo method, object?[] args)
    {
        Calls.Add(Aban360.Tests.ServiceBaselines.ApplicationServiceBaseline.CallName(contract, method) + "(" + string.Join("|", args.Select(Argument)) + ")");
        int step = Calls.Count;
        if (method.ReturnType == typeof(Task)) { AsyncSteps.Add(step); return CompleteVoid(step); }
        if (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            AsyncSteps.Add(step);
            var value = Samples.Create(method.ReturnType.GetGenericArguments()[0], empty: EmptyCollections);
            return typeof(ProbeState).GetMethod(nameof(Complete), BindingFlags.Instance | BindingFlags.NonPublic)!
                .MakeGenericMethod(method.ReturnType.GetGenericArguments()[0]).Invoke(this, new[] { (object)step, value });
        }
        ThrowIfNeeded(step);
        return method.ReturnType == typeof(string) ? "regression-job" : Samples.Create(method.ReturnType);
    }
    async Task CompleteVoid(int step) { if (GateAt == step) await Release.Task; ThrowIfNeeded(step); }
    async Task<T> Complete<T>(int step, object? value) { if (GateAt == step) await Release.Task; ThrowIfNeeded(step); return (T)value!; }
    void ThrowIfNeeded(int step)
    {
        if (CancelAt == step) throw new OperationCanceledException("regression-cancel", Caller);
        if (FailAt == step) throw new InvalidOperationException("regression-failure-" + step);
    }
}

public static class Samples
{
    public static object? Create(Type type, int depth = 0, bool empty = false)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;
        if (type == typeof(string)) return "sample";
        if (type == typeof(int)) return 17;
        if (type == typeof(short)) return (short)17;
        if (type == typeof(long)) return 17L;
        if (type == typeof(bool)) return true;
        if (type == typeof(Guid)) return Regression.UserId;
        if (type == typeof(DateTime)) return new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        if (type.IsEnum) return Enum.GetValues(type).GetValue(0);
        if (type == typeof(void)) return null;
        if (type.IsArray)
        {
            var array = Array.CreateInstance(type.GetElementType()!, empty || depth > 3 ? 0 : 1);
            if (array.Length > 0) array.SetValue(Create(type.GetElementType()!, depth + 1, empty), 0);
            return array;
        }
        if (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type))
        {
            var itemType = type.GetGenericArguments()[0];
            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType))!;
            if (!empty && depth < 3) list.Add(Create(itemType, depth + 1, empty));
            return list;
        }
        if (type.IsValueType) return Activator.CreateInstance(type);
        if (depth > 3 || type.IsInterface || type.IsAbstract) return null;
        var ctor = type.GetConstructors().OrderBy(c => c.GetParameters().Length).FirstOrDefault();
        if (ctor == null) return null;
        var instance = ctor.Invoke(ctor.GetParameters().Select(p => Create(p.ParameterType, depth + 1, empty)).ToArray());
        foreach (var property in type.GetProperties().Where(p => p.CanWrite && p.SetMethod?.IsPublic == true && p.GetIndexParameters().Length == 0))
            property.SetValue(instance, Create(property.PropertyType, depth + 1, empty));
        return instance;
    }
    public static string Json(object? value) => Normalize(JsonSerializer.Serialize(value, value?.GetType() ?? typeof(object)));
    public static string Normalize(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;
        var node = JsonNode.Parse(text);
        Clean(node);
        return node?.ToJsonString() ?? "null";
    }
    static void Clean(JsonNode? node)
    {
        if (node is JsonObject obj)
        {
            // Only nondeterministic envelope timestamps / MVC correlation ids are excluded.
            foreach (var key in obj.Select(p => p.Key).Where(k => k.Equals("traceId", StringComparison.OrdinalIgnoreCase)).ToArray()) obj.Remove(key);
            foreach (var p in obj.ToArray())
            {
                if (p.Key.Equals("Meta", StringComparison.OrdinalIgnoreCase) && p.Value is JsonObject meta)
                    foreach (var key in meta.Select(x => x.Key).Where(k => k.Equals("ServerDateTime", StringComparison.OrdinalIgnoreCase)).ToArray()) meta.Remove(key);
                Clean(p.Value);
            }
        }
        else if (node is JsonArray array) foreach (var child in array) Clean(child);
    }
}

public class PermissionProbe : DispatchProxy
{
    public bool Allowed;
    public string? Key;
    protected override object? Invoke(MethodInfo? method, object?[]? args)
    {
        if (method!.Name != "HasValue") throw new InvalidOperationException("Unexpected permission service call");
        Key = (string)args![2]!;
        return Task.FromResult(Allowed);
    }
}

