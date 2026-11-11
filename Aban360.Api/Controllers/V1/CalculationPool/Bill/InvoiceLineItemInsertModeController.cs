using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill
{
    [Route("v1/invoice-insert-mode")]
    public class InvoiceLineItemInsertModeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceLineItemInsertModeCreateHandler _invoiceLineItemInsertModeCreateHandler;
        private readonly IInvoiceLineItemInsertModeUpdateHandler _invoiceLineItemInsertModeUpdateHandler;
        private readonly IInvoiceLineItemInsertModeDeleteHandler _invoiceLineItemInsertModeDeleteHandler;
        private readonly IInvoiceLineItemInsertModeGetSingleHandler _invoiceLineItemInsertModeGetSingleHandler;
        private readonly IInvoiceLineItemInsertModeGetAllHandler _invoiceLineItemInsertModeGetAllHandler;

        public InvoiceLineItemInsertModeController(
            IUnitOfWork uow,
            IInvoiceLineItemInsertModeCreateHandler invoiceLineItemInsertModeCreateHandler,
            IInvoiceLineItemInsertModeUpdateHandler invoiceLineItemInsertModeUpdateHandler,
            IInvoiceLineItemInsertModeDeleteHandler invoiceLineItemInsertModeDeleteHandler,
            IInvoiceLineItemInsertModeGetSingleHandler invoiceLineItemInsertModeGetSingleHandler,
            IInvoiceLineItemInsertModeGetAllHandler invoiceLineItemInsertModeGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceLineItemInsertModeCreateHandler = invoiceLineItemInsertModeCreateHandler;
            _invoiceLineItemInsertModeCreateHandler.NotNull(nameof(invoiceLineItemInsertModeCreateHandler));

            _invoiceLineItemInsertModeUpdateHandler = invoiceLineItemInsertModeUpdateHandler;
            _invoiceLineItemInsertModeUpdateHandler.NotNull(nameof(invoiceLineItemInsertModeUpdateHandler));

            _invoiceLineItemInsertModeDeleteHandler = invoiceLineItemInsertModeDeleteHandler;
            _invoiceLineItemInsertModeDeleteHandler.NotNull(nameof(invoiceLineItemInsertModeDeleteHandler));

            _invoiceLineItemInsertModeGetSingleHandler = invoiceLineItemInsertModeGetSingleHandler;
            _invoiceLineItemInsertModeGetSingleHandler.NotNull(nameof(invoiceLineItemInsertModeGetSingleHandler));

            _invoiceLineItemInsertModeGetAllHandler = invoiceLineItemInsertModeGetAllHandler;
            _invoiceLineItemInsertModeGetAllHandler.NotNull(nameof(invoiceLineItemInsertModeGetAllHandler));
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

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InvoiceLineItemInsertModeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] InvoiceLineItemInsertModeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _invoiceLineItemInsertModeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
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

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InvoiceLineItemInsertModeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(InvoiceLineItemInsertModeEnum id, CancellationToken cancellationToken)
        {
            InvoiceLineItemInsertModeGetDto InvoiceLineItemInsertModes = await _invoiceLineItemInsertModeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(InvoiceLineItemInsertModes);
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
