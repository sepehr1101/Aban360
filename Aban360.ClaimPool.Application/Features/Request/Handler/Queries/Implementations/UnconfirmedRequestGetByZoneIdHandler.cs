using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class UnconfirmedRequestGetByZoneIdHandler : IUnconfirmedRequestGetByZoneIdHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        static string _title = "ثبت قطعی نشده";
        public UnconfirmedRequestGetByZoneIdHandler(ITrackingQueryService trackingQueryService)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));
        }

        public async Task<ReportOutput<UnconfirmedRequestHeaderOutputDto, UnconfirmedRequestDataOutputDto>> Handle(int zoneId, CancellationToken cancellationToken)
        {
            IEnumerable<UnconfirmedRequestDataOutputDto> data = await _trackingQueryService.GetUnconfirmedRequestByZoneId(zoneId);
            UnconfirmedRequestHeaderOutputDto header = new()
            {
                ZoneId = zoneId,
                ZoneTitle = data?.FirstOrDefault()?.ZoneTitle ?? string.Empty,
                RecordCount = data?.Count() ?? 0,
                Title = _title,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Amount = data?.Sum(x => x.Amount) ?? 0,
            };
            return new ReportOutput<UnconfirmedRequestHeaderOutputDto, UnconfirmedRequestDataOutputDto>(_title, header, data);
        }
    }
}
