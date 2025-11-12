namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record SaleHeaderOutputDto
    {
        public bool HasBroker { get; set; }
        public long CompanyAmount { get; set; }
        public long? CompanyDiscountAmount { get; set; }
        public long CompanyFinalAmount { get; set; }
        public int CompanyItemCount { get; set; }

        public long? BrokerAmount { get; set; }
        public int? BrokerItemCount { get; set; }

        public long SumAmount { get; set; }
        public long PayableAmount { get; set; }
        public int ItemCount { get; set; }
    }
}
