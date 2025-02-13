using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Create.Implementations
{
    public class ZoneCreateHandler : IZoneCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IZoneCommandService _zoneCommandService;
        public ZoneCreateHandler(
            IMapper mapper,
            IZoneCommandService zoneCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _zoneCommandService = zoneCommandService;
            _zoneCommandService.NotNull(nameof(zoneCommandService));
        }

        public async Task Handle(ZoneCreateDto createDto, CancellationToken cancellationToken)
        {
            var zone = _mapper.Map<Zone>(createDto);
            await _zoneCommandService.Add(zone);
        }
    }
}
