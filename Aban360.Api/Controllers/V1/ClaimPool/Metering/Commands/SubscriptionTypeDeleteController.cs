using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/subscription-type")]
    public class SubscriptionTypeDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISubscriptionTypeDeleteHandler _subscriptionTypeDeleteHandler;
        public SubscriptionTypeDeleteController(
            IUnitOfWork uow,
            ISubscriptionTypeDeleteHandler subscriptionTypeDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _subscriptionTypeDeleteHandler = subscriptionTypeDeleteHandler;
            _subscriptionTypeDeleteHandler.NotNull(nameof(subscriptionTypeDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SubscriptionTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _subscriptionTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
	
}
