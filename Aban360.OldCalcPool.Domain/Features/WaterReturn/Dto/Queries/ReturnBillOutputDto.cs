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
        public ReturnBillDataOutputDto PreviousValues { get; set; }
        public ReturnBillDataOutputDto CurrentValues { get; set; }
        public ReturnBillDataOutputDto ReturnValues { get; set; }
        public ReturnBillOutputDto(string description, string zoneTitle, ReturnBillDataOutputDto previousValues, ReturnBillDataOutputDto currentValues, ReturnBillDataOutputDto returnValues)
        {
            Description = description;
            ZoneTitle = zoneTitle;

            IssueDateJalali = DateTime.Now.ToShortPersianDateString();
            MinutesNumber = previousValues.MinutesNumber.ToString();

            PreviousValues = previousValues;
            CurrentValues = currentValues;
            ReturnValues = returnValues;
        }
    }
}
