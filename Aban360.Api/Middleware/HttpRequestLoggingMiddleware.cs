using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Routing;

namespace Aban360.Api.Middleware;

public sealed class HttpRequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HttpRequestLoggingMiddleware> _logger;
    private readonly double _slowMilliseconds;

    public HttpRequestLoggingMiddleware(RequestDelegate next,
        ILogger<HttpRequestLoggingMiddleware> logger, IConfiguration configuration)
    {
        _next = next;
        _logger = logger;
        _slowMilliseconds = double.TryParse(configuration["HttpRequestLogging:SlowRequestMilliseconds"],
            NumberStyles.Float, CultureInfo.InvariantCulture, out var threshold) ? threshold : 2000;
        if (!double.IsFinite(_slowMilliseconds) || _slowMilliseconds <= 0)
            _slowMilliseconds = 2000;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/healthz") ||
            context.Request.Path.StartsWithSegments("/health/live") ||
            context.Request.Path.StartsWithSegments("/health/ready"))
        {
            await _next(context);
            return;
        }

        // Never fall back to a raw path: unmatched URLs can contain sensitive data.
        var route = (context.GetEndpoint() as RouteEndpoint)?.RoutePattern.RawText ?? "<unmatched>";
        var traceId = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;
        var started = Stopwatch.GetTimestamp();
        var failed = false;
        var aborted = false;
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            aborted = true;
            throw;
        }
        catch
        {
            failed = true;
            throw;
        }
        finally
        {
            var elapsed = Stopwatch.GetElapsedTime(started).TotalMilliseconds;
            var status = aborted ? 499 : failed ? StatusCodes.Status500InternalServerError : context.Response.StatusCode;
            var level = status >= 500 ? LogLevel.Error :
                aborted || elapsed >= _slowMilliseconds ? LogLevel.Warning : LogLevel.Information;
            var userId = context.User.Identity?.IsAuthenticated == true
                ? context.User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
            _logger.Log(level,
                "HTTP {RequestMethod} {RouteTemplate} completed {StatusCode} in {ElapsedMilliseconds} ms; TraceId {TraceId}; UserId {UserId}",
                context.Request.Method, route, status, elapsed, traceId, userId);
        }
    }
}
