using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Commands
{
    [Route("v1/invoice-status")]
    public class InvoiceStatusDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceStatusDeleteHandler _invoiceStatusDeleteHandler;
        public InvoiceStatusDeleteController(
            IUnitOfWork uow,
            IInvoiceStatusDeleteHandler invoiceStatusDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceStatusDeleteHandler = invoiceStatusDeleteHandler;
            _invoiceStatusDeleteHandler.NotNull(nameof(invoiceStatusDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] InvoiceStatusDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _invoiceStatusDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
