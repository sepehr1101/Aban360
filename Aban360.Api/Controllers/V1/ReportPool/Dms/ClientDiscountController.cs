using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Dms.Commands.Contracts;
using Aban360.ReportPool.Application.Features.Dms.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.Dms;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Dms
{
    [Route("v1/client-discount")]
    public class ClientDiscountController : BaseController
    {
        private readonly IClientDiscountGetAllHandler _requestDiscountHandler;
        private readonly IClientDiscountUpdateHandler _requestDiscountUpdateHandler;
        private readonly IClientDiscountInsertHandler _requestDiscountInsertHandler;
        public ClientDiscountController(
            IClientDiscountGetAllHandler requestDiscountHandler,
            IClientDiscountUpdateHandler requestDiscountUpdateHandler,
            IClientDiscountInsertHandler requestDiscountInsertHandler)
        {
            _requestDiscountHandler = requestDiscountHandler;
            _requestDiscountHandler.NotNull(nameof(requestDiscountHandler));

            _requestDiscountUpdateHandler = requestDiscountUpdateHandler;
            _requestDiscountUpdateHandler.NotNull(nameof(requestDiscountUpdateHandler));

            _requestDiscountInsertHandler = requestDiscountInsertHandler;
            _requestDiscountInsertHandler.NotNull(nameof(requestDiscountInsertHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ClientDiscount>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var data = await _requestDiscountHandler.Handle(cancellationToken);
            return Ok(data);
        }

        [HttpPost, HttpGet]
        [Route("add")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ClientDiscountInsertDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add([FromForm] ClientDiscountInsertDto input, CancellationToken cancellationToken)
        {
            await _requestDiscountInsertHandler.Handle(input, cancellationToken);
            return Ok(input);
        }

        [HttpPost, HttpGet]
        [Route("edit")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ClientDiscountInsertDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromForm] ClientDiscountUpdateDto input, CancellationToken cancellationToken)
        {
            await _requestDiscountUpdateHandler.Handle(input, cancellationToken);
            return Ok(input);
        }
    }
}
