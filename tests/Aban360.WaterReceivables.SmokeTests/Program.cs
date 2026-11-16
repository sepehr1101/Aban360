using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Validations;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;

void Check(bool condition, string name)
{
    if (!condition) throw new Exception(name);
    Console.WriteLine("PASS: " + name);
}

var validator = new WaterReceivablesValidator();
var queries = new FakeQueries();
var handlerType = typeof(IWaterReceivablesHandler).Assembly.GetType(
    "Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations.WaterReceivablesHandler")!;
var handler = (IWaterReceivablesHandler)Activator.CreateInstance(handlerType, queries, validator)!;
var input = new WaterReceivablesInputDto
{
    GroupBy = WaterReceivablesGroupingEnum.Usage,
    FromDateJalali = "1405/01/01",
    ToDateJalali = "1405/03/31"
};
var result = await handler.Handle(input, CancellationToken.None);
Check(queries.Calls.Count == 4, "Four independent sales/collection queries");
Check(queries.Calls.Take(2).All(c => c.From == "1330/01/01" && c.To == "1404/12/29"),
    "Opening period ends on previous Persian calendar day");
Check(queries.Calls.Skip(2).All(c => c.From == input.FromDateJalali && c.To == input.ToDateJalali),
    "Selected period uses exact inclusive bounds");
var home = result.ReportData.Single(r => r.GroupTitle == "home");
Check(home.OpeningBalance == 70 && home.PeriodSales == 50 && home.PeriodCollections == 20 && home.ClosingBalance == 100,
    "Opening and closing balances match accounting formula");
Check(result.ReportData.Single(r => r.GroupTitle == "collections-only").ClosingBalance == -5,
    "Collection-only groups and negative balances survive merge");
Check(result.ReportData.Single(r => r.GroupTitle == "history-only").ClosingBalance == 40,
    "Historical-only groups survive merge");
Check(result.ReportData.Single(r => r.GroupTitle == "sales-only").ClosingBalance == 10,
    "Sales-only groups survive merge");

queries.Calls.Clear();
await handler.Handle(input with { FromDateJalali = "1404/01/01" }, CancellationToken.None);
Check(queries.Calls.Take(2).All(c => c.To == "1403/12/30"), "Leap Persian year boundary");
queries.Calls.Clear();
await handler.Handle(input with { FromDateJalali = "1330/01/01" }, CancellationToken.None);
Check(queries.Calls.Count == 2, "History start has zero opening balance and no reversed query");
foreach (var invalid in new[]
{
    input with { GroupBy = (WaterReceivablesGroupingEnum)0 },
    input with { FromDateJalali = "1405/1/1" },
    input with { FromDateJalali = "1405/13/01" },
    input with { ToDateJalali = "1404/01/01" },
    input with { FromDateJalali = "1329/12/29" },
    input with { FromDateJalali = null! }
})
    Check(!validator.Validate(invalid).IsValid, "Invalid grouping/date input rejected");

sealed class FakeQueries : IWaterReceivablesQueryService
{
    public List<(string From, string To)> Calls { get; } = new();
    public Task<IEnumerable<WaterReceivablesAmountOutputDto>> GetSales(WaterReceivablesGroupingEnum groupBy, string fromDate, string toDate, CancellationToken cancellationToken)
        => Get(fromDate, toDate, true);
    public Task<IEnumerable<WaterReceivablesAmountOutputDto>> GetCollections(WaterReceivablesGroupingEnum groupBy, string fromDate, string toDate, CancellationToken cancellationToken)
        => Get(fromDate, toDate, false);
    private Task<IEnumerable<WaterReceivablesAmountOutputDto>> Get(string from, string to, bool sales)
    {
        Calls.Add((from, to));
        bool opening = from == "1330/01/01" && to != "1405/03/31";
        var data = opening
            ? sales ? new[] { Row("home", 100), Row("history-only", 40) } : new[] { Row("home", 30) }
            : sales ? new[] { Row("home", 50), Row("sales-only", 10) } : new[] { Row("home", 20), Row("collections-only", 5) };
        return Task.FromResult<IEnumerable<WaterReceivablesAmountOutputDto>>(data);
    }
    private static WaterReceivablesAmountOutputDto Row(string title, long amount) => new() { GroupTitle = title, Amount = amount };
}
