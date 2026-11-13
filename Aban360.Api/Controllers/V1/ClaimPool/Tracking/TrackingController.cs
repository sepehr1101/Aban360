using Aban360.ClaimPool.Application.Features.Sms.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.NotificationPool.Application.Features.Sms;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.ClaimPool.Tracking
{
    public class TrackingController : BaseController
    {
        private readonly ISmsByTrackIdGetHandler _c0_smsByTrackIdHandler;
        private readonly ISmsByQueueIdGetHandler _c0_smsByQueueIdHandler;
        private readonly ISmsOldHandler _c0_smsOldHandler;
        private readonly IBackgroundJobClient _c0_backgroundJobClient;
        private readonly IRequestIsRegisteredDetailHandler _c1_requestIsRegisteredHandler;
        private readonly IExamineTimeSetDetailHandler _c1_examineTimeSetDetailHandler;
        private readonly ISetExaminationResultDetailHandler _c1_setExaminationResultDetailHandler;
        private readonly ITrackNumberAndDescriptionDetailHandler _c1_trackNumberAndDescriptionDetailHandler;
        private readonly ICalculationConfirmedDetailHandler _c1_calculationConfirmedDetailHandler;
        private readonly ICustomerNumberSpecifiedDetailHandler _c1_customerNumberSpecifiedDetailHandler;
        private readonly IAmountConfirmedDetailHandler _c1_amountConfirmedDetailHandler;
        private readonly ITrackingDetailGetByIdHandler _c1_trackingDetailGetByIdHandler;
        private readonly ISeenByAssessmentHandler _c1_seenByAssessmentHandler;
        private readonly ITrackingFlowGetHandler _c2_trackingFlowHandler;
        public TrackingController(ISmsByTrackIdGetHandler c0_smsByTrackIdHandler, ISmsByQueueIdGetHandler c0_smsByQueueIdHandler, ISmsOldHandler c0_smsOldHandler, IBackgroundJobClient c0_backgroundJobClient, IRequestIsRegisteredDetailHandler c1_requestIsRegisteredHandler, IExamineTimeSetDetailHandler c1_examineTimeSetDetailHandler, ISetExaminationResultDetailHandler c1_setExaminationResultDetailHandler, ITrackNumberAndDescriptionDetailHandler c1_trackNumberAndDescriptionDetailHandler, ICalculationConfirmedDetailHandler c1_calculationConfirmedDetailHandler, ICustomerNumberSpecifiedDetailHandler c1_customerNumberSpecifiedDetailHandler, IAmountConfirmedDetailHandler c1_amountConfirmedDetailHandler, ITrackingDetailGetByIdHandler c1_trackingDetailGetByIdHandler, ISeenByAssessmentHandler c1_seenByAssessmentHandler, ITrackingFlowGetHandler c2_trackingFlowHandler)
        {
            _c0_smsByTrackIdHandler = c0_smsByTrackIdHandler;
            _c0_smsByTrackIdHandler.NotNull(nameof(c0_smsByTrackIdHandler));
            _c0_smsByQueueIdHandler = c0_smsByQueueIdHandler;
            _c0_smsByQueueIdHandler.NotNull(nameof(c0_smsByQueueIdHandler));
            _c0_smsOldHandler = c0_smsOldHandler;
            _c0_smsOldHandler.NotNull(nameof(c0_smsOldHandler));
            _c0_backgroundJobClient = c0_backgroundJobClient;
            _c0_backgroundJobClient.NotNull(nameof(c0_backgroundJobClient));
            _c1_requestIsRegisteredHandler = c1_requestIsRegisteredHandler;
            _c1_requestIsRegisteredHandler.NotNull(nameof(c1_requestIsRegisteredHandler));
            _c1_examineTimeSetDetailHandler = c1_examineTimeSetDetailHandler;
            _c1_examineTimeSetDetailHandler.NotNull(nameof(c1_examineTimeSetDetailHandler));
            _c1_setExaminationResultDetailHandler = c1_setExaminationResultDetailHandler;
            _c1_setExaminationResultDetailHandler.NotNull(nameof(c1_setExaminationResultDetailHandler));
            _c1_trackNumberAndDescriptionDetailHandler = c1_trackNumberAndDescriptionDetailHandler;
            _c1_trackNumberAndDescriptionDetailHandler.NotNull(nameof(c1_trackNumberAndDescriptionDetailHandler));
            _c1_calculationConfirmedDetailHandler = c1_calculationConfirmedDetailHandler;
            _c1_calculationConfirmedDetailHandler.NotNull(nameof(c1_calculationConfirmedDetailHandler));
            _c1_customerNumberSpecifiedDetailHandler = c1_customerNumberSpecifiedDetailHandler;
            _c1_customerNumberSpecifiedDetailHandler.NotNull(nameof(c1_customerNumberSpecifiedDetailHandler));
            _c1_amountConfirmedDetailHandler = c1_amountConfirmedDetailHandler;
            _c1_amountConfirmedDetailHandler.NotNull(nameof(c1_amountConfirmedDetailHandler));
            _c1_trackingDetailGetByIdHandler = c1_trackingDetailGetByIdHandler;
            _c1_trackingDetailGetByIdHandler.NotNull(nameof(c1_trackingDetailGetByIdHandler));
            _c1_seenByAssessmentHandler = c1_seenByAssessmentHandler;
            _c1_seenByAssessmentHandler.NotNull(nameof(c1_seenByAssessmentHandler));
            _c2_trackingFlowHandler = c2_trackingFlowHandler;
            _c2_trackingFlowHandler.NotNull(nameof(c2_trackingFlowHandler));
        }

        [HttpPost]
        [Route("~/v1/tracking/display-sms")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TrackingSmsHeaderOutputDto, TrackingSmsDataOutputDto>>), StatusCodes.Status200OK)]
        [ActionName("DisplayFlow")]
        public async Task<IActionResult> DisplayFlowFromSmsByTrackIdGet0([FromBody] GuidInput input, CancellationToken cancellationToken)
        {
            ReportOutput<TrackingSmsHeaderOutputDto, TrackingSmsDataOutputDto> result = await _c0_smsByTrackIdHandler.Handle(input.Input, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("~/v1/tracking/resend-sms")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TrackingSmsHeaderOutputDto, TrackingSmsDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ResendSms([FromBody] GuidInput input, CancellationToken cancellationToken)
        {
            TrackingSmsDataOutputDto result = await _c0_smsByQueueIdHandler.Handle(input.Input, cancellationToken);
            result.TrackId = result.TrackId is null ? Guid.Empty : result.TrackId;
            _c0_backgroundJobClient.Enqueue(() => _c0_smsOldHandler.SendNew(result.Receiver, result.Message, result.TrackId.Value));
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/tracking/request-registered/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestIsRegisterdOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRequestRegistered(Guid trackId, CancellationToken cancellationToken)
        {
            RequestIsRegisterdOutputDto result = await _c1_requestIsRegisteredHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/tracking/assessment-time/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestIsRegisterdOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAssessmentTimeInfo(Guid trackId, CancellationToken cancellationToken)
        {
            ExamineTimeSetOutputDto result = await _c1_examineTimeSetDetailHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/tracking/assessment-result/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SetExaminationResultOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAssessmentResultInfo(Guid trackId, CancellationToken cancellationToken)
        {
            SetExaminationResultOutputDto result = await _c1_setExaminationResultDetailHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/tracking/description/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TrackNumberAndDescriptionOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDescriptionInfo(Guid trackId, CancellationToken cancellationToken)
        {
            TrackNumberAndDescriptionOutputDto result = await _c1_trackNumberAndDescriptionDetailHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/tracking/calculatation-confirmed/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CalculationConfirmedOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCalculationConfirmedInfo(Guid trackId, CancellationToken cancellationToken)
        {
            CalculationConfirmedOutputDto result = await _c1_calculationConfirmedDetailHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/tracking/specified-customerNumber/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CalculationConfirmedOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSpecifiedCustomerNumberInfo(Guid trackId, CancellationToken cancellationToken)
        {
            CustomerNumberSpecifiedOutputDto result = await _c1_customerNumberSpecifiedDetailHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/tracking/amount-confirmed/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CalculationConfirmedOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAmountConfirmedInfo(Guid trackId, CancellationToken cancellationToken)
        {
            AmountConfirmedOutputDto result = await _c1_amountConfirmedDetailHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/tracking/assessment-seen/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CalculationConfirmedOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAssessmentSeenInfo(Guid trackId, CancellationToken cancellationToken)
        {
            SeenByAssessmentOutputDto result = await _c1_seenByAssessmentHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("~/v1/tracking/display-detail")]
        public async Task<IActionResult> Detail([FromBody] TrackingDetailInputDto input, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _c1_trackingDetailGetByIdHandler.Handle(input.TrackId, cancellationToken);
            TrackingDetailGetDto TrackDetailInput = GetTrackDetail(trackingInfo);
            switch (trackingInfo.StatusId)
            {
                case (int)RequestStatusEnum.RequestIsRegisterd: //ثبت درخواست
                {
                    RequestIsRegisterdOutputDto result = await _c1_requestIsRegisteredHandler.Handle(TrackDetailInput.TrackId, CurrentUser, cancellationToken);
                    return Ok(result);
                }

                case (int)RequestStatusEnum.ExamineTimeSet: //تعیین روز بازدید
                {
                    ExamineTimeSetOutputDto result = await _c1_examineTimeSetDetailHandler.Handle(TrackDetailInput.TrackId, CurrentUser, cancellationToken);
                    return Ok(result);
                }

                case (int)RequestStatusEnum.SetExaminationResult: //نتیجه ثبت شده
                {
                    SetExaminationResultOutputDto result = await _c1_setExaminationResultDetailHandler.Handle(TrackDetailInput.TrackId, CurrentUser, cancellationToken);
                    return Ok(result);
                }

                case (int)RequestStatusEnum.ReCalculateRequired or (int)RequestStatusEnum.SoftDeleted or (int)RequestStatusEnum.Archived: // برگشت به محاسبه,آرشیو شده ,حذف درخواست
                {
                    TrackNumberAndDescriptionOutputDto result = await _c1_trackNumberAndDescriptionDetailHandler.Handle(TrackDetailInput.TrackId, CurrentUser, cancellationToken);
                    return Ok(result);
                }

                case (int)RequestStatusEnum.CalculationConfirmd or (int)RequestStatusEnum.SkipSpecifyRadif: //تایید محاسبه , تایید محاسبه دارای ردیف
                {
                    CalculationConfirmedOutputDto result = await _c1_calculationConfirmedDetailHandler.Handle(TrackDetailInput.TrackId, CurrentUser, cancellationToken);
                    return Ok(result);
                }

                case (int)RequestStatusEnum.RadifSpecified: //اختصاص ردیف
                {
                    CustomerNumberSpecifiedOutputDto result = await _c1_customerNumberSpecifiedDetailHandler.Handle(TrackDetailInput.TrackId, CurrentUser, cancellationToken);
                    return Ok(result);
                }

                case (int)RequestStatusEnum.AmountIsConfirmed: //تایید مبلغ
                {
                    AmountConfirmedOutputDto result = await _c1_amountConfirmedDetailHandler.Handle(TrackDetailInput.TrackId, CurrentUser, cancellationToken);
                    return Ok(result);
                }

                case (int)RequestStatusEnum.SeenByExaminer: // مراجعه ارزیاب
                {
                    SeenByAssessmentOutputDto result = await _c1_seenByAssessmentHandler.Handle(TrackDetailInput.TrackId, CurrentUser, cancellationToken);
                    return Ok(result);
                }

                default:
                    throw new InvalidTrackNumberException(ExceptionLiterals.InvalidStateId);
            }
        }

        private TrackingDetailGetDto GetTrackDetail(TrackingOutputDto input)
        {
            return new TrackingDetailGetDto(input.ZoneId, input.TrackId, input.TrackNumber);
        }

        [HttpPost]
        [Route("~/v1/tracking/display-flow")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TrackingDisplayFlowHeaderOutputDto, TrackingDisplayFlowDateOutputDto>>), StatusCodes.Status200OK)]
        [ActionName("DisplayFlow")]
        public async Task<IActionResult> DisplayFlowFromTrackingDisplayFlow2([FromBody] SearchByIdInput input, CancellationToken cancellationToken)
        {
            ReportOutput<TrackingDisplayFlowHeaderOutputDto, TrackingDisplayFlowDateOutputDto> result = await _c2_trackingFlowHandler.Handle(input.Id, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
