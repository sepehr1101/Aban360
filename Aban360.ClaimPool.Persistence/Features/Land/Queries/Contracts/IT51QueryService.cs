using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IT51QueryService
    {
        Task<IEnumerable<NumericDictionary>> Get();
        Task<NumericDictionary> Get(int id, bool hasException);
        Task<string?> GetAddress(int id, bool hasException);
    }
}
