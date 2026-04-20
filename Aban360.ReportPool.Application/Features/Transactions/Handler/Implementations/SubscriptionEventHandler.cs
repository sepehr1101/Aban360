using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;
using Aban360.ReportPool.Application.Features.Transactions.Handler.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;
using DNTPersianUtils.Core;
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

        public async Task<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>> Handle(CardexInput input,CancellationToken cancellationToken)
        {
            bool hasBillId = await _billQueryService.HasBillId(input.Input);
            if (!hasBillId)
                throw new InvalidBillIdException(ExceptionLiterals.BillIdNotFound);

            ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto> result = await _subscriptionEventQueryService.GetEventsSummaryDtos(input.Input, input.FromDateJalali);
            result.ReportHeader.RowCount = result.ReportData?.Count() ?? 0;
            return string.IsNullOrWhiteSpace(input.FromDateJalali) ? result : SetFromDate(result, input.FromDateJalali);

        }
        public async Task<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>> HandleWithLastDb(CardexInput input, CancellationToken cancellationToken)
        {
            bool hasBillId = await _billQueryService.HasBillId(input.Input);
            if (!hasBillId)
                throw new InvalidBillIdException(ExceptionLiterals.BillIdNotFound);

            ZoneIdAndCustomerNumberOutputDto zoneIdAndCustomerNumber = await _customerInfoQueryService.GetZoneIdAndCustomerNumber(input.Input);
            CardexInputDto cardexInfo = new(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber, input.FromDateJalali);
            ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto> result = await _subscriptionEventWithLastDbQueryService.GetEventsSummaryDtos(cardexInfo);
            result.ReportHeader.RowCount = result.ReportData?.Count() ?? 0;

            return string.IsNullOrWhiteSpace(input.FromDateJalali) ? result : SetFromDate(result, input.FromDateJalali);
        }
        private ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto> SetFromDate(ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto> input, string fromDate)
        {
            ICollection<WaterEventsSummaryOutputDataDto> data = input.ReportData.Where(r => r.RegisterDate.CompareTo(fromDate) >= 0).ToList();
            WaterEventsSummaryOutputDataDto? previousFromDateData = input.ReportData.Where(r => r.RegisterDate.CompareTo(fromDate) < 0).OrderByDescending(r => r.RegisterDate).FirstOrDefault();

            string dateOnly = ConvertDate.JalaliToGregorian(fromDate);
            DateTime datetime = ConvertDate.GregorianStringToDateTime(dateOnly);
            string yesterdayDateJalali = datetime.AddDays(-1).ToShortPersianDateString();

            if (previousFromDateData is not null)
            {
                WaterEventsSummaryOutputDataDto latestRecords = new()
                {
                    Id = 0,
                    CommercialUnit = 0,
                    DomesticUnit = 0,
                    OtherUnit = 0,
                    EmptyUnit = 0,
                    HouseholderNumber = 0,
                    ContractualCapacity = 0,
                    UsageSellId = 0,
                    UsageConsumptionId = 0,
                    UsageSellTitle = string.Empty,
                    UsageConsumptionTitle = string.Empty,
                    ReadingNumber = string.Empty,
                    Consumption = 0,
                    ConsumptionAverage = 0,
                    Date = string.Empty,
                    ReadingDate = string.Empty,
                    DebtAmount = 0,
                    CreditAmount = 0,
                    Remained = previousFromDateData.Remained,
                    Description = "مانده ابتدا",
                    BankTitle = string.Empty,
                    BankCode = 0,
                    BillId = previousFromDateData.BillId,
                    PreviousMeterNumber = 0,
                    NextMeterNumber = 0,
                    PreviousMeterDate = string.Empty,
                    CurrentMeterDate = string.Empty,
                    Duration = 0,
                    RegisterDate = yesterdayDateJalali,
                    PayDateJalali = string.Empty,
                    EventDateJalali = string.Empty,
                    TypeCode = 0,
                    MeterStateCode = 0,
                    BranchTypeId = 0,
                    BranchTypeTitle = string.Empty,
                    CounterStateCode = 0,
                    CounterStateTitle = string.Empty,
                };
                data.Add(latestRecords);
            }
            input.ReportHeader.RowCount = data?.Count() ?? 0;

            return new ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>(ReportLiterals.SubscriptionEventSummary, input.ReportHeader, data.OrderBy(r => r.RegisterDate));

        }
    }
}
