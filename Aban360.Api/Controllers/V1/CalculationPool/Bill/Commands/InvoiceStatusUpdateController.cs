using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Commands
{
    [Route("v1/invoice-status")]
    public class InvoiceStatusUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceStatusUpdateHandler _invoiceStatusUpdateHandler;
        public InvoiceStatusUpdateController(
            IUnitOfWork uow,
            IInvoiceStatusUpdateHandler invoiceStatusUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceStatusUpdateHandler = invoiceStatusUpdateHandler;
            _invoiceStatusUpdateHandler.NotNull(nameof(invoiceStatusUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] InvoiceStatusUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _invoiceStatusUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
