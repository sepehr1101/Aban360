using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using ILocationLookupService = global::Aban360.LocationPool.Application.Features.MainHierarchy.Services.ILocationLookupService;

namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Implementations
{
    internal sealed class LocationTreeAdHoc : ILocationTreeAdHoc
    {
        private readonly ILocationLookupService _locationLookupService;
        public LocationTreeAdHoc(ILocationLookupService locationLookupService)
        {
            _locationLookupService = locationLookupService;
            _locationLookupService.NotNull(nameof(locationLookupService));
        }

        public async Task<LocationTree> Handle(ICollection<int> selectedZoneIds, CancellationToken cancellationToken)
        {
            return await _locationLookupService.LocationValueKeyQueryAsync(selectedZoneIds, cancellationToken);
        }

        public async Task<LocationTree> Handle(CancellationToken cancellationToken)
        {
            return await _locationLookupService.LocationValueKeyQueryAsync(cancellationToken);
        }
    }
}
