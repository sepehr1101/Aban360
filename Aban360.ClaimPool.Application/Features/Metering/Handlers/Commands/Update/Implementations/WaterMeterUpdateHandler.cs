using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Implementations
{
    internal sealed class WaterMeterUpdateHandler : IWaterMeterUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterQueryService _queryService;
        public WaterMeterUpdateHandler(
            IMapper mapper,
            IWaterMeterQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(WaterMeterUpdateDto updateDto, CancellationToken cancellationToken)
        {
            WaterMeter waterMeter = await _queryService.Get(updateDto.Id);
            waterMeter.ValidFrom = DateTime.Now;
            waterMeter.InsertLogInfo = "SampleLogInfo";
            waterMeter.Hash = "SampleHash";

            _mapper.Map(updateDto, waterMeter);
        }
    }
}
