using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/line-item-type")]
    public class LineItemTypeGetSingleController : BaseController
    {
        private readonly ILineItemTypeGetSingleHandler _lineItemTypeGetSingleHandler;
        public LineItemTypeGetSingleController(ILineItemTypeGetSingleHandler lineItemTypeGetSingleHandler)
        {
            _lineItemTypeGetSingleHandler = lineItemTypeGetSingleHandler;
            _lineItemTypeGetSingleHandler.NotNull(nameof(lineItemTypeGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LineItemTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var lineItemTypes = await _lineItemTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(lineItemTypes);
        }
    }
}
