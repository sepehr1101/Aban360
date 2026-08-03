namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record PendingPaymentSummaryDataOutputDto
    {
        public int ItemId { get; set; }
        public string ItemTitle { get; set; }
        public long DebtPeriodCount { get; set; }//DebtPeriodsAfterLastPayment
        public long BeginDebt { get; set; }//DebtBefore
        public long EndingDebt { get; set; }//FinalDebt
        public long PayedAmount { get; set; }//PaymentInInterval
    }
}
