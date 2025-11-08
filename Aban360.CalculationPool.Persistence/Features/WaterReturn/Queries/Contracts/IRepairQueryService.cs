using Aban360.CalculationPool.Domain.Features.WaterReturn.Dto.Queries;

namespace Aban360.CalculationPool.Persistence.Features.WaterReturn.Queries.Contracts
{
    public interface IRepairQueryService
    {
        Task<RepairGetDto> Get(int id);
        Task<IEnumerable<RepairGetDto>> Get(string billId);
    }
}