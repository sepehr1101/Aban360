using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    public static class UserZoneIdsHandler
    {
        public static async Task<List<int>> Handler(Guid id,short claimTypeId)
        {
            List<int> zoneIds = await UserZoneIdsQueryService.GetInfo(id,claimTypeId);
            return zoneIds;
        }
    }
}
