using Aban360.CalculationPool.Domain.Features.Rule.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts
{
    public interface ITariffByDetailCommandService
    {
        Task Add(TariffByDetail tariffByDetail);
        Task Remove(TariffByDetail tariffByDetail);
    }
}
