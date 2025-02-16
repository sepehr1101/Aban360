using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Db.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Implementations
{
    public class WaterMeterTagGetSingleHandler : IWaterMeterTagGetSingleHandler
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

        public async Task<ICollection<WaterMeterTagGetDto>> Handle(string billId, CancellationToken cancellationToken)
        {
            var waterMeterTag = await _waterMeterTagQueryService.Get(billId);
            if (waterMeterTag == null)
            {
                throw new InvalidIdException();
            }
            return _mapper.Map<ICollection<WaterMeterTagGetDto>>(waterMeterTag);
        }
    }
}
