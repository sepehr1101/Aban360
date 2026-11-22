using Aban360.Common.Caching;
using Aban360.ReportPool.Application.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Validations;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

void Check(bool condition, string name)
{
    if (!condition) throw new Exception(name);
    Console.WriteLine("PASS: " + name);
}

var validator = new WaterReceivablesValidator();
var queries = new FakeQueries();
var services = new ServiceCollection();
services.AddReportPoolApplicationInjections();
services.AddSingleton<IWaterReceivablesQueryService>(queries);
services.AddSingleton<IValidator<WaterReceivablesInputDto>>(validator);
using var provider = services.BuildServiceProvider(validateScopes: true);
using var scope = provider.CreateScope();
var cache = provider.GetRequiredService<IQueryCache>();
var handler = scope.ServiceProvider.GetRequiredService<IWaterReceivablesHandler>();
Check(ReferenceEquals(cache, scope.ServiceProvider.GetRequiredService<IQueryCache>()),
    "Report handlers use the shared cache instance");
var input = new WaterReceivablesInputDto
{
    GroupBy = WaterReceivablesGroupingEnum.Usage,
    FromDateJalali = "1405/01/01",
    ToDateJalali = "1405/03/31"
};
var result = await handler.Handle(input, CancellationToken.None);
Check(queries.Calls.Count == 4, "Four independent sales/collection queries");
Check(queries.Calls.All(c => c.ZoneIds is null), "Omitted zoneIds is accepted and passed as null");
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
home.OpeningBalance = 0;
result.ReportHeader.OpeningBalance = 0;
var cached = await handler.Handle(input with { ZoneIds = Array.Empty<int>() }, CancellationToken.None);
Check(queries.Calls.Count == 0, "Empty and omitted zoneIds share an unfiltered cache entry");
Check(cached.ReportData.Single(r => r.GroupTitle == "home").OpeningBalance == 70 &&
    cached.ReportHeader.OpeningBalance == 110, "Mutating a response does not affect the cached report");

await handler.Handle(input with { ZoneIds = new[] { 102, 101, 102 } }, CancellationToken.None);
Check(queries.Calls.Count == 4 && queries.Calls.All(c => c.ZoneIds!.SequenceEqual(new[] { 101, 102 })),
    "The selected zone set is normalized for every query and is separate from the unfiltered entry");
await handler.Handle(input with { ZoneIds = new[] { 101, 102 } }, CancellationToken.None);
Check(queries.Calls.Count == 4, "Equivalent zone selections reuse the same cache entry");
await handler.Handle(input with { ZoneIds = new[] { 101 } }, CancellationToken.None);
Check(queries.Calls.Count == 8, "Different zone selections never reuse the same cache entry");
await handler.Handle(input with { GroupBy = WaterReceivablesGroupingEnum.Zone }, CancellationToken.None);
Check(queries.Calls.Count == 12, "Different report groupings use different cache entries");

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

var computation = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
int factoryCalls = 0;
Task<int> Compute(int input, CancellationToken token)
{
    factoryCalls++;
    return computation.Task.WaitAsync(token);
}
TimeSpan ttl = TimeSpan.FromMinutes(1);
var producer = cache.GetOrCreateAsync("concurrent", 1, ttl, Compute, CancellationToken.None);
using var waiterCancellation = new CancellationTokenSource();
var canceledWaiter = cache.GetOrCreateAsync("concurrent", 1, ttl, Compute, waiterCancellation.Token);
var waiter = cache.GetOrCreateAsync("concurrent", 1, ttl, Compute, CancellationToken.None);
waiterCancellation.Cancel();
try
{
    await canceledWaiter;
    throw new Exception("Canceled waiter returned a result");
}
catch (OperationCanceledException) { }
computation.SetResult(42);
Check((await Task.WhenAll(producer, waiter)).All(value => value == 42) && factoryCalls == 1,
    "Concurrent requests share one computation and a canceled waiter does not cancel it");

using var producerCancellation = new CancellationTokenSource();
var pending = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
var canceledProducer = cache.GetOrCreateAsync("retry", 1, ttl,
    (_, token) => pending.Task.WaitAsync(token), producerCancellation.Token);
var retryingWaiter = cache.GetOrCreateAsync("retry", 1, ttl, (_, _) => Task.FromResult(8), CancellationToken.None);
producerCancellation.Cancel();
try
{
    await canceledProducer;
    throw new Exception("Canceled producer returned a result");
}
catch (OperationCanceledException) { }
Check(await retryingWaiter.WaitAsync(TimeSpan.FromSeconds(5)) == 8,
    "An active waiter can retry after the producing request is canceled");

try
{
    await cache.GetOrCreateAsync<int, int>("failure", 1, ttl,
        (_, _) => throw new InvalidOperationException("Query failed"), CancellationToken.None);
    throw new Exception("A factory exception was not propagated");
}
catch (InvalidOperationException error) when (error.Message == "Query failed") { }
Check(await cache.GetOrCreateAsync("failure", 1, ttl, (_, _) => Task.FromResult(7), CancellationToken.None) == 7,
    "Failed computations do not prevent a retry");

var firstInput = new Dictionary<string, int[]> { ["zones"] = [1, 2], ["years"] = [1405] };
var sameInput = new Dictionary<string, int[]> { ["years"] = [1405], ["zones"] = [1, 2] };
await cache.GetOrCreateAsync("first-report", firstInput, ttl, (_, _) => Task.FromResult(10), CancellationToken.None);
Check(await cache.GetOrCreateAsync("first-report", sameInput, ttl, (_, _) => Task.FromResult(20), CancellationToken.None) == 10,
    "Equivalent JSON objects with different property order share one entry");
Check(await cache.GetOrCreateAsync("second-report", sameInput, ttl, (_, _) => Task.FromResult(20), CancellationToken.None) == 20,
    "Different operation prefixes isolate the same input");
sameInput["zones"] = [2, 1];
Check(await cache.GetOrCreateAsync("first-report", sameInput, ttl, (_, _) => Task.FromResult(30), CancellationToken.None) == 30,
    "Array order is preserved in the generic cache key");

sealed class FakeQueries : IWaterReceivablesQueryService
{
    public List<(string From, string To, ICollection<int>? ZoneIds)> Calls { get; } = new();
    public Task<IEnumerable<WaterReceivablesAmountOutputDto>> GetSales(WaterReceivablesGroupingEnum groupBy, string fromDate, string toDate, ICollection<int>? zoneIds, CancellationToken cancellationToken)
        => Get(fromDate, toDate, zoneIds, true);
    public Task<IEnumerable<WaterReceivablesAmountOutputDto>> GetCollections(WaterReceivablesGroupingEnum groupBy, string fromDate, string toDate, ICollection<int>? zoneIds, CancellationToken cancellationToken)
        => Get(fromDate, toDate, zoneIds, false);
    private Task<IEnumerable<WaterReceivablesAmountOutputDto>> Get(string from, string to, ICollection<int>? zoneIds, bool sales)
    {
        Calls.Add((from, to, zoneIds));
        bool opening = from == "1330/01/01" && to != "1405/03/31";
        var data = opening
            ? sales ? new[] { Row("home", 100), Row("history-only", 40) } : new[] { Row("home", 30) }
            : sales ? new[] { Row("home", 50), Row("sales-only", 10) } : new[] { Row("home", 20), Row("collections-only", 5) };
        return Task.FromResult<IEnumerable<WaterReceivablesAmountOutputDto>>(data);
    }
    private static WaterReceivablesAmountOutputDto Row(string title, long amount) => new() { GroupTitle = title, Amount = amount };
}
