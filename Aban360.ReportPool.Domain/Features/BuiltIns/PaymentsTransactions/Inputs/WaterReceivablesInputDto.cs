using Aban360.ReportPool.Domain.Constants;
namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
public record WaterReceivablesInputDto
{
    public WaterReceivablesGroupingEnum GroupBy { get; set; }
    public string FromDateJalali { get; set; } = string.Empty;
    public string ToDateJalali { get; set; } = string.Empty;
    public ICollection<int>? ZoneIds { get; set; }
}
