using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.Transactions;

namespace Aban360.ReportPool.Application.Features.Transactions.Handler.Contracts
{
    public interface IBranchEventSummaryHandler
    {
        Task<ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto>> Handle(CardexInput input, CancellationToken cancellationToken);
        Task<ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto>> HandleWithLastDb(CardexInput input, CancellationToken cancellationToken);
    }
}
