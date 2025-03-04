using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bil.Queries
{
    [Route("v1/invoice-type")]
    public class InvoiceTypeGetAllController : BaseController
    {      
        private readonly IInvoiceTypeGetAllHandler _invoiceTypeGetAllHandler;
        public InvoiceTypeGetAllController(
            IInvoiceTypeGetAllHandler invoiceTypeGetAllHandler)
        {            
            _invoiceTypeGetAllHandler = invoiceTypeGetAllHandler;
            _invoiceTypeGetAllHandler.NotNull(nameof(invoiceTypeGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<InvoiceTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var InvoiceTypes = await _invoiceTypeGetAllHandler.Handle(cancellationToken);
            return Ok(InvoiceTypes);
        }
    }
}
