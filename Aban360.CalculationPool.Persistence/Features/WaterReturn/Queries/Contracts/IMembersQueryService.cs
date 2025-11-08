using Aban360.CalculationPool.Domain.Features.WaterReturn.Dto.Queries;

namespace Aban360.CalculationPool.Persistence.Features.WaterReturn.Queries.Contracts
{
    public interface IMembersQueryService
    {
        Task<MemberGetDto> Get(string billId);
    }
}
