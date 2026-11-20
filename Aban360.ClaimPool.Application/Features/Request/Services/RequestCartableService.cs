using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Application.Features.Request.Services;
public interface IRequestCartableService
{
    Task<ReportOutput<TrackingKartableHeaderOutputDto, TrackingOutputDto>> ReferredRequestGetAsync(IAppUser appUser, CancellationToken cancellationToken);
    Task<ReportOutput<UnAssessmentHeaderOutputDto, UnAssessmentDataOutputDto>> UnAssessmentGetAllAsync(IAppUser appUser, CancellationToken cancellationToken);
    Task<ReportOutput<UnconfirmedRequestHeaderOutputDto, UnconfirmedRequestDataOutputDto>> UnconfirmedRequestGetByZoneIdAsync(int zoneId, CancellationToken cancellationToken);
}

internal sealed class RequestCartableService : IRequestCartableService
{
    public RequestCartableService(ITrackingQueryService trackingQueryService, ICommonZoneService commonZoneService, IExaminationQueryService examinationQueryService)
    {
        _trackingQueryService = trackingQueryService;
        _trackingQueryService.NotNull(nameof(trackingQueryService));
        _commonZoneService = commonZoneService;
        _commonZoneService.NotNull(nameof(commonZoneService));
        _examinationQueryService = examinationQueryService;
        _examinationQueryService.NotNull(nameof(examinationQueryService));
    }

    private readonly ITrackingQueryService _trackingQueryService;
    private readonly ICommonZoneService _commonZoneService;
    static string _title = "کارتابل درخواست‌های ارجاع داخلی";
    static int _referredStatusId = 160;
    public async Task<ReportOutput<TrackingKartableHeaderOutputDto, TrackingOutputDto>> ReferredRequestGetAsync(IAppUser appUser, CancellationToken cancellationToken)
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

    private readonly IExaminationQueryService _examinationQueryService;
    const string _UnAssessmentGetAlltitle = "نیازمند ارزیابی";
    public async Task<ReportOutput<UnAssessmentHeaderOutputDto, UnAssessmentDataOutputDto>> UnAssessmentGetAllAsync(IAppUser appUser, CancellationToken cancellationToken)
    {
        IEnumerable<int> myZoneIds = await _commonZoneService.GetMyZoneIds(appUser);
        IEnumerable<UnAssessmentDataOutputDto> data = await _examinationQueryService.GetUnAssessment(myZoneIds);
        UnAssessmentHeaderOutputDto header = new(data?.Count() ?? 0, _UnAssessmentGetAlltitle);
        return new ReportOutput<UnAssessmentHeaderOutputDto, UnAssessmentDataOutputDto>(_UnAssessmentGetAlltitle, header, data);
    }

    static string _UnconfirmedRequestGetByZoneIdtitle = "ثبت قطعی نشده";
    public async Task<ReportOutput<UnconfirmedRequestHeaderOutputDto, UnconfirmedRequestDataOutputDto>> UnconfirmedRequestGetByZoneIdAsync(int zoneId, CancellationToken cancellationToken)
    {
        IEnumerable<UnconfirmedRequestDataOutputDto> data = await _trackingQueryService.GetUnconfirmedRequestByZoneId(zoneId);
        UnconfirmedRequestHeaderOutputDto header = new()
        {
            ZoneId = zoneId,
            ZoneTitle = data?.FirstOrDefault()?.ZoneTitle ?? string.Empty,
            RecordCount = data?.Count() ?? 0,
            Title = _UnconfirmedRequestGetByZoneIdtitle,
            ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
            Amount = data?.Sum(x => x.Amount) ?? 0,
        };
        return new ReportOutput<UnconfirmedRequestHeaderOutputDto, UnconfirmedRequestDataOutputDto>(_UnconfirmedRequestGetByZoneIdtitle, header, data);
    }
}
