using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/subscription-assignment")]
    public class SubscriptionAssignmentUpdateController : ControllerBase
    {
        private readonly ISubscriptionAssignmentUpdateHandler _updateHandler;
        public SubscriptionAssignmentUpdateController(ISubscriptionAssignmentUpdateHandler updateHandler)
        {
            _updateHandler = updateHandler;
            _updateHandler.NotNull(nameof(_updateHandler));
        }

        [HttpGet, HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionAssignmentUpdateDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromBody] SubscriptionAssignmentUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _updateHandler.Handle(updateDto, cancellationToken);
            return Ok(updateDto);
        }
    }
}
