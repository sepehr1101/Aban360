using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Commands
{
    [Route("v1/customer-info")]
    public class CustomerInfoController : BaseController
    {
        private readonly ICustomerInfoUpdateHandler _customerInfoUpdateHandler;
        public CustomerInfoController(ICustomerInfoUpdateHandler customerInfoUpdateHandler)
        {
            _customerInfoUpdateHandler = customerInfoUpdateHandler;
           _customerInfoUpdateHandler.NotNull(nameof(customerInfoUpdateHandler));
        }

        [HttpPost]
        [Route("update/level1")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerInfoLevel1UpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Level1(CustomerInfoLevel1UpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _customerInfoUpdateHandler.Handle(updateDto, cancellationToken);
            return Ok(updateDto);
        }


        [HttpPost]
        [Route("update/level2")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerInfoLevel2UpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Level2(CustomerInfoLevel2UpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _customerInfoUpdateHandler.Handle(updateDto, cancellationToken);
            return Ok(updateDto);
        }
    }
}
