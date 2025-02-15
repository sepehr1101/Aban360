using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;

namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Implementations
{
    public class ZoneCountQueryAddhoc : IZoneCountQueryAddhoc
    {
        private readonly IZoneGetCountHandler _zoneGetCountHandler;
        public ZoneCountQueryAddhoc(IZoneGetCountHandler zoneGetCountHandler)
        {
            _zoneGetCountHandler = zoneGetCountHandler;
            _zoneGetCountHandler.NotNull(nameof(zoneGetCountHandler));
        }

        public async Task<int> GetCount(ICollection<int> input, CancellationToken cancellationToken)
        {
            return await _zoneGetCountHandler.Handle(input, cancellationToken);
        }
    }
}
