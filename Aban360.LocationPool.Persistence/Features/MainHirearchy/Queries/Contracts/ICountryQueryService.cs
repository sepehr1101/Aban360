using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts
{
    public interface ICountryQueryService
    {
        Task<Country> Get(short Id);
        Task<ICollection<Country>> Get();
    }
}
