namespace Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries
{
    public record TariffConstantGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string Condition { get; set; } = null!;
        public string Key { get; set; } = null!;
        public string FromDateJalali { get; set; } = null!;
        public string ToDateJalali { get; set; } = null!;
        public string? Description { get; set; }
    }
}
