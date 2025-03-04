using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/line-item-type")]
    public class LineItemTypeUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ILineItemTypeUpdateHandler _lineItemTypeUpdateHandler;
        public LineItemTypeUpdateController(
            IUnitOfWork uow,
            ILineItemTypeUpdateHandler lineItemTypeUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _lineItemTypeUpdateHandler = lineItemTypeUpdateHandler;
            _lineItemTypeUpdateHandler.NotNull(nameof(lineItemTypeUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] LineItemTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _lineItemTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
