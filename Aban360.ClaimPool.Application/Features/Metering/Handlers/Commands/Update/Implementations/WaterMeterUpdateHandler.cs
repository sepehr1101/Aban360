using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Implementations
{
    public class WaterMeterUpdateHandler : IWaterMeterUpdateHandler
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
            var waterMeter = await _queryService.Get(updateDto.Id);
            if (waterMeter == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, waterMeter);
        }
    }
}
