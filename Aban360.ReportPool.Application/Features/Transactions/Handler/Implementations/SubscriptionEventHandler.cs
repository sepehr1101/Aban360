using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Transactions.Handler.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.Transactions.Handler.Implementations
{
    internal sealed class SubscriptionEventHandler : ISubscriptionEventHandler
    {
        private readonly ISubscriptionEventQueryService _subscriptionEventQueryService;
        private readonly ISubscriptionEventWithLastDbQueryService _subscriptionEventWithLastDbQueryService;
        private readonly IBillQueryService _billQueryService;
        private readonly ICustomerInfoQueryService _customerInfoQueryService;
        public SubscriptionEventHandler(
            ISubscriptionEventQueryService subscriptionEventQueryService,
            ISubscriptionEventWithLastDbQueryService subscriptionEventWithLastDbQueryService,
            IBillQueryService billQueryService,
            ICustomerInfoQueryService customerInfoQueryService)
        {
            _subscriptionEventQueryService = subscriptionEventQueryService;
            _subscriptionEventQueryService.NotNull(nameof(subscriptionEventQueryService));

            _subscriptionEventWithLastDbQueryService = subscriptionEventWithLastDbQueryService;
            _subscriptionEventWithLastDbQueryService.NotNull(nameof(subscriptionEventWithLastDbQueryService));

            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));

            _customerInfoQueryService = customerInfoQueryService;
            _customerInfoQueryService.NotNull(nameof(customerInfoQueryService));
        }

        public async Task<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>> Handle(string input, string fromDate)
        {
            bool hasBillId = await _billQueryService.HasBillId(input);
            if (!hasBillId)
                throw new InvalidBillIdException(ExceptionLiterals.BillIdNotFound);

            ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto> result = await _subscriptionEventQueryService.GetEventsSummaryDtos(input, fromDate);
            return result;
        }
        public async Task<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>> HandleWithLastDb(string input, string fromDate)
        {
            bool hasBillId = await _billQueryService.HasBillId(input);
            if (!hasBillId)
                throw new InvalidBillIdException(ExceptionLiterals.BillIdNotFound);

            ZoneIdAndCustomerNumberOutputDto zoneIdAndCustomerNumber = await _customerInfoQueryService.GetZoneIdAndCustomerNumber(input);
            CardexInputDto cardexInfo = new(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber, fromDate);
            ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto> result = await _subscriptionEventWithLastDbQueryService.GetEventsSummaryDtos(cardexInfo);
            return result;
        }
    }
}
