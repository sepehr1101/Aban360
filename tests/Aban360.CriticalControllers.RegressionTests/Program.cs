using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Aban360.Api.Controllers.V1;
using Aban360.Api.Extensions;
using Aban360.Common.ApplicationUser;
using Aban360.UserPool.Application.Features.Auth.Services.Contracts;
using Hangfire.Common;
using Hangfire.States;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Aban360.CriticalControllerTests;

public static class Regression
{
    public static int Checks;
    public static readonly Guid UserId = Guid.Parse("2a7416be-9db9-49bc-ac12-e698e1c7d970");
    private const string Key = "controller-regression-test-key-for-local-testing-only-2026";
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
        var names = new HashSet<string> { "RoleController", "AppController", "EndpointController", "ModuleController", "ZoneController" };
        var current = typeof(BaseController).Assembly.GetTypes().Where(t => names.Contains(t.Name)).ToArray();
        var original = typeof(Regression).Assembly.GetTypes().Where(t => t.Namespace?.Contains(".BeforeConsolidation.") == true
            && typeof(ControllerBase).IsAssignableFrom(t) && !t.IsAbstract).ToArray();
        Check(current.Length == 5 && original.Length == 28, "Missing current controllers or pinned original snapshots.");
        var currentMethods = Endpoints(current).ToArray();
        var cases = Endpoints(original).Select(old =>
        {
            var matches = currentMethods.Where(n => n.Route == old.Route && n.Verbs.SequenceEqual(old.Verbs)).ToArray();
            Check(matches.Length == 1, "Missing/ambiguous merged action: " + old.Route);
            return new Comparison(old, matches.Single());
        }).ToArray();
        Check(cases.Length == 30 && currentMethods.Length == 30, "Expected all 30 critical actions.");

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

        await using var oldHost = await HttpHarness.Start(original);
        await using var newHost = await HttpHarness.Start(current);
        string jwt = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(claims: Principal().Claims,
            expires: DateTime.UtcNow.AddHours(1), signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)), SecurityAlgorithms.HmacSha256)));
        int httpCases = 0;
        foreach (var pair in cases)
        {
            foreach (string verb in pair.Old.Verbs)
            {
                var a = await oldHost.Send(pair.Old, verb, jwt);
                var b = await newHost.Send(pair.New, verb, jwt);
                Equal(a, b, pair.Old.Route + " authenticated HTTP " + verb);
                Check(a.Status == 200 && a.TokenValidations == 1, "Valid JWT did not reach action: " + pair.Old.Route);
                httpCases++;
            }
            string preferred = pair.Old.Verbs.First();
            foreach (var auth in new[] { "missing", "malformed", "rejected" })
            {
                var a = await oldHost.Send(pair.Old, preferred, auth == "missing" ? null : auth == "malformed" ? "invalid-token" : jwt, reject: auth == "rejected");
                var b = await newHost.Send(pair.New, preferred, auth == "missing" ? null : auth == "malformed" ? "invalid-token" : jwt, reject: auth == "rejected");
                Equal(a, b, pair.Old.Route + " HTTP auth=" + auth);
                bool raw = pair.Old.Route == "v1/role/params-create";
                Check(a.Status == (raw ? 200 : 401), "Authorization status changed: " + pair.Old.Route);
                if (!raw) Check(a.Calls.Length == 0, "Unauthorized request invoked a handler: " + pair.Old.Route);
                if (auth == "rejected") Check(a.TokenValidations == 1, "JWT validator was bypassed: " + pair.Old.Route);
                httpCases++;
            }
            if (pair.Old.Method.GetParameters().Any(p => p.IsDefined(typeof(FromBodyAttribute))))
            {
                foreach (string body in new[] { "", "{", "null", "{}" })
                {
                    var a = await oldHost.Send(pair.Old, preferred, jwt, bodyOverride: body);
                    var b = await newHost.Send(pair.New, preferred, jwt, bodyOverride: body);
                    Equal(a, b, pair.Old.Route + " invalid body=" + body);
                    if (body != "{}")
                        Check(a.Status == 400 && a.Calls.Length == 0, "Invalid model reached a handler: " + pair.Old.Route);
                    else
                        Check(a.Status is 200 or 400 && (a.Status != 400 || a.Calls.Length == 0),
                            "Empty-object model behavior changed: " + pair.Old.Route);
                    httpCases++;
                }
            }
            if (pair.Old.Method.GetParameters().Any(p => p.ParameterType == typeof(int) || p.ParameterType == typeof(short)))
            {
                var a = await oldHost.Send(pair.Old, preferred, jwt, id: "not-a-number");
                var b = await newHost.Send(pair.New, preferred, jwt, id: "not-a-number");
                Equal(a, b, pair.Old.Route + " invalid id");
                Check(a.Status == 400 && a.Calls.Length == 0, "Invalid id reached a handler: " + pair.Old.Route);
                httpCases++;
            }
            var unsupported = pair.Old.Verbs.Contains("PUT") ? "OPTIONS" : "PUT";
            var oldUnsupported = await oldHost.Send(pair.Old, unsupported, jwt);
            var newUnsupported = await newHost.Send(pair.New, unsupported, jwt);
            Equal(oldUnsupported, newUnsupported, pair.Old.Route + " unsupported verb");
            Check(oldUnsupported.Status == 405, "Unsupported verb was accepted: " + pair.Old.Route);
            httpCases++;
        }
        Console.WriteLine($"PASS: {Checks} checks; 30 actions; {behavioralCases} behavioral scenarios; {httpCases} HTTP scenarios compared before/after.");
        Console.WriteLine("Actual JWT middleware and MVC binding exercised; handler/UoW/Hangfire dependencies are isolated fakes. No database or real jobs used.");
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
            yield return new Endpoint(type, method, type.GetCustomAttribute<RouteAttribute>()!.Template + "/" + route.Template, verbs);
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
        bool mutation = endpoint.Route.EndsWith("/create") || endpoint.Route.EndsWith("/update") || endpoint.Route.EndsWith("/delete");
        Check(outcome.Calls.Count(c => c.Contains(".SaveChangesAsync(")) == (mutation ? 1 : 0), "Unexpected save count: " + endpoint.Route);
        if (mutation) Check(outcome.Calls[1].Contains(".SaveChangesAsync("), "Save must follow the handler: " + endpoint.Route);
        if (endpoint.Route == "v1/role/update")
        {
            Check(outcome.Calls.Length == 3 && outcome.Calls[2].Contains("IBackgroundJobClient.Create("), "Background job order changed.");
            Check(outcome.Calls[2].Contains("IUpdateUsersInRoleHandler") && outcome.Calls[2].Contains("\"17\"")
                && outcome.Calls[2].Contains("none") && outcome.Calls[2].Contains("Enqueued"), "Wrong role update background job: " + outcome.Calls[2]);
        }
        if (endpoint.Route is "v1/zone/all" or "v1/zone/my-default")
            Check(outcome.Calls[0].Contains(UserId.ToString()), "Zone did not receive current user.");
        var json = JsonNode.Parse(outcome.Json)!;
        Check((json["Data"] != null) == (endpoint.Route != "v1/role/params-create"), "Wrong envelope/raw response shape: " + endpoint.Route);
    }

    static void Equal(Outcome a, Outcome b, string scenario)
    {
        Check(a.Status == b.Status && a.Json == b.Json && a.Error == b.Error && a.Calls.SequenceEqual(b.Calls),
            "Behavior mismatch: " + scenario + "\nOLD " + JsonSerializer.Serialize(a) + "\nNEW " + JsonSerializer.Serialize(b));
    }
    static void Equal(HttpOutcome a, HttpOutcome b, string scenario)
    {
        Check(a.Status == b.Status && a.Json == b.Json && a.ContentType == b.ContentType
            && a.Calls.SequenceEqual(b.Calls) && a.TokenValidations == b.TokenValidations,
            "HTTP mismatch: " + scenario + "\nOLD " + JsonSerializer.Serialize(a) + "\nNEW " + JsonSerializer.Serialize(b));
    }

    internal sealed class HttpHarness : IAsyncDisposable
    {
        readonly WebApplication app;
        readonly HttpClient client;
        readonly ProbeState state;
        HttpHarness(WebApplication application, ProbeState probe) { app = application; client = app.GetTestClient(); state = probe; }
        public static async Task<HttpHarness> Start(Type[] types)
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = "Testing", ApplicationName = typeof(Regression).Assembly.FullName });
            builder.Logging.ClearProviders();
            builder.WebHost.UseTestServer();
            builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?> { ["BearerTokens:Key"] = Key, ["BearerTokens:Issuer"] = "regression", ["BearerTokens:Audience"] = "regression" });
            var state = new ProbeState();
            foreach (var contract in types.SelectMany(t => t.GetConstructors().Single().GetParameters()).Select(p => p.ParameterType).Distinct())
                builder.Services.AddScoped(contract, _ => state.Proxy(contract));
            builder.Services.AddAuth(builder.Configuration);
            builder.Services.AddSingleton<ITokenValidatorService>(new TokenValidator(state));
            builder.Services.AddControllers().AddApplicationPart(types[0].Assembly)
                .ConfigureApplicationPartManager(manager => manager.FeatureProviders.Add(new SelectedControllers(types)));
            var app = builder.Build();
            app.UseRouting(); app.UseAuthentication(); app.UseAuthorization(); app.MapControllers();
            await app.StartAsync();
            return new HttpHarness(app, state);
        }
        public async Task<HttpOutcome> Send(Endpoint endpoint, string verb, string? token, bool reject = false, string? bodyOverride = null, string id = "17")
        {
            state.Reset(); state.RejectToken = reject;
            string route = endpoint.Route.Replace("{id}", id);
            bool queryId = endpoint.Method.GetParameters().Any(p => p.Name == "id") && !endpoint.Route.Contains("{id}");
            if (queryId) route += "?id=" + id;
            using var request = new HttpRequestMessage(new HttpMethod(verb), "/" + route);
            if (token != null) request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var bodyParameter = endpoint.Method.GetParameters().SingleOrDefault(p => p.IsDefined(typeof(FromBodyAttribute)));
            if (bodyParameter != null)
                request.Content = new StringContent(bodyOverride ?? JsonSerializer.Serialize(Samples.Create(bodyParameter.ParameterType)), Encoding.UTF8, "application/json");
            using var response = await client.SendAsync(request);
            return new HttpOutcome((int)response.StatusCode, Samples.Normalize(await response.Content.ReadAsStringAsync()),
                response.Content.Headers.ContentType?.ToString(), state.Calls.ToArray(), state.TokenValidations);
        }
        public async ValueTask DisposeAsync() { client.Dispose(); await app.DisposeAsync(); }
    }
    sealed class SelectedControllers(Type[] types) : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            for (int i = feature.Controllers.Count - 1; i >= 0; i--)
                if (!types.Contains(feature.Controllers[i].AsType())) feature.Controllers.RemoveAt(i);
        }
    }
    sealed class TokenValidator(ProbeState state) : ITokenValidatorService
    {
        public Task ValidateAsync(TokenValidatedContext context)
        {
            state.TokenValidations++;
            if (state.RejectToken) context.Fail("Regression validator rejection");
            return Task.CompletedTask;
        }
    }
}

public record Endpoint(Type Type, MethodInfo Method, string Route, string[] Verbs);
public record Comparison(Endpoint Old, Endpoint New);
public record Outcome(int Status, string Json, string? Error, string[] Calls, int[] AsyncSteps);
public record HttpOutcome(int Status, string Json, string? ContentType, string[] Calls, int TokenValidations);

public class ProbeProxy : DispatchProxy
{
    public ProbeState State = null!;
    public Type Contract = null!;
    private bool disposed;
    protected override object? Invoke(MethodInfo? method, object?[]? args)
    {
        // Consolidated controllers inject their shared UoW even for query actions.
        // Verify scoped cleanup separately from handler/save/job calls.
        if (method!.Name == nameof(IDisposable.Dispose) && method.ReturnType == typeof(void)
            && method.GetParameters().Length == 0 && typeof(IDisposable).IsAssignableFrom(Contract))
        {
            Regression.Check(!disposed, "Scoped dependency disposed twice: " + Contract.FullName);
            disposed = true;
            return null;
        }
        return State.Invoke(Contract, method, args ?? []);
    }
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
    // Hangfire may resolve an expression's target to the DispatchProxy runtime type.
    // Compare the implemented handler contract rather than a generated proxy class name.
    static string JobTypeName(Job job)
    {
        if (!typeof(DispatchProxy).IsAssignableFrom(job.Type)) return job.Type.FullName!;
        var parameters = job.Method.GetParameters().Select(p => p.ParameterType).ToArray();
        return job.Type.GetInterfaces().Single(contract => contract.GetMethods().Any(method =>
            method.Name == job.Method.Name && method.ReturnType == job.Method.ReturnType
            && method.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameters))).FullName!;
    }
    string Argument(object? arg) => arg switch
    {
        CancellationToken c => !c.CanBeCanceled ? "none" : Caller.CanBeCanceled && c == Caller ? "caller" : "request",
        IAppUser user => "user:" + user.UserId + ":" + user.Username + ":" + user.FullName,
        Job job => "job:" + JobTypeName(job) + ":" + job.Method.Name + ":" + JsonSerializer.Serialize(job.Args.Select(Argument)),
        IState state => state.Name,
        _ => Samples.Json(arg)
    };
    public object? Invoke(Type contract, MethodInfo method, object?[] args)
    {
        Calls.Add(contract.FullName + "." + method.Name + "(" + string.Join("|", args.Select(Argument)) + ")");
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
