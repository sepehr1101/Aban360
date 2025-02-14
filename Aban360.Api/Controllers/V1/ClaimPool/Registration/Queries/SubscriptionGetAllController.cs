using Aban360.ClaimPool.Application.Features.Registration.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Registration.Queries
{
    [Route("v1/subscription")]
    public class subscriptionGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISubscriptionGetAllHandler _subscriptionHandler;
        public subscriptionGetAllController(
            IUnitOfWork uow,
            ISubscriptionGetAllHandler subscriptionHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _subscriptionHandler = subscriptionHandler;
            _subscriptionHandler.NotNull(nameof(subscriptionHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var subscription = await _subscriptionHandler.Handle(cancellationToken);
            return Ok(subscription);
        }
    }
}
