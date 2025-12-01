namespace Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries
{
    public record SGetDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int Olgo { get; set; }
        public string FromDateJalali { get; set; } = default!;
        public string ToDateJalali { get; set; } = default!;
    }
}
