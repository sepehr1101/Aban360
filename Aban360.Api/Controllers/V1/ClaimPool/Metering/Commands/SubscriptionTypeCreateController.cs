using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/subscription-type")]
    public class SubscriptionTypeCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISubscriptionTypeCreateHandler _subscriptionTypeCreateHandler;
        public SubscriptionTypeCreateController(
            IUnitOfWork uow,
            ISubscriptionTypeCreateHandler subscriptionTypeCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _subscriptionTypeCreateHandler = subscriptionTypeCreateHandler;
            _subscriptionTypeCreateHandler.NotNull(nameof(subscriptionTypeCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] SubscriptionTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _subscriptionTypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }	
}
