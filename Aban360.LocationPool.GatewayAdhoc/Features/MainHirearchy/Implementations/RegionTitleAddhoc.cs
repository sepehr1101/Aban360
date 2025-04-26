using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;

namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Implementations
{
    internal sealed class RegionTitleAddhoc : IRegionTitleAddhoc
    {
        private readonly IRegionGetSingleHandler _regionGetSingleHandler;
        public RegionTitleAddhoc(IRegionGetSingleHandler regionGetSingleHandler)
        {
            _regionGetSingleHandler = regionGetSingleHandler;
            _regionGetSingleHandler.NotNull(nameof(regionGetSingleHandler));
        }
        public async Task<string> Handle(int id, CancellationToken cancellationToken)
        {
            RegionGetDto region = await _regionGetSingleHandler.Handle(id, cancellationToken);
            return region.Title;
        }
    }
}
