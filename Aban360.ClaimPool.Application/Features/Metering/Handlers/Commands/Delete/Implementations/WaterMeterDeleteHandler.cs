using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Implementations
{
    public class WaterMeterDeleteHandler : IWaterMeterDeleteHandler
    {
        private readonly IWaterMeterQueryService _queryService;
        private readonly IWaterMeterCommandService _commandService;
        public WaterMeterDeleteHandler(
            IWaterMeterQueryService queryService,
            IWaterMeterCommandService commandService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));
        }

        public async Task Handle(WaterMeterDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var waterMeter = await _queryService.Get(deleteDto.Id);
            if (waterMeter == null)
            {
                throw new InvalidDataException();
            }
            await _commandService.Remove(waterMeter);
        }
    }
}
