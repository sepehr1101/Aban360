using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/invoice-status")]
    public class InvoiceStatusGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceStatusGetSingleHandler _invoiceStatusGetSingleHandler;
        public InvoiceStatusGetSingleController(
            IUnitOfWork uow,
            IInvoiceStatusGetSingleHandler invoiceStatusGetSingleHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceStatusGetSingleHandler = invoiceStatusGetSingleHandler;
            _invoiceStatusGetSingleHandler.NotNull(nameof(invoiceStatusGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var InvoiceStatuss = await _invoiceStatusGetSingleHandler.Handle(id, cancellationToken);
            return Ok(InvoiceStatuss);
        }
    }
}
