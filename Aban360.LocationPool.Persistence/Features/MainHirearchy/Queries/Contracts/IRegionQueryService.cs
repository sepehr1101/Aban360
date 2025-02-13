using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts
{
    public interface IRegionQueryService
    {
        Task<Region> Get(int id);
        Task<ICollection<Region>> Get();
    }
}
