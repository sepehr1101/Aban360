using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Update.Implementations
{
    public class ZoneUpdateHandler : IZoneUpdateHandler
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
            var zone = await _zoneQueryService.Get(updateDto.Id);
            if (zone == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, zone);
        }
    }
}
