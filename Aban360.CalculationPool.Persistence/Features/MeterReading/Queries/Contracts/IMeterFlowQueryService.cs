using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.CollectBills.Inputs;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts
{
    public interface IMeterFlowQueryService
    {
        Task<MeterFlowGetDto> Get(int id);
        Task<string?> GetInsertDateTime(string fileName);
        Task<MeterFlowValidationDto?> GetMeterFlowValidation(int id);
        Task<int> GetFirstFlowId(int latestFlowId);
        Task<MeterFlowGetDto> GetLatestFlowInfo(int firstFlowId);
        Task<MeterFlowGetDto> GetLatestFlowInfo2(int firstFlowId);
        Task<IEnumerable<MeterFlowCartableGetDto>> GetCartable(IEnumerable<int> zoneIds);
        Task<IEnumerable<MeterFlowCartableGetDto>> GetCartable(MeterFlowByZoneInputDto inputDto, MeterFlowStepEnum stemp);
    }
}
