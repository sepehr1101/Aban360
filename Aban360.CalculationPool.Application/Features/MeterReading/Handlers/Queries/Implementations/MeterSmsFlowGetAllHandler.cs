using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class MeterSmsFlowGetAllHandler : IMeterSmsFlowGetAllHandler
    {
        private readonly IMeterSmsFlowQueryService _meterSmsFlowService;
        public MeterSmsFlowGetAllHandler(IMeterSmsFlowQueryService meterSmsFlowService)
        {
            _meterSmsFlowService = meterSmsFlowService;
            _meterSmsFlowService.NotNull(nameof(meterSmsFlowService));
        }

        public async Task<IEnumerable<MeterSmsFlowGetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<MeterSmsFlowGetDto> meterSmsFlow = await _meterSmsFlowService.Get();
            return meterSmsFlow;
        }
    }
}
