namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record DebtorByDaySummaryDataOutputDto
    {
        public string EventDateJalali { get; set; }
        public int Count { get; set; }
        public long Amount { get; set; }
        public long OffAmount { get; set; }
        public long FinalAmount { get; set; }
    }
}
