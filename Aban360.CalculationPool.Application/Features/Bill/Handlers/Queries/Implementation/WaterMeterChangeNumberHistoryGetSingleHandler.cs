using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class WaterMeterChangeNumberHistoryGetSingleHandler : IWaterMeterChangeNumberHistoryGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterChangeNumberHistoryQueryService _readingPeriodQueryService;
        public WaterMeterChangeNumberHistoryGetSingleHandler(
            IMapper mapper,
            IWaterMeterChangeNumberHistoryQueryService readingPeriodQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _readingPeriodQueryService = readingPeriodQueryService;
            _readingPeriodQueryService.NotNull(nameof(_readingPeriodQueryService));
        }

        public async Task<WaterMeterChangeNumberHistoryGetDto> Handle(long id, CancellationToken cancellationToken)
        {
            var readingPeriod = await _readingPeriodQueryService.Get(id);
            return _mapper.Map<WaterMeterChangeNumberHistoryGetDto>(readingPeriod);
        }
    }
}
