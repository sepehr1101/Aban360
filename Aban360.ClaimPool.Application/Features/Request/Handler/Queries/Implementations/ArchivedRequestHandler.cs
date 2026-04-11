using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class ArchivedRequestHandler : IArchivedRequestHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        static string _title = "کارتابل درخواست‌های آرشیو شده";
        public ArchivedRequestHandler(ITrackingQueryService trackingQueryService)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));
        }

        public async Task<ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<TrackingKartableDataOutputDto> data = await _trackingQueryService.GetAllArchivedRequest();
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
