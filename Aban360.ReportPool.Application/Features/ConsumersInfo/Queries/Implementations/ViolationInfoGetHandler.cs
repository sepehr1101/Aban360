using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    internal class ViolationInfoGetHandler : IViolationInfoGetHandler
    {
        private readonly IViolationInfoService _violationSummaryInfoService;
        public ViolationInfoGetHandler(IViolationInfoService violationSummaryInfoService)
        {
            _violationSummaryInfoService = violationSummaryInfoService;
            _violationSummaryInfoService.NotNull(nameof(violationSummaryInfoService));
        }

        public async Task<IEnumerable<ViolationInfoDto>> Handle(string billId, CancellationToken cancellationToken)
        {
            var violationSummaryInfo = await _violationSummaryInfoService.GetInfo(billId);
            return violationSummaryInfo;
        }
    }
}
