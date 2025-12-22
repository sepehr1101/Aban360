namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record PaymentInquiryOutputDto
    {
        public string? PayDateJalali { get; set; }
        public bool IsPayed { get; set; }
    }
}