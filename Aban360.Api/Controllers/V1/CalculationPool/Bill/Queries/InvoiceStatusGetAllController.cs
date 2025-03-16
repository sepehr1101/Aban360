using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/invoice-status")]
    public class InvoiceStatusGetAllController : BaseController
    {
        private readonly IInvoiceStatusGetAllHandler _invoiceStatusGetAllHandler;
        public InvoiceStatusGetAllController(
            IInvoiceStatusGetAllHandler invoiceStatusGetAllHandler)
        {
            _invoiceStatusGetAllHandler = invoiceStatusGetAllHandler;
            _invoiceStatusGetAllHandler.NotNull(nameof(invoiceStatusGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ICollection<InvoiceStatusGetDto>>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<InvoiceStatusGetDto> InvoiceStatuss = await _invoiceStatusGetAllHandler.Handle(cancellationToken);
            return Ok(InvoiceStatuss);
        }
    }
}
