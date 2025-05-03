using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Implementations
{
    public class _WaterMeterGetByInfoForTariffCalcDetailHandler : I_WaterMeterGetByInfoForTariffCalcDetailHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterQueryService _WaterMeterQueryService;
        public _WaterMeterGetByInfoForTariffCalcDetailHandler(
            IMapper mapper,
            IWaterMeterQueryService WaterMeterQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _WaterMeterQueryService = WaterMeterQueryService;
            _WaterMeterQueryService.NotNull(nameof(WaterMeterQueryService));
        }

        public async Task<ICollection<WaterMeterGetDto>> Handle(string fromDate, string toDate, short usageId
            , short individualTypeId, string fromBillId, string toBillId, int zoneId, CancellationToken cancellationToken)
        {
            ICollection<WaterMeter> WaterMeter = await _WaterMeterQueryService.Get(fromDate, toDate, usageId
            , individualTypeId, fromBillId, toBillId, zoneId);
            
            if (WaterMeter == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<WaterMeterGetDto>>(WaterMeter);
        }
    }
}
