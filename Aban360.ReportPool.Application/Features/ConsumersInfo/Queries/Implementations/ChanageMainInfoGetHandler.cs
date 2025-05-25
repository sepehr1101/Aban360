using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    internal class ChanageMainInfoGetHandler : IChangeMainInfoGetHandler
    {
        private readonly IChangeMainInfoService _ChangeMainSummaryInfoService;
        public ChanageMainInfoGetHandler(IChangeMainInfoService ChangeMainSummaryInfoService)
        {
            _ChangeMainSummaryInfoService = ChangeMainSummaryInfoService;
            _ChangeMainSummaryInfoService.NotNull(nameof(ChangeMainSummaryInfoService));
        }

        public async Task<IEnumerable<ChangeMainInfoDto>> Handle(string billId, CancellationToken cancellationToken)
        {
            var ChangeMainSummaryInfo = await _ChangeMainSummaryInfoService.GetInfo(billId);
            return ChangeMainSummaryInfo;
        }
    }
}
