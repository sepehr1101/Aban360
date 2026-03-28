namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record CalculationRequestDisplayHeaderOutputDto
    {
        public long Amount { get; set; }
        public long Discount { get; set; }
        public long Payable { get; set; }
        public CalculationRequestDisplayHeaderOutputDto(long amount,long discount,long payable)
        {
            Amount = amount;
            Discount = discount;
            Payable = payable;
        }
        public CalculationRequestDisplayHeaderOutputDto()
        {
        }
    }
}   
