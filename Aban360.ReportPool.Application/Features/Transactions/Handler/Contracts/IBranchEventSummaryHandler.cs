using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.Transactions;

namespace Aban360.ReportPool.Application.Features.Transactions.Handler.Contracts
{
    public interface IBranchEventSummaryHandler
    {
        Task<ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto>> Handle(string input, CancellationToken cancellationToken);
        Task<ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto>> HandleWithLastDb(string input, string fromDate, CancellationToken cancellationToken);
    }
}
