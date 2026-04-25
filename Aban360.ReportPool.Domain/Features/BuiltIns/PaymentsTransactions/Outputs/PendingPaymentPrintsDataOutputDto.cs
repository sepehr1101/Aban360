namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record PendingPaymentPrintsDataOutputDto
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string UsageTitle { get; set; }
        public string MobileNumber { get; set; }
        public string BillId { get; set; }  
        public string PayId { get; set; }
        public long DebtAmount { get; set; }
        public int DebtPeriodCount { get; set; }
    }
}
