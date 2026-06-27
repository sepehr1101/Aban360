using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;
using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class GenerateBillWithRemainedHandler : IGenerateBillWithRemainedHandler
    {
        private readonly ICommonZoneService _commonZoneService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly ICustomerInfoService _customerInfoService;
        private readonly ISubscriptionEventQueryService _subscriptionEventQueryService;
        public GenerateBillWithRemainedHandler(
            ICommonZoneService commonZoneService,
            ICommonMemberQueryService commonMemberQueryService,
            IBedBesQueryService bedBesQueryService,
            ICustomerInfoService customerInfoService,
            ISubscriptionEventQueryService subscriptionEventQueryService)
        {
            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(bedBesQueryService));

            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(bedBesQueryService));

            _customerInfoService = customerInfoService;
            _customerInfoService.NotNull(nameof(customerInfoService));

            _subscriptionEventQueryService = subscriptionEventQueryService;
            _subscriptionEventQueryService.NotNull(nameof(subscriptionEventQueryService));
        }

        public async Task<BillIssueRemainedOutputDto> Handle(string billId, IAppUser appUser, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(billId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);
            await _commonZoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);

            ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto> result = await _subscriptionEventQueryService.GetEventsSummaryDtos(billId, string.Empty);
            long amount = result.ReportHeader.Remained;
            string currentMonth = DateTime.Now.ToShortPersianDateString().Substring(5, 2);
            string paymentIdOption = $"1{currentMonth}";
            return new BillIssueRemainedOutputDto()
            {
                CustomerNumber = memberInfo.CustomerNumber,
                ZoneId = memberInfo.ZoneId,
                ZoneTitle = memberInfo.ZoneTitle,
                BillId = memberInfo.BillId,
                ReadingNumber = memberInfo.ReadingNumber,
                UsageId = memberInfo.UsageId,
                UsageTitle = memberInfo.UsageTitle,
                BranchTypeId = memberInfo.UseStateId,
                BranchTypeTitle = memberInfo.UseStateTitle,
                ContractualCapacity = memberInfo.ContractualCapacity,
                Amount = amount,
                PaymentId = TransactionIdGenerator.GeneratePaymentId(amount, billId, paymentIdOption),
            };
        }
    }
}
