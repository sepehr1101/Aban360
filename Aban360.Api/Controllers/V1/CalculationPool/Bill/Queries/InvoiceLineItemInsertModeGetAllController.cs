using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/invoice-line-item-insert-mode")]
    public class InvoiceLineItemInsertModeGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceLineItemInsertModeGetAllHandler _invoiceLineItemInsertModeGetAllHandler;
        public InvoiceLineItemInsertModeGetAllController(
            IUnitOfWork uow,
            IInvoiceLineItemInsertModeGetAllHandler invoiceLineItemInsertModeGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceLineItemInsertModeGetAllHandler = invoiceLineItemInsertModeGetAllHandler;
            _invoiceLineItemInsertModeGetAllHandler.NotNull(nameof(invoiceLineItemInsertModeGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var InvoiceLineItemInsertModes = await _invoiceLineItemInsertModeGetAllHandler.Handle(cancellationToken);
            return Ok(InvoiceLineItemInsertModes);
        }
    }
}
