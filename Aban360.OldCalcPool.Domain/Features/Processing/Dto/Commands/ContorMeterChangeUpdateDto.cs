namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record ContorMeterChangeUpdateDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string MeterChangeDateJalali { get; set; }
        public int MeterChangeNumber { get; set; }
    }
}
