using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Implementations
{
    public class SubscriptionTypeDeleteHandler : ISubscriptionTypeDeleteHandler
    {
        private readonly ISubscriptionTypeCommandService _subscriptionTypeCommandService;
        private readonly ISubscriptionTypeQueryService _subscriptionTypeQueryService;
        public SubscriptionTypeDeleteHandler(
            ISubscriptionTypeCommandService subscriptionTypeCommandService,
            ISubscriptionTypeQueryService subscriptionTypeQueryService)
        {
            _subscriptionTypeCommandService = subscriptionTypeCommandService;
            _subscriptionTypeCommandService.NotNull(nameof(_subscriptionTypeCommandService));

            _subscriptionTypeQueryService = subscriptionTypeQueryService;
            _subscriptionTypeQueryService.NotNull(nameof(_subscriptionTypeQueryService));
        }

        public async Task Handle(SubscriptionTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var subscriptionType = await _subscriptionTypeQueryService.Get(deleteDto.Id);
            await _subscriptionTypeCommandService.Remove(subscriptionType);
        }
    }
}
