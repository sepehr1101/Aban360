using Aban360.LocationPool.Application.Features.MainHierarchy.Services;
using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;

namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Implementations
{
    internal sealed class ZoneCountQueryAddhoc : IZoneCountQueryAddhoc
    {
        private readonly IZoneService _service;
        public ZoneCountQueryAddhoc(IZoneService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }

        public async Task<int> GetCount(ICollection<int> input, CancellationToken cancellationToken)
        {
            return await _service.GetCountAsync(input, cancellationToken);
        }
    }
}
