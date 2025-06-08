using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    internal class FlatInfoGetHandler : IFlatInfoGetHandler
    {
        private readonly IFlatInfoService _branchSpecificationSummaryInfoService;
        public FlatInfoGetHandler(IFlatInfoService branchSpecificationSummaryInfoService)
        {
            _branchSpecificationSummaryInfoService = branchSpecificationSummaryInfoService;
            _branchSpecificationSummaryInfoService.NotNull(nameof(branchSpecificationSummaryInfoService));
        }

        public async Task<IEnumerable<FlatInfoDto>> Handle(string billId, CancellationToken cancellationToken)
        {
            var flatSummaryInfo = await _branchSpecificationSummaryInfoService.GetInfo(billId);
            return flatSummaryInfo;
        }
    }
}
