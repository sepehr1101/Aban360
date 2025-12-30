namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record ServiceLinkRegisterManualInputDto
    {
        public string BillId { get; set; }
        public string BankCode { get; set; }
        public string BankDateJalali { get; set; }
        public string RegisterDateJalali { get; set; }
        public int PaymentGetwayId { get; set; }
        public double Amount { get; set; }
        public string CheckOrMinutesNumber { get; set; }
    }
}
