using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class ReferredRequestGetHandler : IReferredRequestGetHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly ICommonZoneService _commonZoneService;
        static string _title = "کارتابل درخواست‌های ارجاع داخلی";
        static int _referredStatusId = 160;
        public ReferredRequestGetHandler(
            ITrackingQueryService trackingQueryService,
            ICommonZoneService commonZoneService)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<ReportOutput<TrackingKartableHeaderOutputDto, TrackingOutputDto>> Handle(IAppUser appUser, CancellationToken cancellationToken)
        {
            IEnumerable<int> myZoneIds = await _commonZoneService.GetMyZoneIds(appUser);
            IEnumerable<TrackingOutputDto> data = await _trackingQueryService.GetByStatus(new TrackingByStatusInputDto(myZoneIds, _referredStatusId));
            TrackingKartableHeaderOutputDto header = new()
            {
                ZoneCount = data?.DistinctBy(d => d.ZoneId)?.Count() ?? 0,
                RequestCount = data?.Count() ?? 0,
                CurrentDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = _title,
            };

            return new ReportOutput<TrackingKartableHeaderOutputDto, TrackingOutputDto>(_title, header, data);
        }
    }
}
