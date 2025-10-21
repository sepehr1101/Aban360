using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/customer")]
    public class CustomerInfoByBillIdController : BaseController
    {
        private readonly ICustomerInfoHandler _customerInfoByBillIdHandler;
        public CustomerInfoByBillIdController(ICustomerInfoHandler customerInfoByBillIdHandler)
        {
            _customerInfoByBillIdHandler = customerInfoByBillIdHandler;
            _customerInfoByBillIdHandler.NotNull(nameof(customerInfoByBillIdHandler));
        }

        [HttpPost]
        [Route("info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerInfoByBillIdOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByBillId(SearchInput input, CancellationToken cancellationToken)
        {
            CustomerInfoByBillIdOutputDto customerInfo = await _customerInfoByBillIdHandler.Handle(input, cancellationToken);
            return Ok(customerInfo);
        }
    }
}