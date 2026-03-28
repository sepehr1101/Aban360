namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record SaleAndAfterSaleDataOutputDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public long Amount { get; set; }
        public long? Discount { get; set; }
        public long FinalAmount { get; set; }
        public int DiscountTypeId { get; set; }
        public bool Removable { get; set; }
        public SaleAndAfterSaleDataOutputDto(short id, string title, long? amount, long? discount, long? finalAmount, int discountTypeId, bool removable)
        {
            Id = id;
            Title = title;
            Amount = amount ?? 0;
            Discount = discount;
            FinalAmount = finalAmount ?? 0;
            DiscountTypeId = discountTypeId;
            Removable = removable;
        }
    }
}
