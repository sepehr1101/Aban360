namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input
{
    public record MeterInfoInputDto
    {
        public string BillId { get; set; }
        public string CurrentDateJalali { get; set; }
        public int CurrentMeterNumber { get; set; }
    }
}
