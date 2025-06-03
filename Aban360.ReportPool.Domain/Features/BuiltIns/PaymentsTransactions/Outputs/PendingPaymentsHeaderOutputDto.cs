namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record PendingPaymentsHeaderOutputDto
    {
        public int RecordCount { get; set; }
        public long TotalDeptPeriodCount { get; set; }
        public long TotalBeginDebt { get; set; }
        public long TotalEndingDebt { get; set; }
        public long TotalPayedAmount { get; set; }
    }

}
