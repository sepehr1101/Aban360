# SystemPool HTTP reports

GET endpoints under `/v1/logging/http-requests`:

| Endpoint | Result / ordering |
| --- | --- |
| `summary` | Total count, 5xx count, average, maximum and sum of processing milliseconds |
| `routes` | Groups by HTTP method and route template, highest maximum duration first |
| `users` | Groups by internal user ID, highest average duration first; null denotes anonymous |
| `slowest` | Individual requests, longest first; includes timestamp, status and TraceId |

All endpoints require authentication and the role configured in
`ApiSettings:AdminRoleName`, and use the existing BaseController response envelope.
Application and persistence services are registered by the existing assembly scans.

Required query parameters: `FromDate`, `ToDate` in `yyyy/MM/dd` Persian calendar.
Optional: `FromTime`, `ToTime` in `HH:mm` (both default to `00:00`), `UserId`,
`RouteTemplate`, `Page` (default 1), `PageSize` (default 50, maximum 200).
Persian and Arabic digits are accepted. Range is **start-inclusive, end-exclusive**;
to report an entire day, use midnight of the following day as the upper bound.
Maximum interval is 31 days to limit XML scanning of the existing log table.
Summary ignores pagination; other endpoints include TotalGroups on each row.
An empty page is an empty array (no separate total metadata).

Example: `/v1/logging/http-requests/users?FromDate=1405%2F08%2F01&ToDate=1405%2F08%2F02&PageSize=50`

Durations describe server processing inside the request logging middleware,
not browser/network waiting time. Concurrent requests contribute separately.
Only retained HTTP completion events from the new middleware are included;
health probes, static files, earlier historical logs and dropped/filtered events
are not counted. SQL sink batching can delay visibility briefly.

Time boundaries follow the existing logging reports' timestamp convention:
Gregorian DateTime values without timezone conversion. The production SQL sink
and server timezone must be verified together before interpreting day boundaries.

No schema changes or migrations. Queries read the existing Logs.MessageTemplate
and XML Properties columns, with parameterized values and cancellation support.
Aggregation occurs in SQL without the older log viewer's 10,000-row limit.
This assumes the existing default MSSqlServer sink XML format. Query execution
against production SQL and representative load still needs deployment validation.
If indexes or storage changes become necessary, review them separately first.

Run validation: `dotnet run --project tests/Aban360.HttpReports.SmokeTests/Aban360.HttpReports.SmokeTests.csproj`.
