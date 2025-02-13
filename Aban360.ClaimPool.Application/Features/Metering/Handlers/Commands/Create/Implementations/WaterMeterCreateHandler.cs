using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Implementations
{
    public class WaterMeterCreateHandler : IWaterMeterCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterCommandService _commandService;
        public WaterMeterCreateHandler(
            IMapper mapper,
            IWaterMeterCommandService commandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));
        }

        public async Task Handle(WaterMeterCreateDto createDto, CancellationToken cancellationToken)
        {
            var waterMeter = _mapper.Map<WaterMeter>(createDto);
            await _commandService.Add(waterMeter);
        }
    }
}
