using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Implementations
{
    internal sealed class RegionUpdateHandler : IRegionUpdateHandler
    {
        private readonly IRegionQueryService _regionQueryService;
        private readonly IMapper _mapper;
        public RegionUpdateHandler(
            IMapper mapper,
            IRegionQueryService regionQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _regionQueryService = regionQueryService;
            _regionQueryService.NotNull(nameof(regionQueryService));
        }

        public async Task Handle(RegionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            Region region = await _regionQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, region);
        }
    }
}
