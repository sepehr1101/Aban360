using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Commands
{
    [Route("v1/invoice-insert-mode")]
    public class InvoiceLineItemInsertModeUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceLineItemInsertModeUpdateHandler _invoiceLineItemInsertModeUpdateHandler;
        public InvoiceLineItemInsertModeUpdateController(
            IUnitOfWork uow,
            IInvoiceLineItemInsertModeUpdateHandler invoiceLineItemInsertModeUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceLineItemInsertModeUpdateHandler = invoiceLineItemInsertModeUpdateHandler;
            _invoiceLineItemInsertModeUpdateHandler.NotNull(nameof(invoiceLineItemInsertModeUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InvoiceLineItemInsertModeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] InvoiceLineItemInsertModeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _invoiceLineItemInsertModeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
