using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Implementation;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/invoice-status")]
    public class InvoiceStatusGetSingleController : BaseController
    {      
        private readonly IInvoiceStatusGetSingleHandler _invoiceStatusGetSingleHandler;
        public InvoiceStatusGetSingleController(
            IInvoiceStatusGetSingleHandler invoiceStatusGetSingleHandler)
        {
            _invoiceStatusGetSingleHandler = invoiceStatusGetSingleHandler;
            _invoiceStatusGetSingleHandler.NotNull(nameof(invoiceStatusGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InvoiceStatusGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {   var InvoiceStatuss = await _invoiceStatusGetSingleHandler.Handle(id, cancellationToken);
            return Ok(InvoiceStatuss);
        }
    }
}
