namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record PaymentInquiryDataOutputDto
    {
        public string  PaymentDateJalali { get; set; }
        public string PaymentMethod { get; set; }
        public int BankCode { get; set; }
        public string BankTitle { get; set; }
        public string BillId { get; set; }
        public string PaymentId { get; set; }
    }
}