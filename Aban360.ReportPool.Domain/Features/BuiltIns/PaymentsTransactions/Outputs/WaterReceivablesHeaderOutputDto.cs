using Aban360.ReportPool.Domain.Constants;
namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
public record WaterReceivablesHeaderOutputDto
{
    public string Title { get; set; } = string.Empty;
    public WaterReceivablesGroupingEnum GroupBy { get; set; }
    public string FromDateJalali { get; set; } = string.Empty;
    public string ToDateJalali { get; set; } = string.Empty;
    public string ReportDateJalali { get; set; } = string.Empty;
    public int RecordCount { get; set; }
}
