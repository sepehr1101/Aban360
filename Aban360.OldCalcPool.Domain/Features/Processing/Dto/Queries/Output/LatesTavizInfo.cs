namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record LatesTavizInfo
    {
        public int CustomerNumber { get; set; }
        public string? TavizDateJalali { get; set; }
        public string? TavizCause { get; set; }
        public string? TavizRegisterDateJalali { get; set; }
        public int? TavizNumber { get; set; }
    }
}
