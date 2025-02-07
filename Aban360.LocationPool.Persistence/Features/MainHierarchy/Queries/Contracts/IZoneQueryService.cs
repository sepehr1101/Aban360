using Aban360.LocationPool.Domain.Features.MainHierarchy;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts
{
    public interface IZoneQueryService
    {
        Task<Zone> Get(int id);
        Task<ICollection<Zone>> Get();
    }
}
