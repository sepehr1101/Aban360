namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input
{
    public record MeterInfoInputDto
    {
        public string BillId { get; set; } = default!;
        public string CurrentDateJalali { get; set; } = default!;
        public int CurrentMeterNumber { get; set; }
    }
}
