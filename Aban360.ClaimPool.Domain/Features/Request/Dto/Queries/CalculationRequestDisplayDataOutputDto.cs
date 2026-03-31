namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record CalculationRequestDisplayDataOutputDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public long Amount { get; set; }
        public long Discount { get; set; }
        public int DiscountTypeId { get; set; }
        public bool Removable { get; set; }
        public CalculationRequestDisplayDataOutputDto(int id,string title,long amount,long discount,int discountTypeId, bool removable)
        {
            Id = id;
            Title = title;
            Amount = amount;
            Discount = discount;
            DiscountTypeId = discountTypeId;
            Removable = removable;
        }
        public CalculationRequestDisplayDataOutputDto()
        {
        }
    }
}   
