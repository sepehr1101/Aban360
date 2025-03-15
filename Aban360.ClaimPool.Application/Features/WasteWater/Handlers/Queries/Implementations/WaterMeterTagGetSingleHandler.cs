using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Implementations
{
    internal sealed class WaterMeterTagGetSingleHandler : IWaterMeterTagGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterTagQueryService _waterMeterTagQueryService;
        public WaterMeterTagGetSingleHandler(
            IMapper mapper,
            IWaterMeterTagQueryService waterMeterTagQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _waterMeterTagQueryService = waterMeterTagQueryService;
            _waterMeterTagQueryService.NotNull(nameof(waterMeterTagQueryService));
        }

        public async Task<ICollection<WaterMeterTagGetDto>> Handle(int id, CancellationToken cancellationToken)
        {
            WaterMeterTag waterMeterTag = await _waterMeterTagQueryService.Get(id);
            return _mapper.Map<ICollection<WaterMeterTagGetDto>>(waterMeterTag);
        }
    }
}
