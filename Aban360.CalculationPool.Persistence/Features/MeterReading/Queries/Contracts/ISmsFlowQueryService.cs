using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts
{
    public interface ISmsFlowQueryService
    {
        Task<SmsFlowGetDto?> Get(int id, bool hasException);
        Task<SmsFlowGetDto> GetLastByFirstFlowId(int id);
        Task<IEnumerable<SmsFlowGetDto>> Get();
    }
}
