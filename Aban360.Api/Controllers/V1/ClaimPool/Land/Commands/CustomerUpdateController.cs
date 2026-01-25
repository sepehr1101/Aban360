using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/customer")]
    public class CustomerUpdateController : BaseController
    {
        private readonly ICustomerUpdateHandler _customerUpdateHandler;
        public CustomerUpdateController(ICustomerUpdateHandler customerUpdateHandler)
        {
            _customerUpdateHandler = customerUpdateHandler;
            _customerUpdateHandler.NotNull(nameof(customerUpdateHandler));
        }

        [HttpGet, HttpPost]
        [Route("update-1")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update1([FromBody] SubscriptionGetDto inputDto, CancellationToken cancellationToken)
        {
            await _customerUpdateHandler.Handle(inputDto, cancellationToken);
            return Ok(inputDto);
        }
    }
}
