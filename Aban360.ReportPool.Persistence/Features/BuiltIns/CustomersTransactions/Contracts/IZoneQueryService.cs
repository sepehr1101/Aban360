using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts
{
    public interface IZoneQueryService
    {
        Task<IEnumerable<UserZoneIdsOutputDto>> Get();
        Task<bool> GetArticle2(int zoneId);
    }
}
