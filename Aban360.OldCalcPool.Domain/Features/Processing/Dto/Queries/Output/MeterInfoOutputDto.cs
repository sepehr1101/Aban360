namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record MeterInfoOutputDto
    {
        public string PreviousDateJalali { get; set; } = default!;
        public int PreviousNumber { get; set; }
    }
}
