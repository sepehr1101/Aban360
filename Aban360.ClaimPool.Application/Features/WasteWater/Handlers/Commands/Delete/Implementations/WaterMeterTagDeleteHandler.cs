using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Implementations
{
    internal sealed class WaterMeterTagDeleteHandler : IWaterMeterTagDeleteHandler
    {
        private readonly IWaterMeterTagCommandService _waterMeterTagCommandService;
        private readonly IWaterMeterTagQueryService _waterMeterTagQueryService;
        public WaterMeterTagDeleteHandler(
            IWaterMeterTagCommandService waterMeterTagCommandService,
            IWaterMeterTagQueryService waterMeterTagQueryService)
        {
            _waterMeterTagCommandService = waterMeterTagCommandService;
            _waterMeterTagCommandService.NotNull(nameof(waterMeterTagCommandService));

            _waterMeterTagQueryService = waterMeterTagQueryService;
            _waterMeterTagQueryService.NotNull(nameof(waterMeterTagQueryService));
        }

        public async Task Handle(WaterMeterTagDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var waterMeterTag = await _waterMeterTagQueryService.Get(deleteDto.Id);
            await _waterMeterTagCommandService.Remove(waterMeterTag);
        }
    }
}
