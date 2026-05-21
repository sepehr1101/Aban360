using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Constants.Enums;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class KartableRequestGetAllHandler : IKartableRequestGetAllHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IUserClaimQueryService _userClaimQueryService;
        static string _title = "کارتابل درخواست‌ها";
        public KartableRequestGetAllHandler(
            ITrackingQueryService trackingQueryService,
            ICommonZoneService commonZoneService,
            IUserClaimQueryService userClaimQueryService)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _userClaimQueryService = userClaimQueryService;
            _userClaimQueryService.NotNull(nameof(userClaimQueryService));
        }

        public async Task<ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto>> Handle(IAppUser currentUser, CancellationToken cancellationToken)
        {
            IEnumerable<int> zoneIds = await _commonZoneService.GetMyZoneIds(currentUser);
            ICollection<UserClaim> userClaims = await _userClaimQueryService.Get(currentUser.UserId, ClaimType.RequestKartable);
            ICollection<int> userAccesskartables = userClaims.Where(u => u.ValidTo is null).Select(u => int.Parse(u.ClaimValue)).ToList();
            IEnumerable<TrackingKartableDataOutputDto> data = await _trackingQueryService.GetAllOpenRequest(zoneIds, userAccesskartables);
            TrackingKartableHeaderOutputDto header = new()
            {
                ZoneCount = data?.DistinctBy(d => d.ZoneId)?.Count() ?? 0,
                RequestCount = data?.Count() ?? 0,
                CurrentDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = _title,
            };

            return new ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto>(_title, header, data);
        }
    }
}
