using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Transactions.Handler.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.Transactions.Handler.Implementations
{
    internal sealed class SubscriptionEventHandler : ISubscriptionEventHandler
    {
        private readonly ISubscriptionEventQueryService _subscriptionEventQueryService;
        private readonly IBillQueryService _billQueryService;
        public SubscriptionEventHandler(
            ISubscriptionEventQueryService subscriptionEventQueryService,
            IBillQueryService billQueryService)
        {
            _subscriptionEventQueryService = subscriptionEventQueryService;
            _subscriptionEventQueryService.NotNull(nameof(subscriptionEventQueryService));

            _billQueryService= billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));
        }

        public async Task<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>> Handle(string input,string fromDate)
        {
            bool hasBillId=await _billQueryService.HasBillId(input);
            if(!hasBillId)
                throw new InvalidBillIdException(ExceptionLiterals.BillIdNotFound);

            ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto> result = await _subscriptionEventQueryService.GetEventsSummaryDtos(input,fromDate);
            return result;
        }
    }
}
