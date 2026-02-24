using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts
{
    public interface IMeterFlowQueryService
    {
        Task<MeterFlowGetDto> Get(int id);
        Task<string?> GetInsertDateTime(string fileName);
        Task<MeterFlowValidationDto?> GetMeterFlowValidation(int id);
        Task<int> GetFirstFlowId(int latestFlowId);
        Task<IEnumerable<MeterFlowCartableGetDto>> GetCartable();
    }
}
