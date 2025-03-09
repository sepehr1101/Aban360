using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("v1/subscription-type")]
    public class SubscriptionTypeGetAllController : BaseController
    {
        private readonly ISubscriptionTypeGetAllHandler _subscriptionTypeGetAllHandler;
        public SubscriptionTypeGetAllController(ISubscriptionTypeGetAllHandler subscriptionTypeGetAllHandler)
        {
            _subscriptionTypeGetAllHandler = subscriptionTypeGetAllHandler;
            _subscriptionTypeGetAllHandler.NotNull(nameof(subscriptionTypeGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SubscriptionTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var subscriptionTypes = await _subscriptionTypeGetAllHandler.Handle(cancellationToken);
            return Ok(subscriptionTypes);
        }
    }

}
