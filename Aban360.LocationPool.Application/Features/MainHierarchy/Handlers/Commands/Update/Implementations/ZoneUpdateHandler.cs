using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Implementations
{
    internal sealed class ZoneUpdateHandler : IZoneUpdateHandler
    {
        private readonly IZoneQueryService _zoneQueryService;
        private readonly IMapper _mapper;
        public ZoneUpdateHandler(
           IZoneQueryService zoneQueryService,
            IMapper mapper)
        {
            _zoneQueryService = zoneQueryService;
            _zoneQueryService.NotNull(nameof(zoneQueryService));

            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
        }

        public async Task Handle(ZoneUpdateDto updateDto, CancellationToken cancellationToken)
        {
            Zone zone = await _zoneQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, zone);
        }
    }
}
