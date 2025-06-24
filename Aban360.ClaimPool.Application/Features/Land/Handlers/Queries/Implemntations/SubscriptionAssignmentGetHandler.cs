using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class SubscriptionAssignmentGetHandler : ISubscriptionAssignmentGetHandler
    {
        private readonly ISubscriptionAssignmentQueryService _subscriptionAssignmentQueryService;
        public SubscriptionAssignmentGetHandler(ISubscriptionAssignmentQueryService subscriptionAssignmentQueryService)
        {
            _subscriptionAssignmentQueryService = subscriptionAssignmentQueryService;
            _subscriptionAssignmentQueryService.NotNull(nameof(subscriptionAssignmentQueryService));
        }

        public async Task<SubscriptionAssignmentGetDto> Handle(string input, CancellationToken cancellationToken)
        {
            SubscriptionAssignmentGetDto subscriptionAssignment = await _subscriptionAssignmentQueryService.Get(input);
            return subscriptionAssignment;
        }
    }
}
