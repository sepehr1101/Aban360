using Aban360.Common.BaseEntities;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts
{
    public interface IMunicipalityByZoneIdGetlHandler
    {
        Task<IEnumerable<NumericDictionary>> Handle(int zoneId, CancellationToken cancellationToken);
    }
}
