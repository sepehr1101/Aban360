namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record CustomerLegalSummaryByZoneAndUsageInputDto
    {
        public ICollection<int> ZoneIds { get; set; }
        public ICollection<int> UsageIds { get; set; }
    }
}
