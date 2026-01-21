using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ReturnBillHeaderOutputDto
    {
        public string Description { get; set; }
        public string IssueDateJalali { get; set; }
        public string MinutesNumber { get; set; }
        public string ZoneTitle { get; set; }
        public ReturnBillHeaderOutputDto(string description, string zoneTitle,string minutesNumber)
        {
            Description = description;
            ZoneTitle = zoneTitle;

            IssueDateJalali = DateTime.Now.ToShortPersianDateString();
            MinutesNumber = minutesNumber;

        }
    }
}
