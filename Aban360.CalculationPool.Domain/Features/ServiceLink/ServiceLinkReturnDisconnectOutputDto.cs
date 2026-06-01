namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record ServiceLinkReturnDisconnectOutputDto
    {
        public string BillId { get; set; }
        public long Amount { get; set; }
    }
}
