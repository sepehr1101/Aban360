using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands
{
    public record TariffCalculationModeCreateDto
    {
        public TariffCalculationModeEnum Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
