using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ReturnBillOutputDto
    {
        public string Description { get; set; }
        public string IssueDateJalali { get; set; }
        public string MinutesNumber { get; set; }
        public string ZoneTitle { get; set; }
        public ReturnBillDataOutputDto Bill { get; set; }
        public ReturnBillDataOutputDto Repair { get; set; }
        public ReturnBillDataOutputDto AutoBack { get; set; }
        public ReturnBillOutputDto(string description, string zoneTitle, ReturnBillDataOutputDto bill, ReturnBillDataOutputDto repair, ReturnBillDataOutputDto autoBack)
        {
            Description = description;
            ZoneTitle = zoneTitle;

            IssueDateJalali = DateTime.Now.ToShortPersianDateString();
            MinutesNumber = bill.MinutesNumber.ToString();

            Bill = bill;
            Repair = repair;
            AutoBack = autoBack;
        }
    }
}
