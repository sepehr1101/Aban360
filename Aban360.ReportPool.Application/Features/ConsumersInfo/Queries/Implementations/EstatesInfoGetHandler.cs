using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    internal class EstatesInfoGetHandler : IEstatesInfoGetHandler
    {
        private readonly IEstatesInfoService _estatesSummaryInfoService;
        public EstatesInfoGetHandler(IEstatesInfoService estatesSummaryInfoService)
        {
            _estatesSummaryInfoService = estatesSummaryInfoService;
            _estatesSummaryInfoService.NotNull(nameof(estatesSummaryInfoService));
        }

        public async Task<IEnumerable<EstatesInfoDto>> Handle(string billId, CancellationToken cancellationToken)
        {
            var estateSummaryInfo = await _estatesSummaryInfoService.GetInfo(billId);
            return estateSummaryInfo;
        }
    }
}
