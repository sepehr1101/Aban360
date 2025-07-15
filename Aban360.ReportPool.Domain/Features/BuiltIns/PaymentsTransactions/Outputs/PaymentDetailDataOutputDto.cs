namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record PaymentDetailDataOutputDto
    {
        public string CustomerNumber { get; set; }
        public string BankDateJalali { get; set; }
        public int BankCode{ get; set; }
        public string EventBankDateJalali { get; set; }
        public string BillId { get; set; }
        public string PaymentMethodTitle { get; set; }
        public string PaymentDate { get; set; }
        public string RegisterAmount { get; set; }
        public string BankName { get; set; }


    }
}
