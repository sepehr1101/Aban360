namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record UsageSummaryDataOutputDto
    {
        public int TotalUnit { get; set; }
        public string UsageTitle { get; set; }
        public string ZoneTitle { get; set; }
    }
}
