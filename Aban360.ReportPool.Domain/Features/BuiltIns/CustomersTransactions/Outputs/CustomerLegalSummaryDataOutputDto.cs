namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record CustomerLegalSummaryDataOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int LegalCount { get; set; }
        public int NaturalCount { get; set; }
        public int InvalidCount { get; set; }
    }
}
