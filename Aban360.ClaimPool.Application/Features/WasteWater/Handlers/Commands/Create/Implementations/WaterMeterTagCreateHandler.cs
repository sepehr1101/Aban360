using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Implementations
{
    internal sealed class WaterMeterTagCreateHandler : IWaterMeterTagCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterTagCommandService _waterMeterTagCommandService;
        public WaterMeterTagCreateHandler(
            IMapper mapper,
            IWaterMeterTagCommandService waterMeterTagCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _waterMeterTagCommandService = waterMeterTagCommandService;
            _waterMeterTagCommandService.NotNull(nameof(waterMeterTagCommandService));
        }

        public async Task Handle(WaterMeterTagCreateDto createDto, CancellationToken cancellationToken)
        {
            WaterMeterTag waterMeterTag = _mapper.Map<WaterMeterTag>(createDto);
            await _waterMeterTagCommandService.Add(waterMeterTag);
        }
    }
}
