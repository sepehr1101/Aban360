using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/invoice-type")]
    public class InvoiceTypeGetSingleController : BaseController
    {       
        private readonly IInvoiceTypeGetSingleHandler _invoiceTypeGetSingleHandler;
        public InvoiceTypeGetSingleController(
            IInvoiceTypeGetSingleHandler invoiceTypeGetSingleHandler)
        {
            _invoiceTypeGetSingleHandler = invoiceTypeGetSingleHandler;
            _invoiceTypeGetSingleHandler.NotNull(nameof(invoiceTypeGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InvoiceTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            InvoiceTypeGetDto InvoiceTypes = await _invoiceTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(InvoiceTypes);
        }
    }
}
