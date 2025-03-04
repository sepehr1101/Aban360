using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Commands
{
    [Route("v1/invoice-status")]
    public class InvoiceStatusCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceStatusCreateHandler _invoiceStatusCreateHandler;
        public InvoiceStatusCreateController(
            IUnitOfWork uow,
            IInvoiceStatusCreateHandler invoiceStatusCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceStatusCreateHandler = invoiceStatusCreateHandler;
            _invoiceStatusCreateHandler.NotNull(nameof(invoiceStatusCreateHandler));
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
    }
}
