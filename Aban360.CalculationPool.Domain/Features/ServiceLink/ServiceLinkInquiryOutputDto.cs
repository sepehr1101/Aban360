namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record ServiceLinkInquiryOutputDto
    {
        public string ItemTitle { get; set; }
        public double Amount { get; set; }
        public double OffAmount { get; set; }
        public double FinalAmount { get; set; }
        public string? OffTitle { get; set; }
        public string TypeTitle { get; set; }
        public string RegisterDateJalali { get; set; }
    }
}
