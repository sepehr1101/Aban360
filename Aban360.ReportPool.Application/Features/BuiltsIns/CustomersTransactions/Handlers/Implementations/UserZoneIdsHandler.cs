using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    public static class UserZoneIdsHandler
    {
        public static async Task<ICollection<UserZoneIdsOutputDto>> Handler(Guid id,short claimTypeId)
        {
            //ICollection<UserZoneIdsOutputDto> zoneIds = await UserZoneIdsQueryService.GetInfo(id,claimTypeId);
            ICollection<UserZoneIdsOutputDto> zoneIds=await UserZoneIdsPrincipleQueryService.GetInfo(id,claimTypeId);
            return zoneIds;
        }
    }
}
