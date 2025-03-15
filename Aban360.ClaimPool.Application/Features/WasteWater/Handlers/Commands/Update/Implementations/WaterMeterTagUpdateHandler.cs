using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Implementations
{
    internal sealed class WaterMeterTagUpdateHandler : IWaterMeterTagUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterTagQueryService _waterMeterTagQueryService;
        public WaterMeterTagUpdateHandler(
            IMapper mapper,
            IWaterMeterTagQueryService waterMeterTagQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _waterMeterTagQueryService = waterMeterTagQueryService;
            _waterMeterTagQueryService.NotNull(nameof(waterMeterTagQueryService));
        }

        public async Task Handle(WaterMeterTagUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var waterMeterTag = await _waterMeterTagQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, waterMeterTag);
        }
    }
}
