// Frozen original consumer; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.CalculationPool.Bill;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers20.Aban360.Api.Controllers.V1.CalculationPool.Bill
{
    [Route("v1/invoice-status")]
    public class InvoiceStatusController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceStatusCreateHandler _invoiceStatusCreateHandler;
        private readonly IInvoiceStatusUpdateHandler _invoiceStatusUpdateHandler;
        private readonly IInvoiceStatusDeleteHandler _invoiceStatusDeleteHandler;
        private readonly IInvoiceStatusGetSingleHandler _invoiceStatusGetSingleHandler;
        private readonly IInvoiceStatusGetAllHandler _invoiceStatusGetAllHandler;

        public InvoiceStatusController(
            IUnitOfWork uow,
            IInvoiceStatusCreateHandler invoiceStatusCreateHandler,
            IInvoiceStatusUpdateHandler invoiceStatusUpdateHandler,
            IInvoiceStatusDeleteHandler invoiceStatusDeleteHandler,
            IInvoiceStatusGetSingleHandler invoiceStatusGetSingleHandler,
            IInvoiceStatusGetAllHandler invoiceStatusGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceStatusCreateHandler = invoiceStatusCreateHandler;
            _invoiceStatusCreateHandler.NotNull(nameof(invoiceStatusCreateHandler));

            _invoiceStatusUpdateHandler = invoiceStatusUpdateHandler;
            _invoiceStatusUpdateHandler.NotNull(nameof(invoiceStatusUpdateHandler));

            _invoiceStatusDeleteHandler = invoiceStatusDeleteHandler;
            _invoiceStatusDeleteHandler.NotNull(nameof(invoiceStatusDeleteHandler));

            _invoiceStatusGetSingleHandler = invoiceStatusGetSingleHandler;
            _invoiceStatusGetSingleHandler.NotNull(nameof(invoiceStatusGetSingleHandler));

            _invoiceStatusGetAllHandler = invoiceStatusGetAllHandler;
            _invoiceStatusGetAllHandler.NotNull(nameof(invoiceStatusGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InvoiceStatusCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] InvoiceStatusCreateDto createDto, CancellationToken cancellationToken)
        {
            await _invoiceStatusCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InvoiceStatusUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] InvoiceStatusUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _invoiceStatusUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InvoiceStatusDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] InvoiceStatusDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _invoiceStatusDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InvoiceStatusGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            InvoiceStatusGetDto InvoiceStatuss = await _invoiceStatusGetSingleHandler.Handle(id, cancellationToken);
            return Ok(InvoiceStatuss);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ICollection<InvoiceStatusGetDto>>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<InvoiceStatusGetDto> InvoiceStatuss = await _invoiceStatusGetAllHandler.Handle(cancellationToken);
            return Ok(InvoiceStatuss);
        }
    }
}
