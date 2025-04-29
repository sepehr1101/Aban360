using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/invoice-together")]
    public class InvoiceTogetherCreateController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceTogetherCreateHandler _invoiceTogetherCreateHandler;
        public InvoiceTogetherCreateController(
            IUnitOfWork uow, 
            IInvoiceTogetherCreateHandler invoiceTogetherCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceTogetherCreateHandler = invoiceTogetherCreateHandler;
            _invoiceTogetherCreateHandler.NotNull(nameof(invoiceTogetherCreateHandler));
        }

        [HttpPost, HttpGet]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InvoiceTogetherCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] InvoiceTogetherCreateDto createDto, CancellationToken cancellationToken)
        {
            await _invoiceTogetherCreateHandler.Handle(createDto,cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
