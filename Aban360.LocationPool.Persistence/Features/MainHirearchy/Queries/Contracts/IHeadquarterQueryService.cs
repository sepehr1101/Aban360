using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts
{
    public interface IHeadquarterQueryService
    {
        Task<Headquarters> Get(short id);
        Task<ICollection<Headquarters>> Get();
    }
}
