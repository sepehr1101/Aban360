namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record SaleAndAfterSaleHeaderOutputDto
    {
        public long Amount { get; set; }
        public long? Discount { get; set; }
        public long FinalAmount { get; set; }
        public int ItemCount { get; set; }
        public SaleAndAfterSaleHeaderOutputDto(long amount,long? discount,long finalAmount,int itemCount)
        {
            Amount= amount;
            Discount=discount;
            FinalAmount=finalAmount;
            ItemCount=itemCount;
        }
        public SaleAndAfterSaleHeaderOutputDto()
        {
        }
    }
}
