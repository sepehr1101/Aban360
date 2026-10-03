using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Implementations
{
    internal sealed class TrackNumberAndDescriptionDetailHandler : ITrackNumberAndDescriptionDetailHandler
    {
        private readonly ICommonZoneService _zoneService;
        private readonly ITrackingQueryService _trackingQueryService;
        public TrackNumberAndDescriptionDetailHandler(
            ICommonZoneService zoneService,
            ITrackingQueryService trackingQueryService)
        {
            _zoneService = zoneService;
            _zoneService.NotNull(nameof(zoneService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));
        }

        public async Task<TrackNumberAndDescriptionOutputDto> Handle(Guid id, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.Get(id);
            await _zoneService.IsUserInZone(appUser, trackingInfo.ZoneId);
            return new TrackNumberAndDescriptionOutputDto(trackingInfo.TrackNumber, trackingInfo.Description ?? string.Empty);
        }
    }
}
