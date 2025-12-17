namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs
{
    public record PaymentInquiryInputDto
    {
        public string BillId { get; set; }
        public string PaymentId { get; set; }
        public string FromDateJalali { get; set; }
    }
}
