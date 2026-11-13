using Aban360.Api.Controllers.V1;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;

namespace BeforeServiceConsumersWave20.Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Implementations
{
    internal sealed class ZoneTitleAddhoc : IZoneTitleAddhoc
    {
        private readonly IZoneGetSingleHandler _zoneGetSingleHandler;
        public ZoneTitleAddhoc(IZoneGetSingleHandler zoneGetSingleHandler)
        {
            _zoneGetSingleHandler = zoneGetSingleHandler;
            _zoneGetSingleHandler.NotNull(nameof(zoneGetSingleHandler));
        }
        public async Task<string> Handle(int id, CancellationToken cancellationToken)
        {
            ZoneGetDto zone = await _zoneGetSingleHandler.Handle(id, cancellationToken);
            return zone.Title;
        }
    }
}
