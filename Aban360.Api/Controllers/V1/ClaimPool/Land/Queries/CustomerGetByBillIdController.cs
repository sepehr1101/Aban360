using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.Land.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/customer")]
    public class CustomerGetByBillIdController : BaseController
    {
        private readonly ICustomerLookupService _customerGetByBillIdHandler;
        public CustomerGetByBillIdController(ICustomerLookupService customerGetByBillIdHandler)
        {
            _customerGetByBillIdHandler = customerGetByBillIdHandler;
            _customerGetByBillIdHandler.NotNull(nameof(customerGetByBillIdHandler));
        }

        [HttpPost]
        [Route("get-to-update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] SearchInput inputDto, CancellationToken cancellationToken)
        {
            SubscriptionGetDto result = await _customerGetByBillIdHandler.CustomerGetByBillIdAsync(inputDto.Input, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("get-to-update/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByQuery(string billId, CancellationToken cancellationToken)
        {
            SubscriptionGetDto result = await _customerGetByBillIdHandler.CustomerGetByBillIdAsync(billId, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
