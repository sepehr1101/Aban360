using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts
{
    public interface IT9QueryService
    {
        Task<IEnumerable<NumericDictionary>> Get();
        Task<NumericDictionary> Get(int id);
        Task<IEnumerable<NumericDictionary>> GetByTypeId(int typeId);
    }
}
