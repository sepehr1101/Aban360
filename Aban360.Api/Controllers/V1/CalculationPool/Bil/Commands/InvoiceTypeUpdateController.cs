using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Commands
{
    [Route("v1/invoice-type")]
    public class InvoiceTypeUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceTypeUpdateHandler _invoiceTypeUpdateHandler;
        public InvoiceTypeUpdateController(
            IUnitOfWork uow,
            IInvoiceTypeUpdateHandler invoiceTypeUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceTypeUpdateHandler = invoiceTypeUpdateHandler;
            _invoiceTypeUpdateHandler.NotNull(nameof(invoiceTypeUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] InvoiceTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _invoiceTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
