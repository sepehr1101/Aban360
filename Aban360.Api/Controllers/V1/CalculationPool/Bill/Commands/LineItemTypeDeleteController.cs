using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/line-item-type")]
    public class LineItemTypeDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ILineItemTypeDeleteHandler _lineItemTypeDeleteHandler;
        public LineItemTypeDeleteController(
            IUnitOfWork uow,
            ILineItemTypeDeleteHandler lineItemTypeDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _lineItemTypeDeleteHandler = lineItemTypeDeleteHandler;
            _lineItemTypeDeleteHandler.NotNull(nameof(lineItemTypeDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LineItemTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] LineItemTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _lineItemTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
