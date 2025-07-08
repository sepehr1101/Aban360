namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record InvalidPaymentDataOutputDto
    {
        public string CustomerNumber { get; set; }
        public string BillId { get; set; }
        public int ZoneId { get; set; }
        public string PayId { get; set; }
        public string BankAbbriviation { get; set; }
        public int BankCode { get; set; }
        public string CheckState { get; set; }
        public string BankDateJalali { get; set; }
        public long Amount { get; set; }
    }
}
