namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record WaterUsageGroupedDataOutputDto
    {
        public string UsageTitle { get; set; }
        public long Amount { get; set; }
    }
}
