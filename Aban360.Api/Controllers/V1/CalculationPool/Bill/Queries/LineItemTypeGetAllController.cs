using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/line-item-type")]
    public class LineItemTypeGetAllController : BaseController
    {
        private readonly ILineItemTypeGetAllHandler _lineItemTypeGetAllHandler;
        public LineItemTypeGetAllController(ILineItemTypeGetAllHandler LineItemTypeGetAllHandler)
        {
            _lineItemTypeGetAllHandler = LineItemTypeGetAllHandler;
            _lineItemTypeGetAllHandler.NotNull(nameof(LineItemTypeGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<LineItemTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<LineItemTypeGetDto> lineItemTypes = await _lineItemTypeGetAllHandler.Handle(cancellationToken);
            return Ok(lineItemTypes);
        }
    }
}
