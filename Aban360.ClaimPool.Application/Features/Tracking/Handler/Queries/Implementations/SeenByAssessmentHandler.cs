using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;

namespace Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Implementations
{
    internal sealed class SeenByAssessmentHandler : ISeenByAssessmentHandler
    {
        private readonly ICommonZoneService _zoneService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IExaminationQueryService _examinationQueryService;
        private static int _currentStatusCode = (int)RequestStatusEnum.SeenByExaminer;
        public SeenByAssessmentHandler(
            ICommonZoneService zoneService,
            IMoshtrakQueryService moshtrakQueryService,
            ITrackingQueryService trackingQueryService,
            IExaminationQueryService examinationQueryService)
        {
            _zoneService = zoneService;
            _zoneService.NotNull(nameof(zoneService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _examinationQueryService = examinationQueryService;
            _examinationQueryService.NotNull(nameof(examinationQueryService));
        }

        public async Task<SeenByAssessmentOutputDto> Handle(Guid id, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.Get(id);
            await _zoneService.IsUserInZone(appUser, trackingInfo.ZoneId);
            if (trackingInfo.StatusId != _currentStatusCode)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, trackingInfo.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber, true)).FirstOrDefault();
            AssessmentDataOutputDto assessmentInfo = await _examinationQueryService.Get(trackingInfo.TrackNumber, trackingInfo.InsertDateTimeGregorian, true);

            SeenByAssessmentOutputDto result = GetOutput(moshtrakInfo, assessmentInfo);
            return result;
        }
        private SeenByAssessmentOutputDto GetOutput(MoshtrakOutputDto moshtrakInfo, AssessmentDataOutputDto assessmentInfo)
        {
            return new SeenByAssessmentOutputDto()
            {
                TrackNumber = assessmentInfo.TrackNumber,
                AssessmentTrackId = assessmentInfo.TrackIdResult ?? Guid.Empty,
                BillId = assessmentInfo.BillId,
                AssessmentCode = assessmentInfo.AssessmentCode,
                AssessmentName = assessmentInfo.AssessmentName,
                AssessmentMobile = assessmentInfo.AssessmentMobile,
                AssessmentDayJalali = assessmentInfo.AssessmentDateJalali,
                ZoneId = assessmentInfo.ZoneId,
                ZoneTitle = assessmentInfo.ZoneTitle,
                RegionId = moshtrakInfo.RegionId,
                RegionTitle = moshtrakInfo.RegionTitle,
                ResultId = assessmentInfo.ResultId ?? 0,
                ResultTitle = assessmentInfo.ResultTitle,
                ResultDescription = assessmentInfo.Description,
                TrackIdResult = assessmentInfo.TrackIdResult,
                SetResultDateTime = assessmentInfo.SetResultDateTime,
                X1 = assessmentInfo.X1,
                Y1 = assessmentInfo.Y1,
                X2 = assessmentInfo.X2,
                Y2 = assessmentInfo.Y2,
            };
        }
    }
}
