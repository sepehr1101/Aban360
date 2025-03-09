using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands
{
    public record TariffCalculationModeDeleteDto
    {
        public TariffCalculationModeEnum Id { get; set; }
    }
}
