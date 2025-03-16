using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Implementations
{
    internal sealed class WaterMeterTagDefinitionUpdateHandler : IWaterMeterTagDefinitionUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterTagDefinitionQueryService _queryService;
        public WaterMeterTagDefinitionUpdateHandler(
            IMapper mapper,
            IWaterMeterTagDefinitionQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(WaterMeterTagDefinitionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            WaterMeterTagDefinition waterMeterTagDefinition = await _queryService.Get(updateDto.Id);
            _mapper.Map(updateDto, waterMeterTagDefinition);
        }
    }
}
