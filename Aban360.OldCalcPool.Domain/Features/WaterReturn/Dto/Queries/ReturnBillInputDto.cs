using Aban360.OldCalcPool.Domain.Constants;

namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ReturnBillInputDto
    {
        public string BillId { get; set; }
        public int ReturnCauseId { get; set; }
        public ReturnedBillCalculationTypeEnum CalculationType { get; set; }
        public float? UserInput { get; set; }
        public int? MinutesNumber { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public bool IsConfirm { get; set; }
        public string? Description { get; set; }
    }
}
