using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Commands
{
    [Route("v1/invoice-insert-mode")]
    public class InvoiceLineItemInsertModeDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceLineItemInsertModeDeleteHandler _invoiceLineItemInsertModeDeleteHandler;
        public InvoiceLineItemInsertModeDeleteController(
            IUnitOfWork uow,
            IInvoiceLineItemInsertModeDeleteHandler invoiceLineItemInsertModeDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceLineItemInsertModeDeleteHandler = invoiceLineItemInsertModeDeleteHandler;
            _invoiceLineItemInsertModeDeleteHandler.NotNull(nameof(invoiceLineItemInsertModeDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InvoiceLineItemInsertModeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] InvoiceLineItemInsertModeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _invoiceLineItemInsertModeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
