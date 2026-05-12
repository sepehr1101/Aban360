using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Queries
{
    [Route("v1/request")]
    public class RequestKartableDetailsController : BaseController
    {
        private readonly ITrackingDetailGetByIdHandler _trackingDetailGetByIdHandler;
        private readonly ISetExaminationResultDetailHandler _setExaminationResultDetailHandler;
        private readonly IGeneralInformationHandler _generalInformationHandler;
        public RequestKartableDetailsController(
            ITrackingDetailGetByIdHandler trackingDetailGetByIdHandler,
            ISetExaminationResultDetailHandler setExaminationResultDetailHandler,
            IGeneralInformationHandler generalInformationHandler)
        {
            _trackingDetailGetByIdHandler = trackingDetailGetByIdHandler;
            _trackingDetailGetByIdHandler.NotNull(nameof(trackingDetailGetByIdHandler));

            _setExaminationResultDetailHandler = setExaminationResultDetailHandler;
            _setExaminationResultDetailHandler.NotNull(nameof(setExaminationResultDetailHandler));

            _generalInformationHandler = generalInformationHandler;
            _generalInformationHandler.NotNull(nameof(generalInformationHandler));
        }

        [HttpPost]
        [Route("kartable-details/{trackId}")]
        public async Task<IActionResult> DisplayDetails(Guid trackId, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingDetailGetByIdHandler.Handle(trackId, cancellationToken);
            TrackingDetailGetDto TrackDetailInput = GetTrackDetail(trackingInfo);
            GeneralRequestDataOutputDto generalInfo = await _generalInformationHandler.Handle(trackId, cancellationToken);
            switch (trackingInfo.StatusId)
            {
                case 110://نتیجه ثبت شده
                    {
                        return await AssessmentResult(TrackDetailInput, generalInfo, cancellationToken);
                    }
                case 65://برگشت به محاسبه
                    {
                        return await ReCalculation(TrackDetailInput, generalInfo, cancellationToken);
                    }
                case 15://ارزیابی مجدد
                    {
                        return Ok(generalInfo);
                    }
                case 0://ثبت درخواست
                    {
                        return Ok(generalInfo);
                    }
                default: throw new InvalidTrackNumberException(ExceptionLiterals.InvalidStateId);
            }
        }
        private TrackingDetailGetDto GetTrackDetail(TrackingOutputDto input)
        {
            return new TrackingDetailGetDto(input.ZoneId, input.TrackId, input.TrackNumber.ToString());
        }
        private async Task<IActionResult> AssessmentResult(TrackingDetailGetDto TrackDetailInput, GeneralRequestDataOutputDto generalInfo, CancellationToken cancellationToken)
        {
            SetExaminationResultOutputDto assessmentResult = await _setExaminationResultDetailHandler.Handle(TrackDetailInput, cancellationToken);
            if (assessmentResult.IsResultSuccess)
            {
                return Ok(GetAssessmentSuccess(assessmentResult, generalInfo));
            }
            else
            {
                return Ok(assessmentResult);
            }
        }
        private async Task<IActionResult> ReCalculation(TrackingDetailGetDto TrackDetailInput, GeneralRequestDataOutputDto generalInfo, CancellationToken cancellationToken)
        {
            SetExaminationResultOutputDto assessmentResult = await _setExaminationResultDetailHandler.Handle(TrackDetailInput, cancellationToken);
            return Ok(GetAssessmentSuccess(assessmentResult, generalInfo));
        }
        private AssessmentSuccessResultDataOutputDto GetAssessmentSuccess(SetExaminationResultOutputDto assessmentResult, GeneralRequestDataOutputDto generalInfo)
        {
            return new AssessmentSuccessResultDataOutputDto()
            {
                TrackNumber = generalInfo.TrackNumber,
                BillId = generalInfo.BillId,
                NeighbourBillId = generalInfo.NeighbourBillId,
                ZoneTitle = generalInfo.ZoneTitle,
                ZoneId = generalInfo.ZoneId,
                RegionTitle = generalInfo.RegionTitle,
                RegionId = generalInfo.RegionId,
                FirstName = generalInfo.FirstName,
                Surname = generalInfo.Surname,
                FatherName = generalInfo.FatherName,
                NationalCode = generalInfo.NationalCode,
                MobileNumber = generalInfo.MobileNumber,
                PhoneNumber = generalInfo.PhoneNumber,
                Caller = generalInfo.Caller,
                NotificationMobile = generalInfo.NotificationMobile,
                Address = generalInfo.Address,
                AssessmentCode = assessmentResult.AssessmentCode,
                AssessmentName = assessmentResult.AssessmentName,
                AssessmentMobile = assessmentResult.AssessmentMobile,
                AssessmentDayJalali = assessmentResult.AssessmentDayJalali,
                AssessmentResultTitle = assessmentResult.AssessmentResultTitle,
                HasTrench = assessmentResult.HasTrench,
                CompanyServices = generalInfo.CompanyServices,
            };
        }
    }
}