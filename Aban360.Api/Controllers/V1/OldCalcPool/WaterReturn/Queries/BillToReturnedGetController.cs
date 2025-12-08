using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.WaterReturn.Queries
{
    [Route("v1/water-return")]
    public class BillToReturnedGetController : BaseController
    {
        private readonly IBillToReturnListGetHandler _billToReturnedHandler;
        public BillToReturnedGetController(IBillToReturnListGetHandler billToReturnedHandler)
        {
            _billToReturnedHandler = billToReturnedHandler;
            _billToReturnedHandler.NotNull(nameof(billToReturnedHandler));
        }

        [HttpPost, HttpGet]
        [Route("get-bills")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<BillsCanRemovedOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> BillsToReturnedGet([FromBody] SearchInput input, CancellationToken cancellationToken)
        {
            IEnumerable<BillsCanRemovedOutputDto> result = await _billToReturnedHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
