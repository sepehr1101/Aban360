using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.NotificationPool.Application.Features.Sms;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/bill")]
    public class BillSendLatestSmsController : BaseController
    {
        private readonly IConsumerSummaryGetHandler _customerInfoHandler;
        private readonly ISmsOldHandler _smsHandler;
        private readonly IBackgroundJobClient _jobClient;
        public BillSendLatestSmsController(IConsumerSummaryGetHandler customerInfoHandler,
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
        [Route("send-sms")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchInput>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByBillId(SearchInput input, CancellationToken cancellationToken)
        {
            var customerInfo = await _customerInfoHandler.Handle(input.Input, cancellationToken);
            string text = SmsTemplates.SimpleBill;
            _jobClient.Enqueue(() => _smsHandler.Send(/*customerInfo.MobileNumber*/"09135742556", text));
            return Ok(input);
        }
    }
}
