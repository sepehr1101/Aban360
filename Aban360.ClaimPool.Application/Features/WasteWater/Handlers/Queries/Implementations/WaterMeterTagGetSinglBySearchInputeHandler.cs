using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Db.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Implementations
{
    public class WaterMeterTagGetSinglBySearchInputeHandler : IWaterMeterTagGetSinglBySearchInputeHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterTagQueryService _waterMeterTagQueryService;
        public WaterMeterTagGetSinglBySearchInputeHandler(
            IMapper mapper,
            IWaterMeterTagQueryService waterMeterTagQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _waterMeterTagQueryService = waterMeterTagQueryService;
            _waterMeterTagQueryService.NotNull(nameof(waterMeterTagQueryService));
        }

        public async Task<WaterMeterTagGetDto> Handle(string input, CancellationToken cancellationToken)
        {
            var waterMeterTag = await _waterMeterTagQueryService.Get(input);
            if (waterMeterTag == null)
            {
                throw new InvalidIdException();
            }
            return _mapper.Map<WaterMeterTagGetDto>(waterMeterTag);
        }
    }
}
