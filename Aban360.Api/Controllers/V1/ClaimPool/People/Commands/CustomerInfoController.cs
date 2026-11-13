using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Commands
{
    public class CustomerInfoController : BaseController
    {
        private readonly ICustomerInfoUpdateHandler _c0_customerInfoUpdateHandler;
        private readonly ICustomerInfoGetByBillIdHandler _c1_customerInfoGetByBillIdHandler;
        public CustomerInfoController(ICustomerInfoUpdateHandler c0_customerInfoUpdateHandler, ICustomerInfoGetByBillIdHandler c1_customerInfoGetByBillIdHandler)
        {
            _c0_customerInfoUpdateHandler = c0_customerInfoUpdateHandler;
            _c0_customerInfoUpdateHandler.NotNull(nameof(c0_customerInfoUpdateHandler));
            _c1_customerInfoGetByBillIdHandler = c1_customerInfoGetByBillIdHandler;
            _c1_customerInfoGetByBillIdHandler.NotNull(nameof(c1_customerInfoGetByBillIdHandler));
        }

        [HttpPost]
        [Route("~/v1/customer-info/update/level1")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerInfoLevel1UpdateDto>), StatusCodes.Status200OK)]
        [ActionName("Level1")]
        public async Task<IActionResult> Level1FromCustomerInfo0(CustomerInfoLevel1UpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _c0_customerInfoUpdateHandler.Handle(updateDto, cancellationToken);
            return Ok(updateDto);
        }

        [HttpPost]
        [Route("~/v1/customer-info/update/level2")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerInfoLevel2UpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Level2(CustomerInfoLevel2UpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _c0_customerInfoUpdateHandler.Handle(updateDto, cancellationToken);
            return Ok(updateDto);
        }

        [HttpPost]
        [Route("~/v1/customer-info/get-by-billid")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerInfoGetDto>), StatusCodes.Status200OK)]
        [ActionName("Level1")]
        public async Task<IActionResult> Level1FromCustomerInfo1(SearchInput input, CancellationToken cancellationToken)
        {
            CustomerInfoGetDto customerInfo = await _c1_customerInfoGetByBillIdHandler.Handle(input, cancellationToken);
            return Ok(customerInfo);
        }
    }
}
