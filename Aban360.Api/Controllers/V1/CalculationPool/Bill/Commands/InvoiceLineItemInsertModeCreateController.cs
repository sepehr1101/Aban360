using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Commands
{
    [Route("v1/invoice-insert-mode")]
    public class InvoiceLineItemInsertModeCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceLineItemInsertModeCreateHandler _invoiceLineItemInsertModeCreateHandler;
        public InvoiceLineItemInsertModeCreateController(
            IUnitOfWork uow,
            IInvoiceLineItemInsertModeCreateHandler invoiceLineItemInsertModeCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceLineItemInsertModeCreateHandler = invoiceLineItemInsertModeCreateHandler;
            _invoiceLineItemInsertModeCreateHandler.NotNull(nameof(invoiceLineItemInsertModeCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InvoiceLineItemInsertModeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] InvoiceLineItemInsertModeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _invoiceLineItemInsertModeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
