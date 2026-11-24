using Aban360.ReportPool.Domain.Constants;
namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
public record WaterReceivablesHeaderOutputDto
{
    public string Title { get; set; } = string.Empty;
    public WaterReceivablesGroupingEnum GroupBy { get; set; }
    public string GroupByTitle { get; set; } = default!;
    public string FromDateJalali { get; set; } = string.Empty;
    public string ToDateJalali { get; set; } = string.Empty;
    public string ReportDateJalali { get; set; } = string.Empty;
    public int RecordCount { get; set; }
    //
    public long OpeningBalance { get; set; }
    public long PeriodSales { get; set; }
    public long PeriodCollections { get; set; }
    public long ClosingBalance => OpeningBalance + PeriodSales - PeriodCollections;
}
