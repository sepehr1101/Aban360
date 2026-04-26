using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.NotificationPool.Application.Features.Sms;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/assessment")]
    public class AssessmentController : BaseController
    {
        private readonly ISetAssessmentResultHandler _setAssessmentResultHandler;
        private readonly ISetAssessmentTimeHandler _setAssessmentTimeHandler;
        private readonly ISetLightAssessmentResultHandler _setLightAssessmentResultHandler;
        private readonly IReAssessmentRequestHandler _reAssessmentRequestHandler;
        private readonly ISmsOldHandler _smsOldHandler;
        private readonly IBackgroundJobClient _backgroudJobClient;
        public AssessmentController(
            ISetAssessmentResultHandler setAssessmentResultHandler,
            ISetAssessmentTimeHandler setAssessmentTimeHandler,
            ISetLightAssessmentResultHandler setLightAssessmentResultHandler,
            IReAssessmentRequestHandler reAssessmentRequestHandler,
            ISmsOldHandler smsOldHandler,
            IBackgroundJobClient backgroudJobClient)
        {
            _setAssessmentResultHandler = setAssessmentResultHandler;
            _setAssessmentResultHandler.NotNull(nameof(setAssessmentResultHandler));

            _setAssessmentTimeHandler = setAssessmentTimeHandler;
            _setAssessmentTimeHandler.NotNull(nameof(setAssessmentTimeHandler));

            _setLightAssessmentResultHandler = setLightAssessmentResultHandler;
            _setLightAssessmentResultHandler.NotNull(nameof(setLightAssessmentResultHandler));

            _reAssessmentRequestHandler = reAssessmentRequestHandler;
            _reAssessmentRequestHandler.NotNull(nameof(reAssessmentRequestHandler));

            _smsOldHandler = smsOldHandler;
            _smsOldHandler.NotNull(nameof(smsOldHandler));

            _backgroudJobClient = backgroudJobClient;
            _backgroudJobClient.NotNull(nameof(backgroudJobClient));
        }

        [HttpPost]
        [Route("result")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AssessmentResultInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetReult([FromBody] AssessmentResultInputDto inputDto, CancellationToken cancellationToken)
        {
            int examinerCode = UserService.GetUserCode(CurrentUser.Username);
            await _setAssessmentResultHandler.Handle(inputDto, examinerCode, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("result-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetResultSti([FromBody] AssessmentResultInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2021;
            JsonReportId reportId = await JsonOperation.ExportToJson(inputDto, cancellationToken, reportCode);
            return Ok(reportId);
        }

        [HttpPost]
        [Route("set-time")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SetAssessmentTimeOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetTime([FromBody] AssessmentSetTimeInputDto inputDto, CancellationToken cancellationToken)
        {
            int examinerCode = UserService.GetUserCode(CurrentUser.Username);
            SetAssessmentTimeDataOutputDto result = await _setAssessmentTimeHandler.Handle(inputDto, examinerCode, cancellationToken);
            SetAssessmentTimeOutputDto outputDto = GetAssessmentTimeOutputDto(inputDto, result);

            return Ok(outputDto);
        }

        [HttpPost]
        [Route("set-light-result")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MoshtrakUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetAssessmentLite([FromBody] LightAssessmentResultInputDto inputDto, CancellationToken cancellationToken)
        {
            int userName = UserService.GetUserCode(CurrentUser.Username);
            await _setLightAssessmentResultHandler.Handle(inputDto, userName, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("reAssessment")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TrackNumberWithDescriptionInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReAssessment([FromBody] TrackNumberWithDescriptionInputDto inputDto, CancellationToken cancellationToken)
        {
            int userName = UserService.GetUserCode(CurrentUser.Username);
            await _reAssessmentRequestHandler.Handle(inputDto, userName, cancellationToken);
            return Ok(inputDto);
        }
        private SetAssessmentTimeOutputDto GetAssessmentTimeOutputDto(AssessmentSetTimeInputDto inputDto, SetAssessmentTimeDataOutputDto result)
        {
            string customerText = string.Format(SmsTemplates.RequestTimeSet, result.AssessmentName, result.AssessmentMobileNumber, result.AssessmentDateJalai, result.TrackNumber);
            string assessmentText = result.ServiceGroupId == 1 ?
                string.Format(SmsTemplates.NewRequestTimeSetAssessment, result.AssessmentName, result.AssessmentDateJalai, result.Address, result.FullName, result.NeighbourBillId, result.MobileNumber, result.ServiceSelectedList, result.TrackNumber, result.NeighbourBillId) :
                string.Format(SmsTemplates.AfterSaleRequestTimeSetAssessment, result.AssessmentName, result.AssessmentDateJalai, result.Address, result.FullName, result.BillId, result.MobileNumber, result.ServiceSelectedList, result.TrackNumber);
            if (inputDto.HasAssessmentSms)
            {
                _backgroudJobClient.Enqueue(() => _smsOldHandler.Send(result.AssessmentMobileNumber, assessmentText, result.TrackId));
            }
            if (inputDto.HasCustomerSms)
            {
                _backgroudJobClient.Enqueue(() => _smsOldHandler.Send(result.MobileNumber, customerText, result.TrackId));
            }
            return new SetAssessmentTimeOutputDto(inputDto.HasAssessmentSms, inputDto.HasCustomerSms, inputDto.HasAssessmentSms ? assessmentText : null, inputDto.HasCustomerSms ? customerText : null);

        }
    }
}
