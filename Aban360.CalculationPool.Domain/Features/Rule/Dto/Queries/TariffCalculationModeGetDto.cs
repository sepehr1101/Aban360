using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries
{
    public record TariffCalculationModeGetDto
    {
        public TariffCalculationModeEnum Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; } 
    }
}
