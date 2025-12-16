using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ReturnBillSearchDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string ComparisonDateJalali { get; set; }
        public ReturnBillSearchDto(int zoneid, int customerNumber)
        {
            ZoneId = zoneid;
            CustomerNumber = customerNumber;
            ComparisonDateJalali = DateTime.Now.AddYears(-1).ToShortPersianDateString();
        }
    }
}
