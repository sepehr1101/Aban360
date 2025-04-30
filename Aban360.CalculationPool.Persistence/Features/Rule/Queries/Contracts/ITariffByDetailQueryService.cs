using Aban360.CalculationPool.Domain.Features.Rule.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts
{
    public interface ITariffByDetailQueryService
    {
        Task<TariffByDetail> Get(int id);
        Task<ICollection<TariffByDetail>> Get();
    }
}
