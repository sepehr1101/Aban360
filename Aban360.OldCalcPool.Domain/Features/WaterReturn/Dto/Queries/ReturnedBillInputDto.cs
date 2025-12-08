using Aban360.OldCalcPool.Domain.Constants;

namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ReturnedBillInputDto
    {
        public int? ReturnCauseId { get; set; }
        public string  FromDateJalali { get; set; }
        public string  ToDateJalali { get; set; }
        public ReturnedBillCalculationTypeEnum? CalculationType { get; set; }
        public float? UserInput { get; set; }
    }
}
