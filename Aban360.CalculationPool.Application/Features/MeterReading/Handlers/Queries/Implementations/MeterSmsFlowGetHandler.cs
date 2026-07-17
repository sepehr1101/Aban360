using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class MeterSmsFlowGetHandler : IMeterSmsFlowGetHandler
    {
        private readonly IMeterSmsFlowQueryService _meterSmsFlowService;
        public MeterSmsFlowGetHandler(IMeterSmsFlowQueryService meterSmsFlowService)
        {
            _meterSmsFlowService = meterSmsFlowService;
            _meterSmsFlowService.NotNull(nameof(meterSmsFlowService));
        }

        public async Task<MeterSmsFlowGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            MeterSmsFlowGetDto meterSmsFlow = await _meterSmsFlowService.Get(id);
            return meterSmsFlow;
        }
    }
}
