using Aban360.Common.Extensions;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations
{
    public class ZoneGetCountHandler: IZoneGetCountHandler
    {
        private readonly IZoneQueryService _zoneQueryService;
        private readonly IMapper _mapper;
        public ZoneGetCountHandler(
           IZoneQueryService zoneQueryService,
            IMapper mapper)
        {
            _zoneQueryService = zoneQueryService;
            _zoneQueryService.NotNull(nameof(zoneQueryService));

            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
        }

        public async Task<int> Handle(ICollection<int> input,CancellationToken cancellationToken)
        {
            return await _zoneQueryService.GetCount(input);
        }
    }


}
