namespace Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries
{
    public record SupportedOperatorGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}