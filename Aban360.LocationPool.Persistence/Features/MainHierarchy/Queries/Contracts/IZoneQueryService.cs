using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts
{
    public interface IZoneQueryService
    {
        Task<Zone> Get(int id);
        Task<ICollection<Zone>> Get();
        Task<ICollection<Zone>> GetIncludeAll();
    }
}
