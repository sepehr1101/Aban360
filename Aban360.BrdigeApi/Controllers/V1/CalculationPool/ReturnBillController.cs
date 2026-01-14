using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    [Route("v1/bill-return")]
    public class ReturnBillController : BaseController
    {
        private readonly IReturnBillPartialHandler _billToReturnedHandler;
        private readonly IReturnBillFullHandler _billFullHandler;
        public ReturnBillController(
            IReturnBillPartialHandler billToReturnedHandler,
            IReturnBillFullHandler billFullHandler)
        {
            _billToReturnedHandler = billToReturnedHandler;
            _billToReturnedHandler.NotNull(nameof(billToReturnedHandler));

            _billFullHandler = billFullHandler;
            _billFullHandler.NotNull(nameof(billFullHandler));
        }

        [HttpPost, HttpGet]
        [Route("partial")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReturnBillOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PartialReturn([FromBody] ReturnBillPartialInputDto input, CancellationToken cancellationToken)
        {
            ReturnBillOutputDto result = await _billToReturnedHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("full")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReturnBillOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> FullReturn([FromBody] ReturnBillFullInputDto input, CancellationToken cancellationToken)
        {
            ReturnBillOutputDto result = await _billFullHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
