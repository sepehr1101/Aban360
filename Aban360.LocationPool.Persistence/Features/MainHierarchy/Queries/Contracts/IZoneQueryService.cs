using Aban360.Common.BaseEntities;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts
{
    public interface IZoneQueryService
    {
        Task<Zone> Get(int id);
        Task<ICollection<Zone>> Get();
        Task<ICollection<Zone>> GetIncludeAll();
        Task<int> GetCount(ICollection<int> ids);
        Task<IEnumerable<NumericDictionary>> GetByRegionId(int regionId);
    }
}
