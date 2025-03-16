using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Implementations
{
    internal sealed class WaterMeterTagDefinitionDeleteHandler : IWaterMeterTagDefinitionDeleteHandler
    {
        private readonly IWaterMeterTagDefinitionCommandService _commandService;
        private readonly IWaterMeterTagDefinitionQueryService _queryService;
        public WaterMeterTagDefinitionDeleteHandler(
            IWaterMeterTagDefinitionCommandService commandService,
            IWaterMeterTagDefinitionQueryService queryService)
        {
            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(WaterMeterTagDefinitionDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            WaterMeterTagDefinition waterMeterTagDefinition = await _queryService.Get(deleteDto.Id);
            await _commandService.Remove(waterMeterTagDefinition);
        }
    }
}
