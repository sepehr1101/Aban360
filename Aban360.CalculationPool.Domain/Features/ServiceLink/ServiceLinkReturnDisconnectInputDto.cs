namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record ServiceLinkReturnDisconnectInputDto
    {
        public string BillId { get; set; }
        public int DiscountTypeId { get; set; }
        public int DiscountCount { get; set; }
        public string? BlockCode { get; set; }
        public bool IsConfirm { get; set; }
    }
}
