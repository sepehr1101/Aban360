namespace Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries
{
    public record TariffCalculationModeGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
