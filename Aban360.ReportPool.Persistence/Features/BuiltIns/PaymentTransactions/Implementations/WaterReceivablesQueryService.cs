using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations;
internal sealed class WaterReceivablesQueryService : AbstractBaseConnection, IWaterReceivablesQueryService
{
    public WaterReceivablesQueryService(IConfiguration configuration) : base(configuration) { }

    public Task<IEnumerable<WaterReceivablesAmountOutputDto>> GetSales(WaterReceivablesGroupingEnum groupBy, string fromDate, string toDate, CancellationToken cancellationToken)
    {
        var (field, join) = GetGrouping(groupBy, "b");
        string query = $@"SELECT {field} AS GroupTitle,
                   /*SUM(CAST(b.Item1 AS bigint)
                       + CAST(b.Item2 AS bigint)
                       + CAST(b.Item3 AS bigint)
                       + CAST(b.Item4 AS bigint)
                       + CAST(b.Item5 AS bigint)
                       + CAST(b.Item6 AS bigint)
                       + CAST(b.Item7 AS bigint)
                       + CAST(b.Item8 AS bigint)
                       + CAST(b.Item9 AS bigint)
                       + CAST(b.Item10 AS bigint)
                       + CAST(b.Item11 AS bigint)
                       + CAST(b.Item12 AS bigint)
                       + CAST(b.Item13 AS bigint)
                       + CAST(b.Item14 AS bigint)
                       + CAST(b.Item15 AS bigint)
                       + CAST(b.Item16 AS bigint)
                       + CAST(b.Item17 AS bigint)
                       + CAST(b.Item18 AS bigint)) AS Amount*/
            SUM(CAST(b.SumItems AS bigint)) Amount   
            FROM [CustomerWarehouse].dbo.Bills b
            {join}
            WHERE b.RegisterDay BETWEEN @fromDate AND @toDate
              AND b.TypeCode IN (1, 3, 4, 5, 9) 
            GROUP BY {field}";
        return GetAmounts(query, fromDate, toDate, cancellationToken);
    }

    public Task<IEnumerable<WaterReceivablesAmountOutputDto>> GetCollections(WaterReceivablesGroupingEnum groupBy, string fromDate, string toDate, CancellationToken cancellationToken)
    {
        var (field, join) = GetGrouping(groupBy, "p");
        string query = $@"SELECT {field} AS GroupTitle,
                   SUM(CAST(p.Amount AS bigint)) AS Amount
            FROM [CustomerWarehouse].dbo.Payments p
            {join}
            WHERE p.RegisterDay BETWEEN @fromDate AND @toDate
            GROUP BY {field}";
        return GetAmounts(query, fromDate, toDate, cancellationToken);
    }

    private async Task<IEnumerable<WaterReceivablesAmountOutputDto>> GetAmounts(string query, string fromDate, string toDate, CancellationToken cancellationToken)
    {
        using var connection = _sqlReportConnection;
        return await connection.QueryAsync<WaterReceivablesAmountOutputDto>(
            new CommandDefinition(query, new { fromDate, toDate }, commandTimeout: 3600, cancellationToken: cancellationToken));
    }

    private static (string Field, string Join) GetGrouping(WaterReceivablesGroupingEnum groupBy, string alias)
    {
        return groupBy switch
        {
            WaterReceivablesGroupingEnum.Usage => ($"{alias}.UsageTitle", string.Empty),
            WaterReceivablesGroupingEnum.Zone => ($"{alias}.ZoneTitle", string.Empty),
            WaterReceivablesGroupingEnum.Region => ("t46.C2", $@"LEFT JOIN [Db70].dbo.T51 t51 ON t51.C0 = {alias}.ZoneId
                LEFT JOIN [Db70].dbo.T46 t46 ON t46.C0 = t51.C1"),
            _ => throw new ArgumentOutOfRangeException(nameof(groupBy))
        };
    }
}
