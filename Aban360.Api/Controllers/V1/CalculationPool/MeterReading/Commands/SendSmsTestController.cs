using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/test")]
    public class SendSmsTestController : BaseController
    {
        private readonly IMeterReadingDetailSendSmsHandler _SendSmsInsertHandler;
        public SendSmsTestController(IMeterReadingDetailSendSmsHandler SendSmsInsertHandler)
        {
            _SendSmsInsertHandler = SendSmsInsertHandler;
            _SendSmsInsertHandler.NotNull(nameof(SendSmsInsertHandler));
        }

        [HttpPost]
        [Route("send/flowid")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert(int flowid, CancellationToken cancellationToken)
        {
            await _SendSmsInsertHandler.Handle(flowid, CurrentUser.UserId, cancellationToken);
            return Ok(flowid);
        }
    }
}
