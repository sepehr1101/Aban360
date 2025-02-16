using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Db.Exceptions;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Implementations
{
    public class WaterMeterTagDefinitionDeleteHandler : IWaterMeterTagDefinitionDeleteHandler
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
            var waterMeterTagDefinition = await _queryService.Get(deleteDto.Id);
            if (waterMeterTagDefinition == null)
            {
                throw new InvalidIdException();//todo : exception
            }
            await _commandService.Remove(waterMeterTagDefinition);
        }
    }
}
