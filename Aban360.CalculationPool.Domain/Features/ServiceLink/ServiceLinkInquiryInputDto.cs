namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record ServiceLinkInquiryInputDto
    {
        public string PaymentId { get; set; }
        public string BillId { get; set; }
    }
}
