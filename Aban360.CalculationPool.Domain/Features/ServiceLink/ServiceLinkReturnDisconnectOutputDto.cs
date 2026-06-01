namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record ServiceLinkReturnDisconnectOutputDto
    {
        public string BillId { get; set; }
        public long WaterSubscriptionAmount { get; set; }
        public long SewageSubscriptionAmount { get; set; }
        public long SubscriptionAmount { get { return WaterSubscriptionAmount + SewageSubscriptionAmount; } }
    }
}
