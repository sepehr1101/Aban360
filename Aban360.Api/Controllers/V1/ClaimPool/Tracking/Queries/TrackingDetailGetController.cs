using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.Common.Categories.ApiResponse;

namespace Aban360.Api.Controllers.V1.ClaimPool.Tracking.Queries
{
    [Route("v1/tracking")]
    public class TrackingDetailGetController : BaseController
    {
        private readonly IRequestIsRegisteredDetailHandler _requestIsRegisteredHandler;
        private readonly IExamineTimeSetDetailHandler _examineTimeSetDetailHandler;
        private readonly ISetExaminationResultDetailHandler _setExaminationResultDetailHandler;
        private readonly ITrackNumberAndDescriptionDetailHandler _trackNumberAndDescriptionDetailHandler;
        private readonly ICalculationConfirmedDetailHandler _calculationConfirmedDetailHandler;
        private readonly ICustomerNumberSpecifiedDetailHandler _customerNumberSpecifiedDetailHandler;
        private readonly IAmountConfirmedDetailHandler _amountConfirmedDetailHandler;
        private readonly ITrackingDetailGetByIdHandler _trackingDetailGetByIdHandler;
        private readonly ISeenByAssessmentHandler _seenByAssessmentHandler;
        public TrackingDetailGetController(
            IRequestIsRegisteredDetailHandler requestIsRegisteredHandler,
            IExamineTimeSetDetailHandler examineTimeSetDetailHandler,
            ISetExaminationResultDetailHandler setExaminationResultDetailHandler,
            ITrackNumberAndDescriptionDetailHandler trackNumberAndDescriptionDetailHandler,
            ICalculationConfirmedDetailHandler calculationConfirmedDetailHandler,
            ICustomerNumberSpecifiedDetailHandler customerNumberSpecifiedDetailHandler,
            IAmountConfirmedDetailHandler amountConfirmedDetailHandler,
            ITrackingDetailGetByIdHandler trackingDetailGetByIdHandler,
            ISeenByAssessmentHandler seenByAssessmentHandler)
        {
            _requestIsRegisteredHandler = requestIsRegisteredHandler;
            _requestIsRegisteredHandler.NotNull(nameof(requestIsRegisteredHandler));

            _examineTimeSetDetailHandler = examineTimeSetDetailHandler;
            _examineTimeSetDetailHandler.NotNull(nameof(examineTimeSetDetailHandler));

            _setExaminationResultDetailHandler = setExaminationResultDetailHandler;
            _setExaminationResultDetailHandler.NotNull(nameof(setExaminationResultDetailHandler));

            _trackNumberAndDescriptionDetailHandler = trackNumberAndDescriptionDetailHandler;
            _trackNumberAndDescriptionDetailHandler.NotNull(nameof(trackNumberAndDescriptionDetailHandler));

            _calculationConfirmedDetailHandler = calculationConfirmedDetailHandler;
            _calculationConfirmedDetailHandler.NotNull(nameof(calculationConfirmedDetailHandler));

            _customerNumberSpecifiedDetailHandler = customerNumberSpecifiedDetailHandler;
            _customerNumberSpecifiedDetailHandler.NotNull(nameof(customerNumberSpecifiedDetailHandler));

            _amountConfirmedDetailHandler = amountConfirmedDetailHandler;
            _amountConfirmedDetailHandler.NotNull(nameof(amountConfirmedDetailHandler));

            _trackingDetailGetByIdHandler = trackingDetailGetByIdHandler;
            _trackingDetailGetByIdHandler.NotNull(nameof(trackingDetailGetByIdHandler));

            _seenByAssessmentHandler = seenByAssessmentHandler;
            _seenByAssessmentHandler.NotNull(nameof(seenByAssessmentHandler));
        }

        [HttpGet]
        [Route("request-registered/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestIsRegisterdOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRequestRegistered(Guid trackId, CancellationToken cancellationToken)
        {
            RequestIsRegisterdOutputDto result = await _requestIsRegisteredHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("assessment-time/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestIsRegisterdOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAssessmentTimeInfo(Guid trackId, CancellationToken cancellationToken)
        {
            ExamineTimeSetOutputDto result = await _examineTimeSetDetailHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("assessment-result/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SetExaminationResultOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAssessmentResultInfo(Guid trackId, CancellationToken cancellationToken)
        {
            SetExaminationResultOutputDto result = await _setExaminationResultDetailHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("description/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TrackNumberAndDescriptionOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDescriptionInfo(Guid trackId, CancellationToken cancellationToken)
        {
            TrackNumberAndDescriptionOutputDto result = await _trackNumberAndDescriptionDetailHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("calculatation-confirmed/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CalculationConfirmedOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCalculationConfirmedInfo(Guid trackId, CancellationToken cancellationToken)
        {
            CalculationConfirmedOutputDto result = await _calculationConfirmedDetailHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("specified-customerNumber/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CalculationConfirmedOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSpecifiedCustomerNumberInfo(Guid trackId, CancellationToken cancellationToken)
        {
            CustomerNumberSpecifiedOutputDto result = await _customerNumberSpecifiedDetailHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("amount-confirmed/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CalculationConfirmedOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAmountConfirmedInfo(Guid trackId, CancellationToken cancellationToken)
        {
            AmountConfirmedOutputDto result = await _amountConfirmedDetailHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("assessment-seen/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CalculationConfirmedOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAssessmentSeenInfo(Guid trackId, CancellationToken cancellationToken)
        {
            SeenByAssessmentOutputDto result = await _seenByAssessmentHandler.Handle(trackId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("display-detail")]
        public async Task<IActionResult> Detail([FromBody] TrackingDetailInputDto input, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingDetailGetByIdHandler.Handle(input.TrackId, cancellationToken);
            TrackingDetailGetDto TrackDetailInput = GetTrackDetail(trackingInfo);
            switch (trackingInfo.StatusId)
            {
                case (int)RequestStatusEnum.RequestIsRegisterd://ثبت درخواست
                    {
                        RequestIsRegisterdOutputDto result = await _requestIsRegisteredHandler.Handle(TrackDetailInput.TrackId, CurrentUser, cancellationToken);
                        return Ok(result);
                    }
                case (int)RequestStatusEnum.ExamineTimeSet://تعیین روز بازدید
                    {
                        ExamineTimeSetOutputDto result = await _examineTimeSetDetailHandler.Handle(TrackDetailInput.TrackId, CurrentUser, cancellationToken);
                        return Ok(result);
                    }
                case (int)RequestStatusEnum.SetExaminationResult://نتیجه ثبت شده
                    {
                        SetExaminationResultOutputDto result = await _setExaminationResultDetailHandler.Handle(TrackDetailInput.TrackId, CurrentUser, cancellationToken);
                        return Ok(result);
                    }
                case (int)RequestStatusEnum.ReCalculateRequired or (int)RequestStatusEnum.SoftDeleted or (int)RequestStatusEnum.Archived:// برگشت به محاسبه,آرشیو شده ,حذف درخواست
                    {
                        TrackNumberAndDescriptionOutputDto result = await _trackNumberAndDescriptionDetailHandler.Handle(TrackDetailInput.TrackId, CurrentUser, cancellationToken);
                        return Ok(result);
                    }
                case (int)RequestStatusEnum.CalculationConfirmd or (int)RequestStatusEnum.SkipSpecifyRadif://تایید محاسبه , تایید محاسبه دارای ردیف
                    {
                        CalculationConfirmedOutputDto result = await _calculationConfirmedDetailHandler.Handle(TrackDetailInput.TrackId, CurrentUser, cancellationToken);
                        return Ok(result);
                    }
                case (int)RequestStatusEnum.RadifSpecified://اختصاص ردیف
                    {
                        CustomerNumberSpecifiedOutputDto result = await _customerNumberSpecifiedDetailHandler.Handle(TrackDetailInput.TrackId, CurrentUser, cancellationToken);
                        return Ok(result);
                    }
                case (int)RequestStatusEnum.AmountIsConfirmed://تایید مبلغ
                    {
                        AmountConfirmedOutputDto result = await _amountConfirmedDetailHandler.Handle(TrackDetailInput.TrackId, CurrentUser, cancellationToken);
                        return Ok(result);
                    }
                case (int)RequestStatusEnum.SeenByExaminer:// مراجعه ارزیاب
                    {
                        SeenByAssessmentOutputDto result = await _seenByAssessmentHandler.Handle(TrackDetailInput.TrackId, CurrentUser, cancellationToken);
                        return Ok(result);
                    }
                default: throw new InvalidTrackNumberException(ExceptionLiterals.InvalidStateId);
            }
        }
        private TrackingDetailGetDto GetTrackDetail(TrackingOutputDto input)
        {
            return new TrackingDetailGetDto(input.ZoneId, input.TrackId, input.TrackNumber);
        }
    }
}
