using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Commands
{
    [Route("v1/invoice")]
    public class InvoiceCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceCreateHandler _invoiceCreateHandler;
        public InvoiceCreateController(
            IUnitOfWork uow,
            IInvoiceCreateHandler invoiceCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceCreateHandler = invoiceCreateHandler;
            _invoiceCreateHandler.NotNull(nameof(invoiceCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InvoiceCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] InvoiceCreateDto createDto, CancellationToken cancellationToken)
        {
            await _invoiceCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
