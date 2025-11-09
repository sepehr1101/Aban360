using Aban360.OldCalcPools.WaterReturn.Dto.Queries;

namespace Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Contracts
{
    public interface IMembersQueryService
    {
        Task<MemberGetDto> Get(string billId);
    }
}
