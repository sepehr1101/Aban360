using Aban360.CalculationPool.Domain.Features.Rule.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts
{
    public interface ITariffCalculationModeQueryService
    {
        Task<TariffCalculationMode> Get(short id);
        Task<ICollection<TariffCalculationMode>> Get();
    }
}
