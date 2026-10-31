using System.Diagnostics;
using System.Security.Claims;
using Aban360.Api.Middleware;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;

await Check("success and privacy", async () =>
{
    var context = Context();
    context.Request.QueryString = new QueryString("?password=SECRET");
    context.Request.Headers.Authorization = "Bearer SECRET";
    context.Request.Headers.Cookie = "SECRET";
    context.Request.Body = new MemoryStream("SECRET"u8.ToArray());
    context.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "42") }, "test"));
    using var activity = new Activity("request").Start();
    var log = await Run(context, c => { c.Response.StatusCode = 201; return Task.CompletedTask; });
    Assert(log.Events.Count == 1 && log.Events[0].Level == LogLevel.Information);
    Assert(Equals(log.Events[0].Fields["RouteTemplate"], "/users/{id}"));
    Assert(Equals(log.Events[0].Fields["StatusCode"], 201));
    Assert(Equals(log.Events[0].Fields["UserId"], "42"));
    Assert(Equals(log.Events[0].Fields["TraceId"], activity.TraceId.ToString()));
    Assert(!log.Events[0].Text.Contains("SECRET"));
    Assert(context.Request.Body.Position == 0);
});
await Check("handled server error", async () =>
{
    var log = await Run(Context(), c => { c.Response.StatusCode = 500; return Task.CompletedTask; });
    Assert(log.Events.Single().Level == LogLevel.Error);
});
await Check("client error remains informational", async () =>
{
    var log = await Run(Context(), c => { c.Response.StatusCode = 400; return Task.CompletedTask; });
    Assert(log.Events.Single().Level == LogLevel.Information);
});
await Check("slow request", async () =>
{
    var log = await Run(Context(), _ => Task.Delay(30), "1");
    Assert(log.Events.Single().Level == LogLevel.Warning);
});
await Check("exception propagates and logs once", async () =>
{
    var log = new CaptureLogger();
    var error = new InvalidOperationException("SECRET");
    try
    {
        await new HttpRequestLoggingMiddleware(_ => throw error, log, Config()).InvokeAsync(Context());
        throw new Exception("Exception was swallowed");
    }
    catch (InvalidOperationException caught) when (ReferenceEquals(error, caught)) { }
    Assert(log.Events.Count == 1 && log.Events[0].Level == LogLevel.Error);
    Assert(!log.Events[0].Text.Contains("SECRET"));
});
await Check("health exclusions", async () =>
{
    foreach (var path in new[] { "/healthz", "/health/live", "/health/ready" })
    {
        var context = Context();
        context.Request.Path = path;
        var called = false;
        var log = await Run(context, _ => { called = true; return Task.CompletedTask; });
        Assert(called && log.Events.Count == 0);
    }
});
await Check("unmatched route and anonymous identity", async () =>
{
    var context = Context();
    context.SetEndpoint(null);
    context.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "untrusted") }));
    var log = await Run(context, _ => Task.CompletedTask);
    Assert(Equals(log.Events.Single().Fields["RouteTemplate"], "<unmatched>"));
    Assert(log.Events[0].Fields["UserId"] is null);
    Assert(Equals(log.Events[0].Fields["TraceId"], context.TraceIdentifier));
    Assert(!log.Events[0].Text.Contains("SECRET"));
});
await Check("invalid threshold falls back safely", async () =>
{
    var log = await Run(Context(), _ => Task.CompletedTask, "invalid");
    Assert(log.Events.Single().Level == LogLevel.Information);
});
await Check("client cancellation is not a server failure", async () =>
{
    var context = Context();
    using var source = new CancellationTokenSource();
    source.Cancel();
    context.RequestAborted = source.Token;
    var log = new CaptureLogger();
    try
    {
        await new HttpRequestLoggingMiddleware(_ => throw new OperationCanceledException(source.Token), log, Config()).InvokeAsync(context);
        throw new Exception("Cancellation swallowed");
    }
    catch (OperationCanceledException) { }
    Assert(log.Events.Single().Level == LogLevel.Warning);
    Assert(Equals(log.Events[0].Fields["StatusCode"], 499));
});
Console.WriteLine("All 9 HTTP logging smoke tests passed.");

static DefaultHttpContext Context()
{
    var context = new DefaultHttpContext();
    context.Request.Method = "GET";
    context.Request.Path = "/users/SECRET";
    context.SetEndpoint(new RouteEndpoint(_ => Task.CompletedTask,
        RoutePatternFactory.Parse("/users/{id}"), 0, EndpointMetadataCollection.Empty, "test"));
    return context;
}
static IConfiguration Config(string threshold = "2000") => new ConfigurationBuilder()
    .AddInMemoryCollection(new Dictionary<string, string?> { ["HttpRequestLogging:SlowRequestMilliseconds"] = threshold }).Build();
static async Task<CaptureLogger> Run(HttpContext context, RequestDelegate next, string threshold = "2000")
{
    var log = new CaptureLogger();
    await new HttpRequestLoggingMiddleware(next, log, Config(threshold)).InvokeAsync(context);
    return log;
}
static void Assert(bool condition) { if (!condition) throw new Exception("Assertion failed"); }
static async Task Check(string name, Func<Task> test) { await test(); Console.WriteLine($"PASS: {name}"); }

sealed class CaptureLogger : ILogger<HttpRequestLoggingMiddleware>
{
    public List<(LogLevel Level, Dictionary<string, object?> Fields, string Text)> Events { get; } = new();
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
    public bool IsEnabled(LogLevel level) => true;
    public void Log<TState>(LogLevel level, EventId id, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        => Events.Add((level, ((IEnumerable<KeyValuePair<string, object?>>)state!).ToDictionary(p => p.Key, p => p.Value), formatter(state, exception)));
}
