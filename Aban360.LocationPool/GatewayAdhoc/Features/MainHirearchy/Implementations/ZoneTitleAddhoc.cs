using Aban360.LocationPool.Application.Features.MainHierarchy.Services;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;

namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Implementations
{
    internal sealed class ZoneTitleAddhoc : IZoneTitleAddhoc
    {
        private readonly IZoneService _service;
        public ZoneTitleAddhoc(IZoneService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }
        public async Task<string> Handle(int id, CancellationToken cancellationToken)
        {
            ZoneGetDto zone = await _service.GetSingleAsync(id, cancellationToken);
            return zone.Title;
        }
    }
}
