using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Queries;

namespace Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Contracts
{
    public interface IRepairQueryService
    {
        Task<RepairGetDto> Get(int id);
        Task<IEnumerable<RepairGetDto>> Get(string billId);
    }
}