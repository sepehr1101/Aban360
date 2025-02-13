using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts
{
    public interface IMunicipalityQueryService
    {
        Task<Municipality> Get(int id);
        Task<ICollection<Municipality>> Get();
    }
}
