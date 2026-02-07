using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
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
        [Route("update-full")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateFull([FromBody] SubscriptionGetDto inputDto, CancellationToken cancellationToken)
        {
            await _customerUpdateHandler.Handle(inputDto, cancellationToken);
            return Ok(inputDto);
        }
        
        
        [HttpGet, HttpPost]
        [Route("update-1")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerUpdate1Dto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update1([FromBody] CustomerUpdate1Dto inputDto, CancellationToken cancellationToken)
        {
            await _customerUpdateHandler.Handle(inputDto, cancellationToken);
            return Ok(inputDto);
        }
        

        [HttpGet, HttpPost]
        [Route("update-2")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerUpdate2Dto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update2([FromBody] CustomerUpdate2Dto inputDto, CancellationToken cancellationToken)
        {
            await _customerUpdateHandler.Handle(inputDto, cancellationToken);
            return Ok(inputDto);
        }
        

        [HttpGet, HttpPost]
        [Route("update-3")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerUpdate3Dto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update3([FromBody] CustomerUpdate3Dto inputDto, CancellationToken cancellationToken)
        {
            await _customerUpdateHandler.Handle(inputDto, cancellationToken);
            return Ok(inputDto);
        }
        

        [HttpGet, HttpPost]
        [Route("update-5")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerUpdate5Dto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update5([FromBody] CustomerUpdate5Dto inputDto, CancellationToken cancellationToken)
        {
            await _customerUpdateHandler.Handle(inputDto, cancellationToken);
            return Ok(inputDto);
        }
    }
}
