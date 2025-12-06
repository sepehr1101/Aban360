using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input
{
    public record RemovedBillSearchDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string ComparisonDateJalali { get; set; }
        public RemovedBillSearchDto(int zoneid, int customerNumber)
        {
            ZoneId = zoneid;
            CustomerNumber = customerNumber;
            ComparisonDateJalali = DateTime.Now.AddDays(-35).ToShortPersianDateString();
        }
    }
}
