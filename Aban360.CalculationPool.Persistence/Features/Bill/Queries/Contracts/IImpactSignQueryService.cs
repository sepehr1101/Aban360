using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts
{
    public interface IImpactSignQueryService
    {
        Task<ImpactSign> Get(short id);
        Task<ICollection<ImpactSign>> Get();
    }
}
