using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/invoice-insert-mode")]
    public class InvoiceLineItemInsertModeGetAllController : BaseController
    {       
        private readonly IInvoiceLineItemInsertModeGetAllHandler _invoiceLineItemInsertModeGetAllHandler;
        public InvoiceLineItemInsertModeGetAllController(
            IInvoiceLineItemInsertModeGetAllHandler invoiceLineItemInsertModeGetAllHandler)
        {           
            _invoiceLineItemInsertModeGetAllHandler = invoiceLineItemInsertModeGetAllHandler;
            _invoiceLineItemInsertModeGetAllHandler.NotNull(nameof(invoiceLineItemInsertModeGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<InvoiceLineItemInsertModeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<InvoiceLineItemInsertModeGetDto> InvoiceLineItemInsertModes = await _invoiceLineItemInsertModeGetAllHandler.Handle(cancellationToken);
            return Ok(InvoiceLineItemInsertModes);
        }
    }
}
