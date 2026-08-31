namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record CustomerLegalSummaryByZoneAndUsageDataOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public int ValidLegalCount { get; set; }
        public int InValidLegalCount { get; set; }
        public int ValidNaturalCount { get; set; }
        public int InValidNaturalCount { get; set; }
        public int InvalidCount { get; set; }
    }
}
