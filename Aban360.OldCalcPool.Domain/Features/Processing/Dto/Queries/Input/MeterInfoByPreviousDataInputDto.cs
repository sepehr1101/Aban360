namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input
{
    public record MeterInfoByPreviousDataInputDto
    {
        public string? BillId { get; set; } 
        public string PreviousDateJalali { get; set; } = default!;
        public int PreviousNumber { get; set; }

        public string CurrentDateJalali { get; set; } = default!;
        public int CurrentMeterNumber { get; set; }     

        public int? CounterStateCode { get; set; }
    }
}
