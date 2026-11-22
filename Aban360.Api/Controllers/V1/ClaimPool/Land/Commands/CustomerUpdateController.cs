using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.Land.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/customer")]
    public class CustomerUpdateController : BaseController
    {
        private readonly ICustomerMaintenanceService _customerMaintenanceService;
        public CustomerUpdateController(ICustomerMaintenanceService customerMaintenanceService)
        {
            _customerMaintenanceService = customerMaintenanceService;
            _customerMaintenanceService.NotNull(nameof(customerMaintenanceService));
        }

        [HttpPost]
        [Route("update-full")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateFull([FromBody] CustomerUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _customerMaintenanceService.UpdateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("update-estate")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerEstateUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> EstateUpdate([FromBody] CustomerEstateUpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _customerMaintenanceService.UpdateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("update-technical")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerTechnicalUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> TechnicalUpdate([FromBody] CustomerTechnicalUpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _customerMaintenanceService.UpdateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("update-mobile")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerMobileUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateMobile([FromBody] CustomerMobileUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _customerMaintenanceService.UpdateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("set-construction-type")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerNecessaryInfoToUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetConstructionType([FromBody] CustomerNecessaryInfoToUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _customerMaintenanceService.UpdateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("set-normal-branch-type")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BranchTypeToNormalUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetNormalBranchType([FromBody] BranchTypeToNormalUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _customerMaintenanceService.SetNormalBranchTypeAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("update-deletion-state")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerDeletionStateUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateDeletionState([FromBody] CustomerDeletionStateUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _customerMaintenanceService.UpdateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("update-household")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerDeletionStateUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateHousehold([FromBody] CustomerHouseholdUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _customerMaintenanceService.UpdateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("update-installation-date")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterInstallationUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateInstallationDate([FromBody] MeterInstallationUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _customerMaintenanceService.UpdateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("update-discount-type")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerBranchTypeUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateDiscountType([FromBody] CustomerBranchTypeUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _customerMaintenanceService.UpdateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("update-empty-unit")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerEmptyUnitUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateEmptyUnit([FromBody] CustomerEmptyUnitUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _customerMaintenanceService.UpdateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }
    }
}
