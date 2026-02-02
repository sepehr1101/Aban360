using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations
{
    internal sealed class MunicipalityByZoneIdGetlHandler : IMunicipalityByZoneIdGetlHandler
    {
        private readonly IMunicipalityQueryService _municipalqueryService;
        public MunicipalityByZoneIdGetlHandler(IMunicipalityQueryService municipalqueryService)
        {
            _municipalqueryService = municipalqueryService;
            _municipalqueryService.NotNull(nameof(municipalqueryService));
        }

        public async Task<IEnumerable<NumericDictionary>> Handle(int zoneId,CancellationToken cancellationToken)
        {
            return  await _municipalqueryService.GetByZoneId(zoneId);
        }
    }
}
