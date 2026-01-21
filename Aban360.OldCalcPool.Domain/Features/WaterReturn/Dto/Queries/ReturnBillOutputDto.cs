using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ReturnBillOutputDto
    {
        public ReturnBillDataOutputDto PreviousValues { get; set; }
        public ReturnBillDataOutputDto CurrentValues { get; set; }
        public ReturnBillDataOutputDto ReturnValues { get; set; }
        public ReturnBillOutputDto(ReturnBillDataOutputDto previousValues, ReturnBillDataOutputDto currentValues, ReturnBillDataOutputDto returnValues)
        {

            PreviousValues = previousValues;
            CurrentValues = currentValues;
            ReturnValues = returnValues;
        }
    }
}
