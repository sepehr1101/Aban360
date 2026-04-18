using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Features.Tracking.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;

namespace Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Implementations
{
    internal sealed class TrackingFlowGetHandler : ITrackingFlowGetHandler
    {
        private readonly ITrackingKartableQueryService _trackingQueryService;
        private readonly ICommonZoneService _commontZoneService;
        public TrackingFlowGetHandler(
            ITrackingKartableQueryService trackingQueryService,
            ICommonZoneService commontZoneService)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _commontZoneService = commontZoneService;
            _commontZoneService.NotNull(nameof(commontZoneService));
        }

        public async Task<ReportOutput<TrackingDisplayFlowHeaderOutputDto, TrackingDisplayFlowDateOutputDto>> Handle(int trackNumber,IAppUser appUser, CancellationToken cancellationToken)
        {
            ReportOutput<TrackingDisplayFlowHeaderOutputDto, TrackingDisplayFlowDateOutputDto> result= await _trackingQueryService.Get(trackNumber);
            AllowedZoneValidation zoneValidation = new(_commontZoneService);
            await zoneValidation.Validation(result.ReportHeader.ZoneId, appUser);

            return result;
        }
    }
}
