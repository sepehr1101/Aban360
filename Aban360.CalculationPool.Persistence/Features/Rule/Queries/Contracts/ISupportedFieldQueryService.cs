using Aban360.CalculationPool.Domain.Features.Rule.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts
{
    public interface ISupportedFieldQueryService
    {
        Task<SupportedField> Get(short id);
        Task<ICollection<SupportedField>> Get();
    }
}
