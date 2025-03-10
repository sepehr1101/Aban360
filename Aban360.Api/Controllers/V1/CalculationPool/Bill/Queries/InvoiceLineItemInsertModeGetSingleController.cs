using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/invoice-insert-mode")]
    public class InvoiceLineItemInsertModeGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceLineItemInsertModeGetSingleHandler _invoiceLineItemInsertModeGetSingleHandler;
        public InvoiceLineItemInsertModeGetSingleController(
            IUnitOfWork uow,
            IInvoiceLineItemInsertModeGetSingleHandler invoiceLineItemInsertModeGetSingleHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceLineItemInsertModeGetSingleHandler = invoiceLineItemInsertModeGetSingleHandler;
            _invoiceLineItemInsertModeGetSingleHandler.NotNull(nameof(invoiceLineItemInsertModeGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InvoiceLineItemInsertModeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(InvoiceLineItemInsertModeEnum id, CancellationToken cancellationToken)
        {
            var InvoiceLineItemInsertModes = await _invoiceLineItemInsertModeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(InvoiceLineItemInsertModes);
        }
    }
}
