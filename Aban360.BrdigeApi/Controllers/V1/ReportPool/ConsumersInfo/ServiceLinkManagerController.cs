using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/service-link")]
    public class ServiceLinkManagerController : BaseController
    {
        private readonly ICustomerUpdateHandler _customerUpdateHandler;
        string successfullyDone = "با موفقیت انجام شد";
        int _disconnectState = 1;
        int _connectState = 0;
        public ServiceLinkManagerController(ICustomerUpdateHandler customerUpdateHandler)
        {
            _customerUpdateHandler = customerUpdateHandler;
            _customerUpdateHandler.NotNull(nameof(customerUpdateHandler));
        }

        [HttpPost]
        [Route("disconnect")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Disconnect([FromBody] ServiceLinkConnectionInput input, CancellationToken cancellationToken)
        {
            await _customerUpdateHandler.Handle(input, _disconnectState, cancellationToken);
            return Ok(successfullyDone);
        }

        [HttpPost]
        [Route("reconnect")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Reconnect([FromBody] ServiceLinkConnectionInput input,CancellationToken cancellationToken)
        {
            await _customerUpdateHandler.Handle(input, _connectState, cancellationToken);
            return Ok(successfullyDone);
        }
    }
}
