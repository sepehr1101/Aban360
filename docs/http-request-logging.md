# HTTP request logging

The API records one structured completion event per request reaching routing.
Static files served earlier in the pipeline and health probes (`/healthz`,
`/health/live`, `/health/ready`) are excluded. Swagger handled earlier is also excluded.

Fields: RequestMethod, RouteTemplate, StatusCode, ElapsedMilliseconds, TraceId,
and authenticated UserId (NameIdentifier claim). Unmatched requests use
`<unmatched>` rather than the raw URL. Bodies, headers, query strings, route
values and exception messages are not included in this completion event.
Other application/framework logging has its own configuration and should be
reviewed separately for sensitive data.

The existing Serilog sinks receive these events. SQL properties use the existing
Logs table structure; no migration or additional columns are required.
No database has been modified by this change.

Completed 5xx requests and escaping exceptions use Error. Slow requests use
Warning; other responses, including 4xx, use Information. The default slow
threshold is 2000 ms, configurable using
`HttpRequestLogging:SlowRequestMilliseconds` (positive finite number).
Invalid thresholds fall back to 2000 ms. Escaping OperationCanceledException
with a cancelled RequestAborted token is logged as a synthetic 499 at Warning,
not a server error; this does not change the HTTP response sent to the client.

The logging middleware wraps exception handling after routing so handled
exceptions are measured with their final response status. Authentication runs
inside this wrapper, allowing the completion event to include the user ID.

Run the dependency-free smoke tests:

```powershell
dotnet run --project tests/Aban360.HttpLogging.SmokeTests/Aban360.HttpLogging.SmokeTests.csproj
```

These tests capture structured log events without connecting to SQL Server.
They cover privacy and route redaction, identity and trace correlation, status
levels, slow requests, exception propagation and health probe exclusions.
