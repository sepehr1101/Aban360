namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record UnspecifiedPaymentDataOutputDto
    {
        public int CustomerNumber { get; set; }
        public string EventDateJalali { get; set; }
        public string BankDateJalali { get; set; }
        public int BankId { get; set; }
        public string BankName{ get; set; }
        public string BillId { get; set; }
        public string PaymentId { get; set; }
        public string PaymentDateJalali { get; set; }
        public long Amount { get; set; }
        public string PaymentGateway { get; set; }

    }
}

