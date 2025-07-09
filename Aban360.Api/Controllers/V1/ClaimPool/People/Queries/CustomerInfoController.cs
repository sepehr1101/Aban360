using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Queries
{
    [Route("v1/customer-info")]
    public class CustomerInfoController : BaseController
    {
        private readonly ICustomerInfoGetByBillIdHandler _customerInfoGetByBillIdHandler;
        public CustomerInfoController(ICustomerInfoGetByBillIdHandler customerInfoGetByBillIdHandler)
        {
            _customerInfoGetByBillIdHandler = customerInfoGetByBillIdHandler;
            _customerInfoGetByBillIdHandler.NotNull(nameof(customerInfoGetByBillIdHandler));
        }

        [HttpPost]
        [Route("get-by-billid")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerInfoGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Level1(SearchInput input, CancellationToken cancellationToken)
        {
            CustomerInfoGetDto customerInfo = await _customerInfoGetByBillIdHandler.Handle(input, cancellationToken);
            return Ok(customerInfo);
        }

    }
}
