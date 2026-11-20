using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Factories;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Services;
public interface ILocationLookupService
{
    Task<LocationTree> LocationValueKeyQueryAsync(CancellationToken cancellationToken);
    Task<LocationTree> LocationValueKeyQueryAsync(ICollection<int> selectedZoneIds, CancellationToken cancellationToken);
    Task<IEnumerable<NumericDictionary>> ZonesByRegionIdGetAsync(int id, CancellationToken cancellationToken);
}

internal sealed class LocationLookupService : ILocationLookupService
{
    private readonly IZoneQueryService _zoneQueryService;
    public LocationLookupService(IZoneQueryService zoneQueryService)
    {
        _zoneQueryService = zoneQueryService;
        _zoneQueryService.NotNull(nameof(zoneQueryService));
    }

    public async Task<LocationTree> LocationValueKeyQueryAsync(CancellationToken cancellationToken)
    {
        ICollection<Zone> zones = await _zoneQueryService.GetIncludeAll();
        return zones.CreateLocationTree();
    }

    public async Task<LocationTree> LocationValueKeyQueryAsync(ICollection<int> selectedZoneIds, CancellationToken cancellationToken)
    {
        ICollection<Zone> zones = await _zoneQueryService.GetIncludeAll();
        return zones.CreateLocationTree(selectedZoneIds);
    }

    public async Task<IEnumerable<NumericDictionary>> ZonesByRegionIdGetAsync(int id, CancellationToken cancellationToken)
    {
        return await _zoneQueryService.GetByRegionId(id);
    }
}
