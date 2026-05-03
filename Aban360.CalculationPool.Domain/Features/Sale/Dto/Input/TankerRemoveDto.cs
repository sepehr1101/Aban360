using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record TankerRemoveDto
    {
        public int CustomerNumber { get; set; }
        public int UserCode { get; set; }
        public string CurrentDateJalali { get; set; }=DateTime.Now.ToShortPersianDateString();
    }
}
