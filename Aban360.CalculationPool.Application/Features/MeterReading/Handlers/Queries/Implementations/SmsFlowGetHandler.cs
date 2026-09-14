using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class SmsFlowGetHandler : ISmsFlowGetHandler
    {
        private readonly ISmsFlowQueryService _meterSmsFlowService;
        public SmsFlowGetHandler(ISmsFlowQueryService meterSmsFlowService)
        {
            _meterSmsFlowService = meterSmsFlowService;
            _meterSmsFlowService.NotNull(nameof(meterSmsFlowService));
        }

        public async Task<SmsFlowGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            SmsFlowGetDto meterSmsFlow = await _meterSmsFlowService.Get(id, true);
            return meterSmsFlow;
        }
    }
}
