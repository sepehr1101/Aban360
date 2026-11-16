namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
public record WaterReceivablesAmountOutputDto
{
    public string GroupTitle { get; set; } = string.Empty;
    public long Amount { get; set; }
}
