using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/line-item-type-group")]
    public class LineItemTypeGroupGetAllController : BaseController
    {
        private readonly ILineItemTypeGroupGetAllHandler _lineItemTypeGroupGetAllHandler;
        public LineItemTypeGroupGetAllController(ILineItemTypeGroupGetAllHandler LineItemTypeGroupGetAllHandler)
        {
            _lineItemTypeGroupGetAllHandler = LineItemTypeGroupGetAllHandler;
            _lineItemTypeGroupGetAllHandler.NotNull(nameof(LineItemTypeGroupGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<LineItemTypeGroupGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var lineItemTypeGroups = await _lineItemTypeGroupGetAllHandler.Handle(cancellationToken);
            return Ok(lineItemTypeGroups);
        }
    }
}
