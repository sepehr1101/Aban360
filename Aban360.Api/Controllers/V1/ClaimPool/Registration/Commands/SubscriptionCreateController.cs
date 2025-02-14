using Aban360.ClaimPool.Application.Features.Registration.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Registration.Dto.Command;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Registration.Commands
{
    [Route("v1/subscription")]
    public class SubscriptionCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISubscriptionCreateHandler _subscriptionHandler;
        public SubscriptionCreateController(
            IUnitOfWork uow,
            ISubscriptionCreateHandler subscriptionHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _subscriptionHandler = subscriptionHandler;
            _subscriptionHandler.NotNull(nameof(subscriptionHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] SubscriptionCreateDto createDto, CancellationToken cancellationToken)
        {
            await _subscriptionHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
