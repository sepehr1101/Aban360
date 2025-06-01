using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/customres-search")]
    public class CustomersSearchController : BaseController
    {
        private readonly ICustomerSearchHandler _customerSearchHandler;
        public CustomersSearchController(ICustomerSearchHandler customerSearchHandler)
        {
            _customerSearchHandler = customerSearchHandler;
            _customerSearchHandler.NotNull(nameof(customerSearchHandler));
        }

        [HttpPost, HttpGet]
        [Route("info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<CustomerSearchHeaderOutputDto, CustomerSearchDataOutputDto>>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInfo(CustomerSearchInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<CustomerSearchHeaderOutputDto, CustomerSearchDataOutputDto> customer = await _customerSearchHandler.Handle(input, cancellationToken);
            return Ok(customer);
        }
    }
}
