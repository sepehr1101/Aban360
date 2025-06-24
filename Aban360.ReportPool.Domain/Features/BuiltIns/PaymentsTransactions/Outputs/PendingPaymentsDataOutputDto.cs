namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record PendingPaymentsDataOutputDto
    {
        public string HeadquarterTitle { get; set; }//
        public string RegionTitle { get; set; }//
        public string ZoneTitle { get; set; }
        public string ReadingNumber { get; set; }
        public int CustomerNumber { get; set; }
        public string FirstName { get; set; }//+
        public string Surname { get; set; }//+
        public string UsageConsumptionTitle { get; set; }
        public string UsageSellTitle { get; set; }
        public string BillId { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string UseStateTitle { get; set; }
        public long DebtPeriodCount { get; set; }//DebtPeriodsAfterLastPayment
        public long BeginDebt { get; set; }//DebtBefore
        public long EndingDebt { get; set; }//FinalDebt
        public long PayedAmount { get; set; }//PaymentInInterval
    }

}
