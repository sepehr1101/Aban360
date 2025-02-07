using Aban360.LocationPool.Domain.Features.MainHierarchy;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts
{
    public interface IRegionQueryService
    {
        Task<Region> Get(int id);
        Task<ICollection<Region>> Get();
    }
}
