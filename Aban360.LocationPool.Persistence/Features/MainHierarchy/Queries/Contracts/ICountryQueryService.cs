using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts
{
    public interface ICountryQueryService
    {
        Task<Country> Get(short Id);
        Task<ICollection<Country>> Get();
    }
}
