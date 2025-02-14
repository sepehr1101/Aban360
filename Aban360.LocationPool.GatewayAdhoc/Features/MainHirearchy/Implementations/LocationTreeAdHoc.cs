﻿using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;

namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Implementations
{
    internal sealed class LocationTreeAdHoc : ILocationTreeAdHoc
    {
        private readonly ILocationValueKeyQueryHandler _locationValueKeyQueryHandler;
        public LocationTreeAdHoc(ILocationValueKeyQueryHandler locationValueKeyQueryHandler)
        {
            _locationValueKeyQueryHandler = locationValueKeyQueryHandler;
            _locationValueKeyQueryHandler.NotNull(nameof(locationValueKeyQueryHandler));
        }
        public async Task<LocationTree> Handle(ICollection<int> selectedZoneIds, CancellationToken cancellationToken)
        {
            return await _locationValueKeyQueryHandler.Handle(selectedZoneIds, cancellationToken);
        }
    }
}
