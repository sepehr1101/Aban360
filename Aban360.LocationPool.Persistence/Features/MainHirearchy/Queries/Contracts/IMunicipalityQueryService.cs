using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts
{
    public interface IMunicipalityQueryService
    {
        Task<Municipality> Get(int id);
        Task<ICollection<Municipality>> Get();
    }
}
