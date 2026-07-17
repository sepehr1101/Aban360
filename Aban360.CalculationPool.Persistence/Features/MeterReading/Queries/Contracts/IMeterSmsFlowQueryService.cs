using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts
{
    public interface IMeterSmsFlowQueryService
    {
        Task<MeterSmsFlowGetDto> Get(int id);
        Task<IEnumerable<MeterSmsFlowGetDto>> Get();
    }
}
