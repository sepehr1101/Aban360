using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Transactions.Handler.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;

namespace Aban360.ReportPool.Application.Features.Transactions.Handler.Implementations
{
    internal sealed class BranchEventSummaryHandler : IBranchEventSummaryHandler
    {
        private readonly IBranchEventSummaryQueryService _branchEventSummaryQueryService;
        public BranchEventSummaryHandler(IBranchEventSummaryQueryService branchEventSummaryQueryService)
        {
            _branchEventSummaryQueryService = branchEventSummaryQueryService;
            _branchEventSummaryQueryService.NotNull(nameof(branchEventSummaryQueryService));
        }

        public async Task<ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto>> Handle(string input,CancellationToken cancellationToken)
        {
            ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto> result = await _branchEventSummaryQueryService.Get(input);
            if (CanSetRamained(result))
            {
                result.ReportHeader.Remained = result.ReportData.Sum(data => data.DebtAmount) 
                                               - result.ReportData.Sum(data => data.CreditAmount)
                                               - result.ReportData.Sum(data=> data.DiscountAmount);
            }
            return result;
        }
        private bool CanSetRamained(ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto> result)
        {
            return
                result is not null &&
                result.ReportData is not null &&
                result.ReportData.Any() &&
                result.ReportHeader is not null;
        }
    }
}
