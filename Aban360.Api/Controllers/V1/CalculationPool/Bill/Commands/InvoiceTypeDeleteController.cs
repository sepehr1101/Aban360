using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Commands
{
    [Route("v1/invoice-type")]
    public class InvoiceTypeDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceTypeDeleteHandler _invoiceTypeDeleteHandler;
        public InvoiceTypeDeleteController(
            IUnitOfWork uow,
            IInvoiceTypeDeleteHandler invoiceTypeDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceTypeDeleteHandler = invoiceTypeDeleteHandler;
            _invoiceTypeDeleteHandler.NotNull(nameof(invoiceTypeDeleteHandler));
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
    }
}
