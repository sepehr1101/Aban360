using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/line-item-type-group")]
    public class LineItemTypeGroupGetSingleController : BaseController
    {
        private readonly ILineItemTypeGroupGetSingleHandler _lineItemTypeGroupGetSingleHandler;
        public LineItemTypeGroupGetSingleController(ILineItemTypeGroupGetSingleHandler lineItemTypeGroupGetSingleHandler)
        {
            _lineItemTypeGroupGetSingleHandler = lineItemTypeGroupGetSingleHandler;
            _lineItemTypeGroupGetSingleHandler.NotNull(nameof(lineItemTypeGroupGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LineItemTypeGroupGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var lineItemTypeGroups = await _lineItemTypeGroupGetSingleHandler.Handle(id, cancellationToken);
            return Ok(lineItemTypeGroups);
        }
    }
}
