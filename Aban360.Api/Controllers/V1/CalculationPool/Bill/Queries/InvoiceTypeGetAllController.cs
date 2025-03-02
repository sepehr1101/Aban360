using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/invoice-type")]
    public class InvoiceTypeGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceTypeGetAllHandler _invoiceTypeGetAllHandler;
        public InvoiceTypeGetAllController(
            IUnitOfWork uow,
            IInvoiceTypeGetAllHandler invoiceTypeGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceTypeGetAllHandler = invoiceTypeGetAllHandler;
            _invoiceTypeGetAllHandler.NotNull(nameof(invoiceTypeGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var InvoiceTypes = await _invoiceTypeGetAllHandler.Handle(cancellationToken);
            return Ok(InvoiceTypes);
        }
    }
}
