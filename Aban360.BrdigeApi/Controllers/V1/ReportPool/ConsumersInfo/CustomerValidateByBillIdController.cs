using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/customer")]
    public class CustomerValidateByBillIdController : BaseController
    {
        private readonly ICustomerValidateByBillIdHandler _customerInfoByBillIdHandler;
        public CustomerValidateByBillIdController(ICustomerValidateByBillIdHandler customerInfoByBillIdHandler)
        {
            _customerInfoByBillIdHandler = customerInfoByBillIdHandler;
            _customerInfoByBillIdHandler.NotNull(nameof(customerInfoByBillIdHandler));
        }

        [HttpPost]
        [Route("validate")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerBillIdValidateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Validate(SearchInput input, CancellationToken cancellationToken)
        {
            CustomerBillIdValidateDto customerInfo = await _customerInfoByBillIdHandler.Handle(input, CurrentUser, cancellationToken);
            return Ok(customerInfo);
        }
    }
}