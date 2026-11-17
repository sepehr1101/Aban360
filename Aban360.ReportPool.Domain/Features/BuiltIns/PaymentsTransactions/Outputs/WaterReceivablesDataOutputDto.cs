namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
public record WaterReceivablesDataOutputDto
{
    public string GroupTitle { get; set; } = string.Empty;
    public long OpeningBalance { get; set; }
    public long PeriodSales { get; set; }
    public long PeriodCollections { get; set; }
    public long ClosingBalance => OpeningBalance + PeriodSales - PeriodCollections;
}
