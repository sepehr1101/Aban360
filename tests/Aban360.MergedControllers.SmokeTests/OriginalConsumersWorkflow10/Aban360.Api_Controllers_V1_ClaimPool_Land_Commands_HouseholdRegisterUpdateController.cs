using global::Aban360.Api.Controllers.V1.ClaimPool.Land.Commands;
using Microsoft.Extensions.Logging;
using global::Aban360.Api.Controllers.V1;
using global::Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using global::Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using global::Aban360.Common.Categories.ApiResponse;
using global::Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeConsumersWorkflow10.Aban360.Api.Controllers.V1.ClaimPool.Land.Commands{
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
