using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts
{
    public interface IMeterReadingValidateHandler
    {
        HighLowEnum GetAttentionState(MeterReadingDetailDataOutputDto meterReading, MeterFlowStepEnum latestFlowStep);
        bool IsAttentionCounterState(int counterStateCode);
    }
}
