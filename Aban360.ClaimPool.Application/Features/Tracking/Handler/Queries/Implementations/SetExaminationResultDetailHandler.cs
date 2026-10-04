using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations;
using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Implementations
{
    internal sealed class SetExaminationResultDetailHandler : ISetExaminationResultDetailHandler
    {
        private readonly ICommonZoneService _zoneService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IExaminationQueryService _examinationQueryService;
        private static int _currentStatusCode = (int)RequestStatusEnum.SetExaminationResult;
        public SetExaminationResultDetailHandler(
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

        public async Task<SetExaminationResultOutputDto> Handle(Guid id, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.Get(id);
            await _zoneService.IsUserInZone(appUser, trackingInfo.ZoneId);
            if (trackingInfo.StatusId != _currentStatusCode)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }

            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, trackingInfo.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber, true)).FirstOrDefault();
            MoshtrakServiceDto sData = MoshtrakService.GetMoshtrakServiceDto(moshtrakInfo);
            IEnumerable<NumericDictionary> s = MoshtrakService.GetServicesSelectedDto(sData, trackingInfo.ServiceGroupId);
            AssessmentDataOutputDto assessmentInfo = await _examinationQueryService.GetByTrackId(id, true);

            SetExaminationResultOutputDto result = GetOutput(trackingInfo, moshtrakInfo, assessmentInfo, s);
            return result;
        }
        private SetExaminationResultOutputDto GetOutput(TrackingOutputDto trackingInfo, MoshtrakOutputDto moshtrakInfo, AssessmentDataOutputDto assessmentInfo, IEnumerable<NumericDictionary> s)
        {
            return new SetExaminationResultOutputDto()
            {
                BillId = trackingInfo.BillId ?? string.Empty,
                ZoneId = trackingInfo.ZoneId,
                ZoneTitle = trackingInfo.ZoneTitle,
                RegionId = trackingInfo.RegionId,
                RegionTitle = trackingInfo.RegionTitle,

                AssessmentCode = assessmentInfo.AssessmentCode,
                AssessmentName = assessmentInfo.AssessmentName,
                AssessmentMobile = assessmentInfo.AssessmentMobile,
                AssessmentDayJalali = assessmentInfo.AssessmentDateJalali,
                FullName = moshtrakInfo.FullName ?? string.Empty,
                TrackNumber = moshtrakInfo.TrackNumber,
                Address = moshtrakInfo.Address,
                MobileNumber = moshtrakInfo.MobileNumber,
                AssessmentResultTitle = assessmentInfo.ResultTitle ?? string.Empty,
                IsResultSuccess = assessmentInfo.IsResultSuccess,
                HasTrench = (moshtrakInfo.HasEnsheabAb || moshtrakInfo.HasEnsheabFazelab) && assessmentInfo.IsResultSuccess,
                ServiceSelected = s
            };
        }
    }
}
