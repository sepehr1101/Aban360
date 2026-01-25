using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/customer-by-billid")]
    public class CustomerGetByBillIdController : BaseController
    {
        private readonly ICustomerGetByBillIdHandler _customerGetByBillIdHandler;
        public CustomerGetByBillIdController(ICustomerGetByBillIdHandler customerGetByBillIdHandler)
        {
            _customerGetByBillIdHandler = customerGetByBillIdHandler;
            _customerGetByBillIdHandler.NotNull(nameof(customerGetByBillIdHandler));
        }

        [HttpGet, HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] SearchInput inputDto, CancellationToken cancellationToken)
        {
            SubscriptionGetDto result = await _customerGetByBillIdHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
