namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record SaleHeaderOutputDto
    {
        public bool HasBroker { get; set; }
        public long CompanyAmount { get; set; }
        public long? BrokerAmount { get; set; }
        public int CompanyOfferingCount { get; set; }
        public int? BrokerOfferingCount { get; set; }

        public long OfferingAmount { get; set; }
        public int OfferingCount { get; set; }
    }
}
