using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Commands
{
    [Route("v1/invoice-type")]
    public class InvoiceTypeCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceTypeCreateHandler _invoiceTypeCreateHandler;
        public InvoiceTypeCreateController(
            IUnitOfWork uow,
            IInvoiceTypeCreateHandler invoiceTypeCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceTypeCreateHandler = invoiceTypeCreateHandler;
            _invoiceTypeCreateHandler.NotNull(nameof(invoiceTypeCreateHandler));
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
    }
}
