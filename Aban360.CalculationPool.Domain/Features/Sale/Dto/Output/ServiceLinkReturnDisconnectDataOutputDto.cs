using System.Runtime.InteropServices;

namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record ServiceLinkReturnDisconnectDataOutputDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public long Amount { get; set; }
        public long Discount { get; set; }
        public long FinalAmount { get; set; }
        public int DiscountTypeId { get; set; }
        public ServiceLinkReturnDisconnectDataOutputDto(short id, string title, long? amount, long? discount, long? finalAmount, [Optional] int discountTypeId)
        {
            Id = id;
            Title = title;
            Amount = amount ?? 0;
            Discount = discount ?? 0;
            FinalAmount = finalAmount ?? 0;
            DiscountTypeId = discountTypeId;
        }
    }
}
