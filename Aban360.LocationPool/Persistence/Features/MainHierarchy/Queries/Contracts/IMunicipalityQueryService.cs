using Aban360.Common.BaseEntities;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts
{
    public interface IMunicipalityQueryService
    {
        Task<Municipality> Get(int id);
        Task<ICollection<Municipality>> Get();
        Task<IEnumerable<NumericDictionary>> GetByZoneId(int zoneId);
    }
}
