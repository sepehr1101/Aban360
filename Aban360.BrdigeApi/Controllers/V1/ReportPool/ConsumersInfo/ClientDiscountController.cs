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
        private readonly IClientDiscountRemoveHandler _requestDiscountRemoveHandler;

        public ClientDiscountController(
            IClientDiscountUpdateHandler requestDiscountUpdateHandler,
            IClientDiscountInsertHandler requestDiscountInsertHandler,
            IClientDiscountRemoveHandler requestDiscountRemoveHandler)
        {
            _requestDiscountUpdateHandler = requestDiscountUpdateHandler;
            _requestDiscountUpdateHandler.NotNull(nameof(requestDiscountUpdateHandler));

            _requestDiscountInsertHandler = requestDiscountInsertHandler;
            _requestDiscountInsertHandler.NotNull(nameof(requestDiscountInsertHandler));

            _requestDiscountRemoveHandler = requestDiscountRemoveHandler;
            _requestDiscountRemoveHandler.NotNull(nameof(requestDiscountRemoveHandler));
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

        [HttpPost, HttpGet, HttpDelete]
        [Route("remove/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            await _requestDiscountRemoveHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(id);
        }
    }
}
