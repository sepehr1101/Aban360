using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class MeterFlowGetHandler : IMeterFlowGetHandler
    {
        private readonly IMeterFlowService _meterFlowService;
        public MeterFlowGetHandler(IMeterFlowService meterFlowService)
        {
            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(meterFlowService));
        }

        public async Task<MeterFlowGetDto> Handle(int id, CancellationToken cancellationToken)
        {
            MeterFlowGetDto meterFlow = await _meterFlowService.Get(id);
            return meterFlow;
        }
    }
}
