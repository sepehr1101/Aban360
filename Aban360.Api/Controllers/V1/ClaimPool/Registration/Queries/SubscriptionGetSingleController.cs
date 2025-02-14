using Aban360.ClaimPool.Application.Features.Registration.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Registration.Queries
{
    [Route("v1/subscription")]
    public class subscriptionGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISubscriptionGetSingleHandler _subscriptionHandler;
        public subscriptionGetSingleController(
            IUnitOfWork uow,
            ISubscriptionGetSingleHandler subscriptionHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _subscriptionHandler = subscriptionHandler;
            _subscriptionHandler.NotNull(nameof(subscriptionHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            var subscription = await _subscriptionHandler.Handle(id, cancellationToken);
            return Ok(subscription);
        }
    }
}
