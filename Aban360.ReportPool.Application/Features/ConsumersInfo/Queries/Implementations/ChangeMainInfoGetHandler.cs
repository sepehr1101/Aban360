using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    internal class ChangeMainInfoGetHandler : IChangeMainInfoGetHandler
    {
        private readonly IChangeMainInfoService _changeMainSummaryInfoService;
        public ChangeMainInfoGetHandler(IChangeMainInfoService changeMainSummaryInfoService)
        {
            _changeMainSummaryInfoService = changeMainSummaryInfoService;
            _changeMainSummaryInfoService.NotNull(nameof(changeMainSummaryInfoService));
        }

        public async Task<Dictionary<string, List<string>>> Handle(string billId, CancellationToken cancellationToken)
        {
            var changeMainSummaryInfo = await _changeMainSummaryInfoService.GetInfo(billId);
            return changeMainSummaryInfo;
        }
    }
}
