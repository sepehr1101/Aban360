using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Implementations
{
    public class RegionCreateHandler : IRegionCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRegionCommandService _regionCommandService;
        public RegionCreateHandler(
            IMapper mapper,
            IRegionCommandService regionCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _regionCommandService = regionCommandService;
            _regionCommandService.NotNull(nameof(regionCommandService));
        }

        public async Task Handle(RegionCreateDto createDto, CancellationToken cancellationToken)
        {
            var region = _mapper.Map<Region>(createDto);
            await _regionCommandService.Add(region);
        }
    }

}
