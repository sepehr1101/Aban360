namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record PendingPaymentSummaryByZoneAndUsageDataOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public long DebtPeriodCount { get; set; }//DebtPeriodsAfterLastPayment
        public long BeginDebt { get; set; }//DebtBefore
        public long EndingDebt { get; set; }//FinalDebt
        public long PayedAmount { get; set; }//PaymentInInterval
    }
}
