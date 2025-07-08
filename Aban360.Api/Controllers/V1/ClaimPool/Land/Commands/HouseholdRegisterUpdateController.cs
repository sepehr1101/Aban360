using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/household-register")]
    public class HouseholdRegisterUpdateController : BaseController
    {
        private readonly IHouseholdRegisterUpdateHandler _householdRegisterUpdateHandler;
        public HouseholdRegisterUpdateController(IHouseholdRegisterUpdateHandler householdRegisterUpdateHandler)
        {
            _householdRegisterUpdateHandler = householdRegisterUpdateHandler;
            _householdRegisterUpdateHandler.NotNull(nameof(householdRegisterUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<HouseholdRegisterUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] HouseholdRegisterUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _householdRegisterUpdateHandler.Handle(updateDto, cancellationToken);

            return Ok(updateDto);
        }
    }
}
