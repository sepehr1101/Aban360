using Aban360.Common.Exceptions;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;
using Aban360.SystemPool.Persistence.Features.Logging.Queries.Contracts;
using Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Implementations;

var query = new CaptureQuery();
var handler = new HttpRequestReportHandler(query);
HttpRequestReportInput Valid() => new() { FromDate = "1405/01/01", ToDate = "1405/01/02" };
void Assert(bool value) { if (!value) throw new Exception("Assertion failed"); }
await handler.Handle(Valid(), HttpRequestReportKind.Summary, default);
Assert(query.From == new DateTime(2026, 3, 21) && query.To == new DateTime(2026, 3, 22));
var persian = Valid(); persian.FromDate = "۱۴۰۵/۰۱/۰۱";
await handler.Handle(persian, HttpRequestReportKind.Users, default);
Assert(query.From == new DateTime(2026, 3, 21));
foreach (var bad in new[] {
    new HttpRequestReportInput { FromDate = "1405/+1/01", ToDate = "1405/01/02" },
    new HttpRequestReportInput { FromDate = "1405/½1/01", ToDate = "1405/01/02" },
    new HttpRequestReportInput { FromDate = "1405/12/30", ToDate = "1406/01/02" },
    new HttpRequestReportInput { FromDate = "1405/01/02", ToDate = "1405/01/01" },
    new HttpRequestReportInput { FromDate = "1405/01/01", ToDate = "1405/03/01" },
    new HttpRequestReportInput { FromDate = "1405/01/01", ToDate = "1405/01/02", FromTime = "25:00" },
    new HttpRequestReportInput { FromDate = "1405/01/01", ToDate = "1405/01/02", PageSize = 201 } })
{
    var count = query.Calls;
    try { await handler.Handle(bad, HttpRequestReportKind.Summary, default); throw new Exception("Invalid input accepted"); }
    catch (InvalidDateException) { Assert(query.Calls == count); }
}
var filtered = Valid(); filtered.UserId = "42"; filtered.RouteTemplate = "/users/{id}"; filtered.Page = 2;
using var source = new CancellationTokenSource();
await handler.Handle(filtered, HttpRequestReportKind.Requests, source.Token);
Assert(query.Input == filtered && query.Kind == HttpRequestReportKind.Requests && query.Token == source.Token);
await handler.Handle(filtered, HttpRequestReportKind.Summary, default);
Assert(query.Input!.Page == 1 && query.Input.UserId == "42" && filtered.Page == 2);
Console.WriteLine("All HTTP report handler checks passed (dates, validation, filters, paging, cancellation).");

sealed class CaptureQuery : IHttpRequestReportQueryService
{
    public DateTime From, To;
    public int Calls;
    public HttpRequestReportInput? Input;
    public HttpRequestReportKind Kind;
    public CancellationToken Token;
    public Task<IEnumerable<HttpRequestReportOutput>> Get(DateTime from, DateTime to, HttpRequestReportInput input, HttpRequestReportKind kind, CancellationToken cancellationToken)
    {
        From = from; To = to; Input = input; Kind = kind; Token = cancellationToken; Calls++;
        return Task.FromResult<IEnumerable<HttpRequestReportOutput>>(Array.Empty<HttpRequestReportOutput>());
    }
}
