using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class InstallmentDisplayHandler : IInstallmentDisplayHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IGhestQueryService _ghestQueryService;
        static string _title = "تقسیط";
        public InstallmentDisplayHandler(
            ITrackingQueryService trackingQueryService,
            IGhestQueryService ghestQueryService)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _ghestQueryService = ghestQueryService;
            _ghestQueryService.NotNull(nameof(ghestQueryService));
        }

        public async Task<ReportOutput<InstallmentRequestHeaderOutputDto, InstallmentRequestDataOutputDto>> Handle(int trackNumber, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(trackNumber);
            IEnumerable<InstallmentRequestDataOutputDto> karts = await _ghestQueryService.Get(trackingInfo.StringTrackNumber, trackingInfo.ZoneId);

            InstallmentRequestHeaderOutputDto header = new(karts?.Sum(x => x.Amount) ?? 0, karts?.Count() ?? 0);
            ReportOutput<InstallmentRequestHeaderOutputDto, InstallmentRequestDataOutputDto> result = new(_title, header, karts);

            return result;
        }
    }
}
