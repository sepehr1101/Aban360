using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record TankerWaterHeaderOutputDto
    {
        public int RecordCount { get; set; }
        public string RegisterDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
        public long Amount { get; set; }
        public TankerWaterHeaderOutputDto(int recordCount,string title,long amount)
        {
            RecordCount = recordCount;
            Title = title;
            Amount = amount;
        }
        public TankerWaterHeaderOutputDto()
        {
        }
    }
}
