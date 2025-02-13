using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts
{
    public interface IZoneQueryService
    {
        Task<Zone> Get(int id);
        Task<ICollection<Zone>> Get();
    }
}
