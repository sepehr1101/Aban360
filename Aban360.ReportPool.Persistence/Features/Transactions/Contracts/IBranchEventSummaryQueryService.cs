using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.Transactions;

namespace Aban360.ReportPool.Persistence.Features.Transactions.Contracts
{
    public interface IBranchEventSummaryQueryService
    {
        Task<ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto>> Get(string billId);
    }
}
