namespace Aban360.ReportPool.Domain.Features.Transactions
{
    public record BranchEventSummaryDataOutputDto
    {
        public string UsageTitle { get; set; }
        public int UsageId { get; set; }
        public string TrackNumber { get; set; }
        public string RegisterDateJalali { get; set; }
        public long DebtAmount { get; set; }
        public long CreditAmount { get; set; }
        public string BankDataJalali { get; set; }
        public string BankName { get; set; }
        public string Description { get; set; }
    }
}
