using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Application.Features.BuiltsIns.Sms.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.Sms
{
    [Route("v1/send-sms-to-mobile")]
    public class SendSmsToMobileController : BaseController
    {
        private readonly ISendSmsToMobileHandler _sendSmsToMobileHandler;
        public SendSmsToMobileController(ISendSmsToMobileHandler sendSmsToMobileHandler)
        {
            _sendSmsToMobileHandler = sendSmsToMobileHandler;
            _sendSmsToMobileHandler.NotNull(nameof(sendSmsToMobileHandler));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SendSmsToMobileHeaderOutputDto, SendSmsToMobileDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SendSmsToMobileInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<SendSmsToMobileHeaderOutputDto, SendSmsToMobileDataOutputDto> SendSmsToMobile = await _sendSmsToMobileHandler.Handle(input, cancellationToken);
            return Ok(SendSmsToMobile);
        }
    }
}
