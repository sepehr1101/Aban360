namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record MeterInfoOutputDto
    {
        public string PreviousDateJalali { get; set; }
        public int PreviousNumber { get; set; }
        public string BranchType { get; set; }
    }
}
