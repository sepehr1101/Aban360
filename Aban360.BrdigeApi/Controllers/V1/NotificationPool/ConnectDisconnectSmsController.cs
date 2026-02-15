using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.NotificationPool.Application.Features.Sms;
using Aban360.NotificationPool.Domain.Features.Sms;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.NotificationPool
{
    [Route("v1/sms")]
    public class ConnectDisconnectSmsController:BaseController
    {
        private readonly IConsumerSummaryGetHandler _customerInfoHandler;
        private readonly ISmsOldHandler _smsHandler;
        private readonly IBackgroundJobClient _jobClient;

        public ConnectDisconnectSmsController(
            IConsumerSummaryGetHandler customerInfoHandler,
            ISmsOldHandler smsOldHandler,
            IBackgroundJobClient jobClient)
        {
            _customerInfoHandler = customerInfoHandler;
            _customerInfoHandler.NotNull(nameof(customerInfoHandler));

            _smsHandler = smsOldHandler;
            _smsHandler.NotNull(nameof(_smsHandler));

            _jobClient = jobClient;
            _jobClient.NotNull(nameof(_jobClient));
        }

        [HttpPost]
        [Route("connected")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchInput>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendConnected([FromBody] ServiceConnectSmsDto input, CancellationToken cancellationToken)
        {
            var customerInfo= await _customerInfoHandler.Handle(input.BillId, cancellationToken);
            string text = string.Format(SmsTemplates.ServiceLinkConnected, customerInfo.ZoneTitle, input.BillId, input.When, Environment.NewLine);
            _jobClient.Enqueue(() => _smsHandler.Send(customerInfo.MobileNumber, text));
            return Ok(input);
        }

        [HttpPost]
        [Route("disconnected")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchInput>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendDisonnected([FromBody] ServiceLinkDisconnectSmsDto input, CancellationToken cancellationToken)
        {
            var customerInfo = await _customerInfoHandler.Handle(input.BillId, cancellationToken);
            string text = string.Format(SmsTemplates.ServiceLinkDisconnected, customerInfo.ZoneTitle, input.BillId, input.When, Environment.NewLine);
            _jobClient.Enqueue(() => _smsHandler.Send(customerInfo.MobileNumber, text));
            return Ok(input);
        }

        [HttpPost]
        [Route("connect-alert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchInput>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendConnectAlert([FromBody] ServiceConnectSmsDto input, CancellationToken cancellationToken)
        {
            var customerInfo = await _customerInfoHandler.Handle(input.BillId, cancellationToken);
            string text = string.Format(SmsTemplates.ServiceLinkConnectAlert, customerInfo.ZoneTitle, input.BillId, input.Hour, Environment.NewLine);
            _jobClient.Enqueue(() => _smsHandler.Send(customerInfo.MobileNumber, text));
            return Ok(input);
        }

        [HttpPost]
        [Route("disconnect-alert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchInput>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendDisonnectAlert([FromBody] ServiceLinkDisconnectSmsDto input, CancellationToken cancellationToken)
        {
            var customerInfo = await _customerInfoHandler.Handle(input.BillId, cancellationToken);
            string text = string.Format(SmsTemplates.ServiceLinkDisconnectAlert, customerInfo.ZoneTitle, input.BillId, input.Why, input.Hour, Environment.NewLine);
            _jobClient.Enqueue(() => _smsHandler.Send(customerInfo.MobileNumber, text));
            return Ok(input);
        }
    }
}
