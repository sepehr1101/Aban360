using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Application.Features.Sms.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.NotificationPool.Application.Features.Sms;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/request")]
    public class AfterSaleRequestController : BaseController
    {
        private readonly IRequestAfterSaleHandler _requestAfterSaleHandler;
        private readonly IRequestDuplicateValidationHandler _requestDuplicateValidation;
        private readonly ISmsOldHandler _smsOldHandler;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public AfterSaleRequestController(
            IRequestAfterSaleHandler requestAfterSaleHandler,
            IRequestDuplicateValidationHandler requestDuplicateValidation,
            ISmsOldHandler smsOldHandler,
            IBackgroundJobClient backgroundJobClient)
        {
            _requestAfterSaleHandler = requestAfterSaleHandler;
            _requestAfterSaleHandler.NotNull(nameof(requestAfterSaleHandler));

            _requestDuplicateValidation = requestDuplicateValidation;
            _requestDuplicateValidation.NotNull(nameof(requestDuplicateValidation));

            _smsOldHandler = smsOldHandler;
            _smsOldHandler.NotNull(nameof(smsOldHandler));

            _backgroundJobClient = backgroundJobClient;
            _backgroundJobClient.NotNull(nameof(backgroundJobClient));
        }

        [HttpPost]
        [Route("a-s")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NewRequestOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AfterSaleRequest([FromBody] RequestAfterSaleInputDto inputDto, CancellationToken cancellationToken)
        {
            int userName = UserService.GetUserCode(CurrentUser.Username);
            var (moshtrakInfo, trackId) = await _requestAfterSaleHandler.Handle(inputDto, userName, cancellationToken);
            string text = string.Format(SmsTemplates.RequestRegister, moshtrakInfo.TrackNumber);
            if (inputDto.HasSms)
            {
                _backgroundJobClient.Enqueue(() => _smsOldHandler.Send(moshtrakInfo.NotificationMobile, text, trackId));
            }
            NewRequestOutputDto outputDto = new(moshtrakInfo.TrackNumber, inputDto.HasSms, inputDto.HasSms ? text : null);

            return Ok(outputDto);
        }

        [HttpPost]
        [Route("is-duplicate/a-s")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TrackingDuplicateValidationOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> IsDuplicateAfterSaleRequest([FromBody] AfterSaleTrackingDuplicateValidationInputDto inputDto, CancellationToken cancellationToken)
        {
            TrackingDuplicateValidationInputDto totalValidation = new(inputDto.BillId, null, null, TrackingDuplicateValidationTypeEnum.ByBillId);
            TrackingDuplicateValidationOutputDto result = await _requestDuplicateValidation.Handle(totalValidation, cancellationToken);
            return Ok(result);
        }
    }
}
