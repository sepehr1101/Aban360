using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/line-item-type-group")]
    public class LineItemTypeGroupUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ILineItemTypeGroupUpdateHandler _lineItemTypeGroupUpdateHandler;
        public LineItemTypeGroupUpdateController(
            IUnitOfWork uow,
            ILineItemTypeGroupUpdateHandler lineItemTypeGroupUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _lineItemTypeGroupUpdateHandler = lineItemTypeGroupUpdateHandler;
            _lineItemTypeGroupUpdateHandler.NotNull(nameof(lineItemTypeGroupUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] LineItemTypeGroupUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _lineItemTypeGroupUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
