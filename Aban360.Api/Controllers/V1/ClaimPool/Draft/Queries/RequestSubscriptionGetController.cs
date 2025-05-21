using Aban360.ClaimPool.Application.Features.Draft.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Queries
{
    [Route("v1/request-subscription")]
    public class RequestSubscriptionGetController : BaseController
    {
        private readonly IRequestSubscriptionGetHandler _requestSubscriptionGetHandler;
        public RequestSubscriptionGetController(IRequestSubscriptionGetHandler requestSubscriptionGetHandler)
        {
            _requestSubscriptionGetHandler = requestSubscriptionGetHandler;
            _requestSubscriptionGetHandler.NotNull(nameof(requestSubscriptionGetHandler));
        }

        [HttpPost, HttpGet]
        [Route("get/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestSubscriptionGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string billId, CancellationToken cancellationToken)
        {
            var requestSubscription = await _requestSubscriptionGetHandler.Handle(billId, cancellationToken);
            return Ok(requestSubscription);
        }
    }
}
