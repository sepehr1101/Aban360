using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Requests.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.Request.Inputs;
using Aban360.ReportPool.Domain.Features.Request.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Request
{
    [Route("v1/sms")]
    public class ReceivedSmsController : BaseController
    {
        private readonly IReceivedSmdByPaigingHandler _receivedSmdByPaigingHandler;
        public ReceivedSmsController(IReceivedSmdByPaigingHandler receivedSmdByPaigingHandler)
        {
            _receivedSmdByPaigingHandler = receivedSmdByPaigingHandler;
            _receivedSmdByPaigingHandler.NotNull(nameof(receivedSmdByPaigingHandler));
        }

        [HttpPost]
        [Route("received")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ReceivedSmsDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReceivedSmsByPaigination([FromBody] ReceivedSmsInputDto inputDto, CancellationToken cancellationToken)
        {
            IEnumerable<ReceivedSmsDataOutputDto> result=await _receivedSmdByPaigingHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
