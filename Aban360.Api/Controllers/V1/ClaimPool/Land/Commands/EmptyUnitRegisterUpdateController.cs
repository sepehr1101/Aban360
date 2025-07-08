using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/empty-unit-register")]
    public class EmptyUnitRegisterUpdateController : BaseController
    {
        private readonly IEmptyUnitRegisterUpdateHandler _emptyUnitRegisterUpdateHandler;
        public EmptyUnitRegisterUpdateController(IEmptyUnitRegisterUpdateHandler emptyUnitRegisterUpdateHandler)
        {
            _emptyUnitRegisterUpdateHandler = emptyUnitRegisterUpdateHandler;
            _emptyUnitRegisterUpdateHandler.NotNull(nameof(emptyUnitRegisterUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EmptyUnitRegisterUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] EmptyUnitRegisterUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _emptyUnitRegisterUpdateHandler.Handle(updateDto, cancellationToken);

            return Ok(updateDto);
        }
    }
}
