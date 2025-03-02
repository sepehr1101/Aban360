using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/invoice-status")]
    public class InvoiceStatusGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceStatusGetAllHandler _invoiceStatusGetAllHandler;
        public InvoiceStatusGetAllController(
            IUnitOfWork uow,
            IInvoiceStatusGetAllHandler invoiceStatusGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceStatusGetAllHandler = invoiceStatusGetAllHandler;
            _invoiceStatusGetAllHandler.NotNull(nameof(invoiceStatusGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var InvoiceStatuss = await _invoiceStatusGetAllHandler.Handle(cancellationToken);
            return Ok(InvoiceStatuss);
        }
    }
}
