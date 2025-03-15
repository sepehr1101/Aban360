using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("v1/subscription-type")]
    public class SubscriptionTypeGetSingleController : BaseController
    {
        private readonly ISubscriptionTypeGetSingleHandler _subscriptionTypeGetSingleHandler;
        public SubscriptionTypeGetSingleController(ISubscriptionTypeGetSingleHandler subscriptionTypeGetSingleHandler)
        {
            _subscriptionTypeGetSingleHandler = subscriptionTypeGetSingleHandler;
            _subscriptionTypeGetSingleHandler.NotNull(nameof(subscriptionTypeGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(SubscriptionTypeEnum id, CancellationToken cancellationToken)
        {
            SubscriptionTypeGetDto subscriptionTypes = await _subscriptionTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(subscriptionTypes);
        }
    }

}
