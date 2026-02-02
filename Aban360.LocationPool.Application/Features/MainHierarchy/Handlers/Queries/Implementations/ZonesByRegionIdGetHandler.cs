using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations
{
    public class ZonesByRegionIdGetHandler : IZonesByRegionIdGetHandler
    {
        private readonly IZoneQueryService _zoneQueryService;
        public ZonesByRegionIdGetHandler(IZoneQueryService zoneQueryService)
        {
            _zoneQueryService = zoneQueryService;
            _zoneQueryService.NotNull(nameof(zoneQueryService));
        }

        public async Task<IEnumerable<NumericDictionary>> Handle(int id, CancellationToken cancellationToken)
        {
            return await _zoneQueryService.GetByRegionId(id);
        }
    }
}
