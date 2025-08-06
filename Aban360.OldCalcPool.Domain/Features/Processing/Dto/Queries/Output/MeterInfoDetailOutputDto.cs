namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record MeterInfoDetailOutputDto
    {
        public string PreviousDateJalali { get; set; } = default!;
        public int PreviousNumber { get; set; }
        public string CurrentDateJalali { get; set; } = default!;
        public int CurrentNumber { get; set; }
    }
}
