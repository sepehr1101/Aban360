using Aban360.CalculationPool.Domain.Features.Rule.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts
{
    public interface ISupportedOperatorQueryService
    {
        Task<SupportedOperator> Get(short id);
        Task<ICollection<SupportedOperator>> Get();
    }
}
