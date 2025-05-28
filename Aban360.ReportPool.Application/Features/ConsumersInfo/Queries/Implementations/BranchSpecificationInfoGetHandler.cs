using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    internal class BranchSpecificationInfoGetHandler : IBranchSpecificationInfoGetHandler
    {
        private readonly IBranchSpecificationInfoService _BranchSpecificationSummaryInfoService;
        public BranchSpecificationInfoGetHandler(IBranchSpecificationInfoService BranchSpecificationSummaryInfoService)
        {
            _BranchSpecificationSummaryInfoService = BranchSpecificationSummaryInfoService;
            _BranchSpecificationSummaryInfoService.NotNull(nameof(BranchSpecificationSummaryInfoService));
        }

        public async Task<BranchSpecificationInfoDto> Handle(string billId, CancellationToken cancellationToken)
        {
            var BranchSpecificationSummaryInfo = await _BranchSpecificationSummaryInfoService.GetInfo(billId);
            return BranchSpecificationSummaryInfo;
        }
    }
}
