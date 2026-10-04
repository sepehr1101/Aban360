using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record OtherExpensesItemsInsertInputDto
    {
        public int ZoneId { get; set; }
        public int UsageId { get; set; }
        public int ServiceId { get; set; }
        public long Amount { get; set; }
    }
}
