using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Reading.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;

namespace Aban360.MeterPool.Application.Features.Reading.Handlers.Queries.Implementataions
{
    internal sealed class ReadingTrackingFlowGetHandler : IReadingTrackingFlowGetHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly ICommonZoneService _zoneService;
        private static string _title = ReportLiterals.ReadingTreackingFlow;
        public ReadingTrackingFlowGetHandler(
            ITrackingQueryService trackingQueryService,
            ICommonZoneService zoneService)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _zoneService = zoneService;
            _zoneService.NotNull(nameof(zoneService));
        }

        public async Task<ReportOutput<TrackingHeaderDto, TrackingDataDto>> Handle(int trackNumber, IAppUser appUser, CancellationToken cancellationToken)
        {
            IEnumerable<TrackingDataDto> data = await _trackingQueryService.GetFlow(trackNumber);
            if (data.Any())
            {
                await _zoneService.IsUserInZone(appUser.UserId, data.FirstOrDefault().ZoneId);
            }
            TrackingHeaderDto header = new()
            {
                RecordCount = data?.Count() ?? 0,
                Title = _title
            };

            ReportOutput<TrackingHeaderDto, TrackingDataDto> result = new(_title, header, data);
            return result;
        }
    }
}
