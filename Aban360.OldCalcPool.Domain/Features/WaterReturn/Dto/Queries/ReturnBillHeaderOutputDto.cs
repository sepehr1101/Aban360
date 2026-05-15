using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ReturnBillHeaderOutputDto
    {
        public string Description { get; set; }
        public string IssueDateJalali { get; set; }
        public int? ConfirmNumber { get; set; }
        public string? InputMinutesNumber { get; set; }
        public string ZoneTitle { get; set; }
        public bool HasReturned { get; set; }
        public ReturnBillHeaderOutputDto(string description, string zoneTitle, int? confirmNumber,string? inputMinutesNumber, bool hasReturned)
        {
            Description = description;
            ZoneTitle = zoneTitle;

            IssueDateJalali = DateTime.Now.ToShortPersianDateString();
            ConfirmNumber = confirmNumber;
            InputMinutesNumber = inputMinutesNumber;

            HasReturned = hasReturned;
        }
    }
}
