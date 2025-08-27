using Aban360.Common.Excel;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.Transactions;

namespace Aban360.ReportPool.Application.Features.Transactions.Handler.Contracts
{
    public interface IBranchEventSummaryHandler
    {
        Task<ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto>> Handle(string input, CancellationToken cancellationToken);
    }
}
