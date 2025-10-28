using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record SaleDataOutputDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public long Amount { get; set; }
        public long? Discount { get; set; }
        public SaleDataOutputDto(short id, string title, long? amount, long? discount)
        {
            Id = id;
            Title = title;
            Amount = amount ?? 0;
            Discount = discount;
        }
    }
}
