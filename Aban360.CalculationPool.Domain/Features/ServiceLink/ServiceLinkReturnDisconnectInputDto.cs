namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record ServiceLinkReturnDisconnectInputDto
    {
        public string BillId { get; set; }
        public bool IsConfirm { get; set; }
    }
}
