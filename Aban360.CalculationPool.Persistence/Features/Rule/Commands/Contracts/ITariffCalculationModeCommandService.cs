using Aban360.CalculationPool.Domain.Features.Rule.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts
{
    public interface ITariffCalculationModeCommandService
    {
        Task Add(TariffCalculationMode tariffCalculationMode);
        Task Remove(TariffCalculationMode tariffCalculationMode);

    }
}
