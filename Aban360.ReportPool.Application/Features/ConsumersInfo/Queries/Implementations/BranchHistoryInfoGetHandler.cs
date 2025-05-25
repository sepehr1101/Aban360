using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    internal class BranchHistoryInfoGetHandler : IBranchHistoryInfoGetHandler
    {
        private readonly IBranchHistoryInfoService _branchSpecificationSummaryInfoService;
        public BranchHistoryInfoGetHandler(IBranchHistoryInfoService branchSpecificationSummaryInfoService)
        {
            _branchSpecificationSummaryInfoService = branchSpecificationSummaryInfoService;
            _branchSpecificationSummaryInfoService.NotNull(nameof(branchSpecificationSummaryInfoService));
        }

        public async Task<BranchHistoryInfoDto> Handle(string billId, CancellationToken cancellationToken)
        {
            var branchSpecificationSummaryInfo = await _branchSpecificationSummaryInfoService.GetInfo(billId);
            return branchSpecificationSummaryInfo;
        }
    }
}
