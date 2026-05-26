namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record ServiceLinkPaymentRemoveInputDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public int Id { get; set; }
    }
}