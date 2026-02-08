using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.NotificationPool.Application.Features.Sms;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.NotificationPool
{
    [Route("v1/sms")]
    public class ConnectDisconnectSmsController:BaseController
    {
        private readonly ICustomerInfoGetByBillIdHandler _customerInfoHandler;
        private readonly ISmsOldHandler _smsHandler;
        private readonly IBackgroundJobClient _jobClient;

        public ConnectDisconnectSmsController(
            ICustomerInfoGetByBillIdHandler customerInfoHandler,
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

        [AllowAnonymous]
        [HttpPost]
        [Route("connect")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchInput>), StatusCodes.Status200OK)]
        public IActionResult SendConnect([FromBody]SearchInput input)
        {
            string text = SmsTemplates.ServiceLinkConnect;
            _jobClient.Enqueue(() => _smsHandler.Send("09132057232", text));
            return Ok(input);
        }
    }
}
