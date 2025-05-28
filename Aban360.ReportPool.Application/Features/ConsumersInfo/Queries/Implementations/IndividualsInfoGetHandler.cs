using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    internal class IndividualsInfoGetHandler : IIndividualsInfoGetHandler
    {
        private readonly IIndividualsInfoService _individualsSummaryInfoService;
        public IndividualsInfoGetHandler(IIndividualsInfoService individualsSummaryInfoService)
        {
            _individualsSummaryInfoService = individualsSummaryInfoService;
            _individualsSummaryInfoService.NotNull(nameof(individualsSummaryInfoService));
        }

        public async Task<IEnumerable<IndividualsInfoDto>> Handle(string billId, CancellationToken cancellationToken)
        {
            var individualsSummaryInfo = await _individualsSummaryInfoService.GetInfo(billId);
            return individualsSummaryInfo;
        }
    }
}
