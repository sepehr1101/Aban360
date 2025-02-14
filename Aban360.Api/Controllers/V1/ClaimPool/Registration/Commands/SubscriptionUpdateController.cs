using Aban360.ClaimPool.Application.Features.Registration.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Registration.Dto.Command;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Registration.Commands
{
    [Route("v1/subscription")]
    public class subscriptionUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISubscriptionUpdateHandler _subscriptionHandler;
        public subscriptionUpdateController(
            IUnitOfWork uow,
            ISubscriptionUpdateHandler subscriptionHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _subscriptionHandler = subscriptionHandler;
            _subscriptionHandler.NotNull(nameof(subscriptionHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] SubscriptionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _subscriptionHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
