namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record ServiceLinkRegisterManualInputDto
    {
        public string BillId { get; set; }
        public IEnumerable<ServiceLinkRegisterItemDto>  PayItems{ get; set; }
    }
}