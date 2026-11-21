using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.BaseEntities;
using Aban360.Api.Filters;
using Aban360.ClaimPool.Application.Features.People.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Commands
{
    [Route("v1/customer-info")]
    public class CustomerInfoController : BaseController
    {
        private readonly ICustomerInfoService _customerInfoService;
        public CustomerInfoController(ICustomerInfoService customerInfoService)
        {
            _customerInfoService = customerInfoService;
            _customerInfoService.NotNull(nameof(customerInfoService));
        }

        [HttpPost]
        [Route("update/level1")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerInfoLevel1UpdateDto>), StatusCodes.Status200OK)]
        [ActionName("Level1")]
        public async Task<IActionResult> Level1FromCustomerInfo0(CustomerInfoLevel1UpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _customerInfoService.UpdateAsync(updateDto, cancellationToken);
            return Ok(updateDto);
        }

        [HttpPost]
        [Route("update/level2")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerInfoLevel2UpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Level2(CustomerInfoLevel2UpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _customerInfoService.UpdateAsync(updateDto, cancellationToken);
            return Ok(updateDto);
        }

        [HttpPost]
        [Route("get-by-billid")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerInfoGetDto>), StatusCodes.Status200OK)]
        [ActionName("Level1")]
        public async Task<IActionResult> Level1FromCustomerInfo1(SearchInput input, CancellationToken cancellationToken)
        {
            CustomerInfoGetDto customerInfo = await _customerInfoService.GetByBillIdAsync(input, cancellationToken);
            return Ok(customerInfo);
        }
    }
}
