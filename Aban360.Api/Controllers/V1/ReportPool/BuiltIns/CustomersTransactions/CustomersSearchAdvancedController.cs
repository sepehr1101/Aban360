using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/customres-search-advanced")]
    public class CustomersSearchAdvancedController : BaseController
    {
        private readonly ICustomerSearchAdvancedHandler _customerSearchAdvancedHandler;
        public CustomersSearchAdvancedController(ICustomerSearchAdvancedHandler customerSearchAdvancedHandler)
        {
            _customerSearchAdvancedHandler = customerSearchAdvancedHandler;
            _customerSearchAdvancedHandler.NotNull(nameof(customerSearchAdvancedHandler));
        }

        [HttpPost, HttpGet]
        [Route("info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CustomerSearchOutputDto >>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInfo(CustomerSearchAdvancedInputDto input, CancellationToken cancellationToken)
        {
            ICollection<CustomerSearchOutputDto> customer = await _customerSearchAdvancedHandler.Handle(input, cancellationToken);
            return Ok(customer);
        }
    }
}
