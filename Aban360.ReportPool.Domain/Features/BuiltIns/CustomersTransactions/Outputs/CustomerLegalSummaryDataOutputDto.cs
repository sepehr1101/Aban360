namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record CustomerLegalSummaryDataOutputDto
    {
        public int ItemId { get; set; }
        public string ItemTitle { get; set; }
        public int LegalCount { get; set; }
        public int NaturalCount { get; set; }
        public int InvalidCount { get; set; }
    }
}
