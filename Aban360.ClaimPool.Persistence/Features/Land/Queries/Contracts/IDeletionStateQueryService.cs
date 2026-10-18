using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IDeletionStateQueryService
    {
        Task<IEnumerable<NumericDictionary>> Get();
        Task<NumericDictionary> Get(int id, bool hasException);
    }
}
