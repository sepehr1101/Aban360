using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/line-item-type-group")]
    public class LineItemTypeGroupDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ILineItemTypeGroupDeleteHandler _lineItemTypeGroupDeleteHandler;
        public LineItemTypeGroupDeleteController(
            IUnitOfWork uow,
            ILineItemTypeGroupDeleteHandler lineItemTypeGroupDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _lineItemTypeGroupDeleteHandler = lineItemTypeGroupDeleteHandler;
            _lineItemTypeGroupDeleteHandler.NotNull(nameof(lineItemTypeGroupDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LineItemTypeGroupDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] LineItemTypeGroupDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _lineItemTypeGroupDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
