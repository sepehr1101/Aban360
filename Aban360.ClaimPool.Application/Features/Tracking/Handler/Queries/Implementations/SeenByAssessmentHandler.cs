using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Tracking.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Implementations
{
    internal sealed class SeenByAssessmentHandler : ISeenByAssessmentHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly ITrackingDetailQueryService _trackingDetailQueryService;
        public SeenByAssessmentHandler(
            ITrackingDetailQueryService trackingDetailQueryService,
            ITrackingQueryService trackingQueryService)
        {
            _trackingDetailQueryService = trackingDetailQueryService;
            _trackingDetailQueryService.NotNull(nameof(trackingDetailQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));
        }

        public async Task<SeenByAssessmentOutputDto> Handle(TrackingDetailInputDto input, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.Get(input.TrackId);
            SeenByAssessmentGetDto seenbyAssessmentDto = new(trackingInfo.TrackNumber, trackingInfo.InsertDateTimeGregorian.ToString("yyyy-MM-dd"));
            SeenByAssessmentOutputDto result = await _trackingDetailQueryService.GetSeenByAssessment(seenbyAssessmentDto);

            return result;
        }
    }
}
