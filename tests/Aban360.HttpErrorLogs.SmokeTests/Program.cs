using Aban360.Common.Exceptions;
using Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Implementations;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;
using Aban360.SystemPool.Persistence.Features.Logging.Queries.Contracts;

var checks = 0;
void Check(bool condition, string name)
{
    if (!condition) throw new Exception(name);
    checks++;
}
HttpErrorLogInput Valid() => new()
{
    StatusCodeClass = "4XX", FromDate = "1405/01/01", ToDate = "1405/01/02",
    FromTime = "08:30", ToTime = "23:59"
};
var query = new CaptureQuery();
var handler = new HttpErrorLogHandler(query);
using var cancellation = new CancellationTokenSource();
var result = await handler.Handle(Valid(), cancellation.Token);
Check(ReferenceEquals(result, query.Rows), "Error details returned unchanged");
Check(query.From == new DateTime(2026, 3, 21, 8, 30, 0), "Jalali start conversion");
Check(query.To == new DateTime(2026, 3, 23), "Includes end minute and midnight rollover");
Check(query.Lower == 400 && query.Upper == 500, "4XX includes 400 through 499");
Check(query.Token == cancellation.Token && query.Page == 1 && query.Size == 50, "Cancellation and paging defaults");
var server = Valid();
server.StatusCodeClass = " 5xx ";
server.FromDate = "۱۴۰۵/۰۱/۰۱";
server.FromTime = "٠٨:٣٠";
server.Page = int.MaxValue;
server.PageSize = 200;
await handler.Handle(server, default);
Check(query.Lower == 500 && query.Upper == 600, "5XX includes 500 through 599");
Check(query.From == new DateTime(2026, 3, 21, 8, 30, 0) && query.Page == int.MaxValue && query.Size == 200,
    "Localized digits and maximum paging");
var minute = Valid();
minute.ToDate = minute.FromDate;
minute.ToTime = minute.FromTime;
await handler.Handle(minute, default);
Check(query.To - query.From == TimeSpan.FromMinutes(1), "Same minute is searchable");

foreach (var mutate in new Action<HttpErrorLogInput>[]
{
    x => x.StatusCodeClass = "", x => x.StatusCodeClass = null!,
    x => x.StatusCodeClass = "3XX", x => x.StatusCodeClass = "400",
    x => x.StatusCodeClass = "4XX' OR 1=1--",
    x => x.FromDate = "1405/12/30", x => x.FromDate = null!,
    x => x.FromDate = "2026-03-21", x => x.FromTime = "24:00",
    x => x.ToTime = "12:30:01", x => x.ToTime = "",
    x => x.ToDate = "1404/12/29", x => x.ToDate = "1405/03/01",
    x => x.Page = 0, x => x.PageSize = 0, x => x.PageSize = 201
})
{
    var invalid = Valid();
    mutate(invalid);
    var calls = query.Calls;
    try { await handler.Handle(invalid, default); throw new Exception("Invalid search accepted"); }
    catch (BaseException) { Check(calls == query.Calls, "Invalid search rejected before database access"); }
}
try { await handler.Handle(null!, default); throw new Exception("Null search accepted"); }
catch (CustomValidationException) { checks++; }
Console.WriteLine($"PASS: {checks} HTTP error log checks. No database connection was opened.");

sealed class CaptureQuery : IHttpErrorLogQueryService
{
    public DateTime From, To;
    public int Lower, Upper, Page, Size, Calls;
    public CancellationToken Token;
    public IEnumerable<HttpErrorLogOutput> Rows = new[]
    {
        new HttpErrorLogOutput { Id = 1, StatusCode = 400, TraceId = "trace-400" },
        new HttpErrorLogOutput { Id = 2, StatusCode = 401, TraceId = "trace-401" }
    };
    public Task<IEnumerable<HttpErrorLogOutput>> Get(DateTime from, DateTime toExclusive,
        int statusCodeFrom, int statusCodeToExclusive, int page, int pageSize, CancellationToken cancellationToken)
    {
        Calls++;
        (From, To, Lower, Upper, Page, Size, Token) =
            (from, toExclusive, statusCodeFrom, statusCodeToExclusive, page, pageSize, cancellationToken);
        return Task.FromResult(Rows);
    }
}
