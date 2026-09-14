using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class SmsFlowGetAllHandler : ISmsFlowGetAllHandler
    {
        private readonly ISmsFlowQueryService _meterSmsFlowService;
        public SmsFlowGetAllHandler(ISmsFlowQueryService meterSmsFlowService)
        {
            _meterSmsFlowService = meterSmsFlowService;
            _meterSmsFlowService.NotNull(nameof(meterSmsFlowService));
        }

        public async Task<IEnumerable<SmsFlowGetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<SmsFlowGetDto> meterSmsFlow = await _meterSmsFlowService.Get();
            return meterSmsFlow;
        }
    }
}
