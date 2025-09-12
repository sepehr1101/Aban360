namespace Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries
{
    public record ZaribCQueryDto
    {
        public int Id { get; set; }
        public string FromDateJalali { get; set; } = default!;
        public string ToDateJalali { get; set; } = default!;
        public int C { get; set; }
    }
}
