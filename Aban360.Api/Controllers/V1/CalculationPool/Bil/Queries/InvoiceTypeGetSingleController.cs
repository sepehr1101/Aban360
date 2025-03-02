using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/invoice-type")]
    public class InvoiceTypeGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceTypeGetSingleHandler _invoiceTypeGetSingleHandler;
        public InvoiceTypeGetSingleController(
            IUnitOfWork uow,
            IInvoiceTypeGetSingleHandler invoiceTypeGetSingleHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceTypeGetSingleHandler = invoiceTypeGetSingleHandler;
            _invoiceTypeGetSingleHandler.NotNull(nameof(invoiceTypeGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var InvoiceTypes = await _invoiceTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(InvoiceTypes);
        }
    }
}
