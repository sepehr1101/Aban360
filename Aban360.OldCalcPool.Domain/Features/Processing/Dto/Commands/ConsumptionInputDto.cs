namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record ConsumptionInputDto
    {
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public int PreviousMeterNumber { get; set; }
        public int CurrentMeterNumber { get; set; }
    }
}
