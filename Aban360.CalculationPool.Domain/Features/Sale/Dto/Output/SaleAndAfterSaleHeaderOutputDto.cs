namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record SaleAndAfterSaleHeaderOutputDto
    {
        public Guid? TrackingId { get; set; }
        public long Amount { get; set; }
        public long? Discount { get; set; }
        public long FinalAmount { get; set; }
        public int ItemCount { get; set; }
        public SaleAndAfterSaleHeaderOutputDto(long amount,long? discount,long finalAmount,int itemCount, Guid? trackingId=null)
        {
            Amount= amount;
            Discount=discount;
            FinalAmount=finalAmount;
            ItemCount=itemCount;
            TrackingId = trackingId;
        }
        public SaleAndAfterSaleHeaderOutputDto()
        {
        }
    }
}
