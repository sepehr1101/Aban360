using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Transactions.Handler.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.Transactions.Handler.Implementations
{
    internal sealed class SubscriptionEventHandler : ISubscriptionEventHandler
    {
        private readonly ISubscriptionEventQueryService _subscriptionEventQueryService;
        public SubscriptionEventHandler(ISubscriptionEventQueryService subscriptionEventQueryService)
        {
            _subscriptionEventQueryService = subscriptionEventQueryService;
            _subscriptionEventQueryService.NotNull(nameof(subscriptionEventQueryService));
        }

        public async Task<ReportOutput<EventsSummaryOutputHeaderDto, EventsSummaryOutputDataDto>> Handle(string input)
        {
            ReportOutput<EventsSummaryOutputHeaderDto, EventsSummaryOutputDataDto> result = await _subscriptionEventQueryService.GetEventsSummaryDtos(input);
            return result;
        }
    }
}
