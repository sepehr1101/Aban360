using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Infrastructure.Features.Geo;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class AssessmentLocatoinsGetHandler : IAssessmentLocatoinsGetHandler
    {
        private readonly IExaminationQueryService _examinationQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IGisService _gisService;
        public AssessmentLocatoinsGetHandler(
            IExaminationQueryService examinationQueryService,
            ITrackingQueryService trackingQueryService,
            IGisService gisService)
        {
            _examinationQueryService = examinationQueryService;
            _examinationQueryService.NotNull(nameof(examinationQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _gisService = gisService;
            _gisService.NotNull(nameof(gisService));
        }

        public async Task<AssessmentLocationsGetDto> Handle(Guid trackId, CancellationToken cancellationToken)
        {
            AssessmentDataOutputDto examinationInfo = await _examinationQueryService.GetByTrackId(trackId);
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
}
