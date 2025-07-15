using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/customer")]
    public class GetCustomerOldInfoController : BaseController
    {
        private readonly ICustomerOldInfoHandler _customerOldInfoHandler;
        public GetCustomerOldInfoController(ICustomerOldInfoHandler customerOldInfoHandler)
        {
            _customerOldInfoHandler = customerOldInfoHandler;
            _customerOldInfoHandler.NotNull(nameof(customerOldInfoHandler));
        }

        [HttpPost]
        [Route("old-info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerOldInfoOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByBillId(CustomerOldInfoInputDto input, CancellationToken cancellationToken)
        {
            CustomerOldInfoOutputDto customerInfo = await _customerOldInfoHandler.Handle(input, cancellationToken);
            return Ok(customerInfo);
        }
    }
}
