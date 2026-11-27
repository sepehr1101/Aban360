using Aban360.LocationPool.Application.Features.MainHierarchy.Services;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;

namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Implementations
{
    internal sealed class RegionTitleAddhoc : IRegionTitleAddhoc
    {
        private readonly IRegionService _service;

        public RegionTitleAddhoc(IRegionService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }
        public async Task<string> Handle(int id, CancellationToken cancellationToken)
        {
            RegionGetDto region = await _service.GetSingleAsync(id, cancellationToken);
            return region.Title;
        }
    }
}
