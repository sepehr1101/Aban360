using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts
{
    public interface IHandoverQueryService
    {
        Task<IEnumerable<HandoverQueryDto>> Get();
    }
}
