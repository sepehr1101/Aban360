using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.Land.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/empty-unit-register")]
    public class EmptyUnitRegisterUpdateController : BaseController
    {
        private readonly ICustomerMaintenanceService _customerMaintenanceService;
        public EmptyUnitRegisterUpdateController(ICustomerMaintenanceService customerMaintenanceService)
        {
            _customerMaintenanceService = customerMaintenanceService;
            _customerMaintenanceService.NotNull(nameof(customerMaintenanceService));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EmptyUnitRegisterUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] EmptyUnitRegisterUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _customerMaintenanceService.UpdateEmptyUnitAsync(updateDto, cancellationToken);
            return Ok(updateDto);
        }
    }
}
