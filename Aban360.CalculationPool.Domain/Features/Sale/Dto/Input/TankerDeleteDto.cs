using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record TankerDeleteDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public int UserCode { get; set; }
        public string CurrentDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public TankerDeleteDto(int zoneId, int customerNumber, int userCode)
        {
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            UserCode = userCode;
        }
        public TankerDeleteDto()
        {
        }
    }
}
