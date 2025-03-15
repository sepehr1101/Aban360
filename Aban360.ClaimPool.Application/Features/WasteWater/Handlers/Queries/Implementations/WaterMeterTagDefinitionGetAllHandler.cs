using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Implementations
{
    internal sealed class WaterMeterTagDefinitionGetAllHandler : IWaterMeterTagDefinitionGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterTagDefinitionQueryService _queryService;
        public WaterMeterTagDefinitionGetAllHandler(
            IMapper mapper,
            IWaterMeterTagDefinitionQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<ICollection<WaterMeterTagDefinitionGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<WaterMeterTagDefinition> waterMeterTagDefinition = await _queryService.Get();
            return _mapper.Map<ICollection<WaterMeterTagDefinitionGetDto>>(waterMeterTagDefinition);
        }
    }
}
