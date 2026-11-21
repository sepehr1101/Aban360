using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Infrastructure.Features.Geo;

namespace Aban360.ClaimPool.Application.Features.Request.Services;
public interface IAssessmentLookupsService
{
    Task<AssessmentDataOutputDto> AssessmentByTrackIdGetAsync(Guid id, CancellationToken cancellationToken);
    Task<ReportOutput<AssessmentDuplicatHeaderOutputDto, AssessmentDuplicateDetailDataOutputDto>> AssessmentDuplicateDetailAsync(AssessmentDuplicateInputDto inputDto, CancellationToken cancellationToken);
    IEnumerable<NumericDictionary> AssessmentDuplicateParametersGetAsync(CancellationToken cancellationToken);
    Task<AssessmentLocationsGetDto> AssessmentLocatoinsGetAsync(Guid trackId, CancellationToken cancellationToken);
}

internal sealed class AssessmentLookupsService : IAssessmentLookupsService
{
    private readonly IExaminationQueryService _examinationQueryService;
    private readonly ITrackingQueryService _trackingQueryService;
    private static string _title = ReportLiterals.AssessmentDuplicate;
    private readonly IGisService _gisService;
    public AssessmentLookupsService(IExaminationQueryService examinationQueryService, ITrackingQueryService trackingQueryService, IGisService gisService)
    {
        _examinationQueryService = examinationQueryService;
        _examinationQueryService.NotNull(nameof(examinationQueryService));
        _trackingQueryService = trackingQueryService;
        _trackingQueryService.NotNull(nameof(trackingQueryService));
        _gisService = gisService;
        _gisService.NotNull(nameof(gisService));
    }

    public async Task<AssessmentDataOutputDto> AssessmentByTrackIdGetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _examinationQueryService.GetByTrackId(id, true);
    }

    public async Task<ReportOutput<AssessmentDuplicatHeaderOutputDto, AssessmentDuplicateDetailDataOutputDto>> AssessmentDuplicateDetailAsync(AssessmentDuplicateInputDto inputDto, CancellationToken cancellationToken)
    {
        IEnumerable<AssessmentDuplicateDetailDataOutputDto> dataWithoutColour = await _trackingQueryService.Get(inputDto);
        IEnumerable<AssessmentDuplicateDetailDataOutputDto> data = AssessmentDuplicateDetailSetColour(dataWithoutColour);
        AssessmentDuplicatHeaderOutputDto header = new()
        {
            FromDateJalali = inputDto.FromDateJalali,
            ToDateJalali = inputDto.ToDateJalali,
            RecordCount = dataWithoutColour?.Count() ?? 0,
            CustomerCount = dataWithoutColour?.DistinctBy(x => x.BillId)?.Count() ?? 0,
            Title = _title,
        };
        return new ReportOutput<AssessmentDuplicatHeaderOutputDto, AssessmentDuplicateDetailDataOutputDto>(_title, header, data);
    }

    private IEnumerable<AssessmentDuplicateDetailDataOutputDto> AssessmentDuplicateDetailSetColour(IEnumerable<AssessmentDuplicateDetailDataOutputDto> data)
    {
        string? tempVariable = null;
        bool isColour = false;
        foreach (var item in data)
        {
            if (tempVariable is not null && item.DuplicateItem != tempVariable)
            {
                isColour = !isColour;
            }

            item.IsColour = isColour;
            tempVariable = item.DuplicateItem;
        }

        return data;
    }

    public IEnumerable<NumericDictionary> AssessmentDuplicateParametersGetAsync(CancellationToken cancellationToken)
    {
        return new List<NumericDictionary>()
        {
            new NumericDictionary(1, "شماره موبایل"),
            //new NumericDictionary(2,"کد پستی"),
            //new NumericDictionary(3,"کد ملی"),
            new NumericDictionary(4, "شناسه قبض"),
            new NumericDictionary(5, "شناسه قبض همسایه"),
        };
    }

    public async Task<AssessmentLocationsGetDto> AssessmentLocatoinsGetAsync(Guid trackId, CancellationToken cancellationToken)
    {
        AssessmentDataOutputDto examinationInfo = await _examinationQueryService.GetByTrackId(trackId, true);
        TrackingOutputDto trackingInfo = await _trackingQueryService.Get(trackId);
        AssessmentLocationsGetDto result = new()
        {
            XMap = examinationInfo.X1 ?? string.Empty,
            YMap = examinationInfo.Y1 ?? string.Empty,
            XGps = examinationInfo.X2 ?? string.Empty,
            YGps = examinationInfo.Y2 ?? string.Empty,
            Accuracy = examinationInfo.Accuracy ?? string.Empty,
        };
        if (!string.IsNullOrWhiteSpace(trackingInfo.BillId))
        {
            CustomerLocationDto locationInfo = await _gisService.GetCustomerLocation(new CustomerLocationInputDto(trackingInfo.BillId));
            result.XGis = locationInfo.X;
            result.YGis = locationInfo.Y;
        }

        return result;
    }
}
