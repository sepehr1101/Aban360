using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Features.Tracking.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Implementations
{
    internal sealed class TrackingFlowGetHandler : ITrackingFlowGetHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        public TrackingFlowGetHandler(ITrackingQueryService trackingQueryService)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));
        }

        public async Task<ReportOutput<TrackingDisplayFlowHeaderOutputDto, TrackingDisplayFlowDateOutputDto>> Handle(int trackNumber, CancellationToken cancellationToken)
        {
            return await _trackingQueryService.Get(trackNumber);
        }
    }
}
