using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Dms.Commands.Contracts;
using Aban360.ReportPool.Domain.Features.Dms;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/client-discount")]
    public class ClientDiscountController : BaseController
    {
        private readonly IClientDiscountUpdateHandler _requestDiscountUpdateHandler;
        private readonly IClientDiscountInsertHandler _requestDiscountInsertHandler;
        public ClientDiscountController(
            IClientDiscountUpdateHandler requestDiscountUpdateHandler,
            IClientDiscountInsertHandler requestDiscountInsertHandler)
        {
            _requestDiscountUpdateHandler = requestDiscountUpdateHandler;
            _requestDiscountUpdateHandler.NotNull(nameof(requestDiscountUpdateHandler));

            _requestDiscountInsertHandler = requestDiscountInsertHandler;
            _requestDiscountInsertHandler.NotNull(nameof(requestDiscountInsertHandler));
        }

        [HttpPost, HttpGet]
        [Route("add")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ClientDiscountInsertDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add([FromBody] ClientDiscountInsertDto input, CancellationToken cancellationToken)
        {
            await _requestDiscountInsertHandler.Handle(input, cancellationToken);
            return Ok(input);
        }

        [HttpPost, HttpGet]
        [Route("edit")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ClientDiscountInsertDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromBody] ClientDiscountUpdateDto input, CancellationToken cancellationToken)
        {
            await _requestDiscountUpdateHandler.Handle(input, cancellationToken);
            return Ok(input);
        }
    }
}
