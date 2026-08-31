namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record CustomerLegalSummaryDataOutputDto
    {
        public int ItemId { get; set; }
        public string ItemTitle { get; set; }
        public int ValidLegalCount { get; set; }
        public int InValidLegalCount { get; set; }
        public int ValidNaturalCount { get; set; }
        public int InValidNaturalCount { get; set; }
        public int InvalidCount { get; set; }
    }
}
