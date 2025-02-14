using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Factories;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations
{
    internal sealed class LocationValueKeyQueryHandler : ILocationValueKeyQueryHandler
    {
        private readonly IZoneQueryService _zoneQueryService;
        public LocationValueKeyQueryHandler(IZoneQueryService zoneQueryService)
        {
            _zoneQueryService = zoneQueryService;
            _zoneQueryService.NotNull(nameof(zoneQueryService));
        }
        public async Task<LocationTree> Handle(CancellationToken cancellationToken)
        {
            ICollection<Zone> zones = await _zoneQueryService.GetIncludeAll();
            return zones.CreateLocationTree();
        }
        public async Task<LocationTree> Handle(ICollection<int> selectedZoneIds,CancellationToken cancellationToken)
        {
            ICollection<Zone> zones = await _zoneQueryService.GetIncludeAll();
            return zones.CreateLocationTree(selectedZoneIds);
        }
    }
}
