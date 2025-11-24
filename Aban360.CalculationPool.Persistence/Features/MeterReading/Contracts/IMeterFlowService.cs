using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts
{
    public interface IMeterFlowService
    {
        Task<int> Create(MeterFlowCreateDto input);
        Task Update(MeterFlowUpdateDto input);
        Task<MeterFlowGetDto> Get(int id);
        Task<string?> Get(string fileName);
        Task<int> GetFirstFlowId(int latestFlowId);
        Task<IEnumerable<MeterFlowCartableGetDto>> GetCartable();
    }
}
