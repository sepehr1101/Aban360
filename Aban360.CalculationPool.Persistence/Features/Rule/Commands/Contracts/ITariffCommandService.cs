using Aban360.CalculationPool.Domain.Features.Rule.Entties;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts
{
    public interface ITariffCommandService
    {
        Task Add(Tariff tariff);
        Task Remove(Tariff tariff);
    }
}
