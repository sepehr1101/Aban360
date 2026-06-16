using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts
{
    public interface IT100QueryService
    {
        Task<NumericDictionary> Get(int id, bool hasException);
        Task<IEnumerable<NumericDictionary>> Get();
    }
}
