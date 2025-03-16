using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Implementations
{
    internal sealed class WaterMeterTagDefinitionCreateHandler : IWaterMeterTagDefinitionCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterTagDefinitionCommandService _commandService;
        private readonly IWaterMeterTagDefinitionQueryService _queryService;
        public WaterMeterTagDefinitionCreateHandler(
            IMapper mapper,
            IWaterMeterTagDefinitionCommandService commandService,
            IWaterMeterTagDefinitionQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(WaterMeterTagDefinitionCreateDto createDto, CancellationToken cancellationToken)
        {
            var waterMeterTagDefinition = _mapper.Map<WaterMeterTagDefinition>(createDto);
            await _commandService.Add(waterMeterTagDefinition);
        }
    }
}
