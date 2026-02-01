using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations
{
    internal sealed class RegionDictionaryGetAllHandler : IRegionDictionaryGetAllHandler
    {
        private readonly IRegionQueryService _regionQueryService;
        public RegionDictionaryGetAllHandler(IRegionQueryService regionQueryService)
        {
            _regionQueryService = regionQueryService;
            _regionQueryService.NotNull(nameof(regionQueryService));
        }

        public async Task<IEnumerable<NumericDictionary>> Handle(CancellationToken cancellationToken)
        {
            return await _regionQueryService.GetDictionary();
        }
    }
}
