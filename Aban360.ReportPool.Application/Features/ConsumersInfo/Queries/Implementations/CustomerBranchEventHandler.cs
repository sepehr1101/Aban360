using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    internal sealed class CustomerBranchEventHandler : ICustomerBranchEventHandler
    {
        private readonly ISubscriptionEventQueryService _subscriptionEventQueryService;
        public CustomerBranchEventHandler(ISubscriptionEventQueryService subscriptionEventQueryService)
        {
            _subscriptionEventQueryService = subscriptionEventQueryService;
            _subscriptionEventQueryService.NotNull(nameof(_subscriptionEventQueryService));
        }

        public async Task<IEnumerable<BranchEventsDto>> Handle(string billId, CancellationToken cancellationToken)
        {
            IEnumerable<BranchEventsDto> branchEventsDtos = await _subscriptionEventQueryService.GetBranchEventDtos(billId);
            return branchEventsDtos;
        }

    }
}