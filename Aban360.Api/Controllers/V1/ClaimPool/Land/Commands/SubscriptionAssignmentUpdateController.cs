using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/subscription-assignment")]
    public class SubscriptionAssignmentUpdateController : BaseController
    {
        private readonly ISubscriptionAssignmentUpdateHandler _subscriptionAssignmentHandler;
        private readonly ISubscriptionAssignmentByTrackNumberUpdateHandler _subscriptionAssigmentByTrackNumberHandler;
        public SubscriptionAssignmentUpdateController(
            ISubscriptionAssignmentUpdateHandler subscriptionAssignmentHandler,
            ISubscriptionAssignmentByTrackNumberUpdateHandler subscriptionAssigmentByTrackNumberHandler)
        {
            _subscriptionAssignmentHandler = subscriptionAssignmentHandler;
            _subscriptionAssignmentHandler.NotNull(nameof(subscriptionAssignmentHandler));

            _subscriptionAssigmentByTrackNumberHandler = subscriptionAssigmentByTrackNumberHandler;
            _subscriptionAssigmentByTrackNumberHandler.NotNull(nameof(subscriptionAssigmentByTrackNumberHandler));
        }

        [HttpGet, HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionAssignmentUpdateDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromBody] SubscriptionAssignmentUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _subscriptionAssignmentHandler.Handle(updateDto, cancellationToken);
            return Ok(updateDto);
        }

        [HttpGet, HttpPost]
        [Route("update-by-tracknumber")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionAssignmentByTrackNumberUpdateDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateByTrackNumber([FromBody] SubscriptionAssignmentByTrackNumberUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _subscriptionAssigmentByTrackNumberHandler.Handle(updateDto, CurrentUser, cancellationToken);
            return Ok(updateDto);
        }
    }
}
