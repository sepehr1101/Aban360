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

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill
{
    [Route("v1/invoice-type")]
    public class InvoiceTypeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceTypeCreateHandler _invoiceTypeCreateHandler;
        private readonly IInvoiceTypeUpdateHandler _invoiceTypeUpdateHandler;
        private readonly IInvoiceTypeDeleteHandler _invoiceTypeDeleteHandler;
        private readonly IInvoiceTypeGetSingleHandler _invoiceTypeGetSingleHandler;
        private readonly IInvoiceTypeGetAllHandler _invoiceTypeGetAllHandler;

        public InvoiceTypeController(
            IUnitOfWork uow,
            IInvoiceTypeCreateHandler invoiceTypeCreateHandler,
            IInvoiceTypeUpdateHandler invoiceTypeUpdateHandler,
            IInvoiceTypeDeleteHandler invoiceTypeDeleteHandler,
            IInvoiceTypeGetSingleHandler invoiceTypeGetSingleHandler,
            IInvoiceTypeGetAllHandler invoiceTypeGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceTypeCreateHandler = invoiceTypeCreateHandler;
            _invoiceTypeCreateHandler.NotNull(nameof(invoiceTypeCreateHandler));

            _invoiceTypeUpdateHandler = invoiceTypeUpdateHandler;
            _invoiceTypeUpdateHandler.NotNull(nameof(invoiceTypeUpdateHandler));

            _invoiceTypeDeleteHandler = invoiceTypeDeleteHandler;
            _invoiceTypeDeleteHandler.NotNull(nameof(invoiceTypeDeleteHandler));

            _invoiceTypeGetSingleHandler = invoiceTypeGetSingleHandler;
            _invoiceTypeGetSingleHandler.NotNull(nameof(invoiceTypeGetSingleHandler));

            _invoiceTypeGetAllHandler = invoiceTypeGetAllHandler;
            _invoiceTypeGetAllHandler.NotNull(nameof(invoiceTypeGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InvoiceTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] InvoiceTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _invoiceTypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InvoiceTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] InvoiceTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _invoiceTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InvoiceTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] InvoiceTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _invoiceTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InvoiceTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            InvoiceTypeGetDto InvoiceTypes = await _invoiceTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(InvoiceTypes);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<InvoiceTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<InvoiceTypeGetDto> InvoiceTypes = await _invoiceTypeGetAllHandler.Handle(cancellationToken);
            return Ok(InvoiceTypes);
        }
    }
}
