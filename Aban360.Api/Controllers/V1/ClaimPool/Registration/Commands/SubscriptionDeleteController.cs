using Aban360.ClaimPool.Application.Features.Registration.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Registration.Dto.Command;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Registration.Commands
{
    [Route("v1/subscription")]
    public class subscriptionDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISubscriptionDeleteHandler _subscriptionHandler;
        public subscriptionDeleteController(
            IUnitOfWork uow,
            ISubscriptionDeleteHandler subscriptionHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _subscriptionHandler = subscriptionHandler;
            _subscriptionHandler.NotNull(nameof(subscriptionHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] SubscriptionDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _subscriptionHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
