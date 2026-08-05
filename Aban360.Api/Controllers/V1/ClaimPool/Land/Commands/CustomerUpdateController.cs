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
        private readonly ICustomerBranchTypeToNormalUpdateHandler _branchTypeToNormalUpdateHandler;
        private readonly ICustomerDeletionStateUpdateHandler _customerDeletionStateUpdateHandler;
        public CustomerUpdateController(
            ICustomerUpdateHandler customerUpdateHandler,
            ICustomerBranchTypeToNormalUpdateHandler branchTypeToNormalUpdateHandler,
            ICustomerDeletionStateUpdateHandler customerDeletionStateUpdateHandler)
        {
            _customerUpdateHandler = customerUpdateHandler;
            _customerUpdateHandler.NotNull(nameof(customerUpdateHandler));

            _branchTypeToNormalUpdateHandler = branchTypeToNormalUpdateHandler;
            _branchTypeToNormalUpdateHandler.NotNull(nameof(branchTypeToNormalUpdateHandler));

            _customerDeletionStateUpdateHandler = customerDeletionStateUpdateHandler;
            _customerDeletionStateUpdateHandler.NotNull(nameof(customerDeletionStateUpdateHandler));
        }

        [HttpGet, HttpPost]
        [Route("update-full")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateFull([FromBody] CustomerUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _customerUpdateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpGet, HttpPost]
        [Route("update-estate")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerEstateUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> EstateUpdate([FromBody] CustomerEstateUpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _customerUpdateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpGet, HttpPost]
        [Route("update-technical")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerTechnicalUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> TechnicalUpdate([FromBody] CustomerTechnicalUpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _customerUpdateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpGet, HttpPost]
        [Route("update-mobile")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerMobileUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateMobile([FromBody] CustomerMobileUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _customerUpdateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpGet, HttpPost]
        [Route("set-construction-type")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerBranchTypeUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetConstructionType([FromBody] CustomerBranchTypeUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _customerUpdateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpGet, HttpPost]
        [Route("set-normal-branch-type")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BranchTypeToNormalUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetNormalBranchType([FromBody] BranchTypeToNormalUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _branchTypeToNormalUpdateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }
        
        [HttpGet, HttpPost]
        [Route("update-deletion-state")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerDeletionStateUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateDeletionState([FromBody] CustomerDeletionStateUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _customerDeletionStateUpdateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }
    }
}
