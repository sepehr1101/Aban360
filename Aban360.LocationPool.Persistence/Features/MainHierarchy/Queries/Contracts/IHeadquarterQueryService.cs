using Aban360.LocationPool.Domain.Features.MainHierarchy;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts
{
    public interface IHeadquarterQueryService
    {
        Task<Headquarters> Get(short id);
        Task<ICollection<Headquarters>> Get();
    }
}
