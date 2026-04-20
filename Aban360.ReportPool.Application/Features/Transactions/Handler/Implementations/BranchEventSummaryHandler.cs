using Aban360.Common.BaseEntities;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;
using Aban360.ReportPool.Application.Features.Transactions.Handler.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;
using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Application.Features.Transactions.Handler.Implementations
{
    internal sealed class BranchEventSummaryHandler : IBranchEventSummaryHandler
    {
        private readonly IBranchEventSummaryQueryService _branchEventSummaryQueryService;
        private readonly IBranchEventSummaryWithLastDbQueryService _branchEventWithLastDbQueryService;
        private readonly IBillQueryService _billQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        public BranchEventSummaryHandler(
            IBranchEventSummaryQueryService branchEventSummaryQueryService,
            IBranchEventSummaryWithLastDbQueryService branchEventWithLastDbQueryService,
            IBillQueryService billQueryService,
            ICommonMemberQueryService commonMemberQueryService)
        {
            _branchEventSummaryQueryService = branchEventSummaryQueryService;
            _branchEventSummaryQueryService.NotNull(nameof(branchEventSummaryQueryService));

            _branchEventWithLastDbQueryService = branchEventWithLastDbQueryService;
            _branchEventWithLastDbQueryService.NotNull(nameof(billQueryService));

            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));
        }

        public async Task<ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto>> Handle(CardexInput input, CancellationToken cancellationToken)
        {
            ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto> result = await _branchEventSummaryQueryService.Get(input.Input);

            if (CanSetRamained(result))
            {
                result.ReportHeader.Remained = result.ReportData.Sum(data => data.DebtAmount)
                                              - result.ReportData.Sum(data => data.CreditAmount)
                                              - result.ReportData.Sum(data => data.DiscountAmount);
            }
            result.ReportHeader.RowCount = result.ReportData?.Count() ?? 0;

            return string.IsNullOrWhiteSpace(input.FromDateJalali) ? result : SetFromDate(result, input.FromDateJalali);
        }
        public async Task<ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto>> HandleWithLastDb(CardexInput input, CancellationToken cancellationToken)
        {
            bool hasBillId = await _billQueryService.HasBillId(input.Input);
            if (!hasBillId)
                throw new InvalidBillIdException(ExceptionLiterals.BillIdNotFound);

            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(input.Input);
            CardexInputDto cardexInfo = new(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber, input.FromDateJalali);

            ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto> result = await _branchEventWithLastDbQueryService.Get(cardexInfo);
            if (CanSetRamained(result))
            {
                result.ReportHeader.Remained = result.ReportData.Sum(data => data.DebtAmount)
                                              - result.ReportData.Sum(data => data.CreditAmount)
                                              - result.ReportData.Sum(data => data.DiscountAmount);
            }
            result.ReportHeader.RowCount = result.ReportData?.Count() ?? 0;

            return string.IsNullOrWhiteSpace(input.FromDateJalali) ? result : SetFromDate(result, input.FromDateJalali);
        }
        private bool CanSetRamained(ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto> result)
        {
            return
                result is not null &&
                result.ReportData is not null &&
                result.ReportData.Any() &&
                result.ReportHeader is not null;
        }
        private ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto> SetFromDate(ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto> input, string fromDate)
        {
            ICollection<BranchEventSummaryDataOutputDto> data = input.ReportData.Where(r => r.RegisterDateJalali.CompareTo(fromDate) >= 0).ToList();
            BranchEventSummaryDataOutputDto? previousFromDateData = input.ReportData.Where(r => r.RegisterDateJalali.CompareTo(fromDate) < 0).OrderByDescending(r => r.RegisterDateJalali).FirstOrDefault();

            string dateOnly = ConvertDate.JalaliToGregorian(fromDate);
            DateTime datetime = ConvertDate.GregorianStringToDateTime(dateOnly);
            string yesterdayDateJalali = datetime.AddDays(-1).ToShortPersianDateString();

            if (previousFromDateData is not null)
            {
                BranchEventSummaryDataOutputDto latestRecords = new()
                {
                    UsageTitle = string.Empty,
                    UsageId = 0,
                    TrackNumber = string.Empty,
                    RegisterDateJalali = yesterdayDateJalali,
                    DebtAmount = 0,
                    CreditAmount = 0,
                    Remained = previousFromDateData.Remained,
                    BankDateJalali = string.Empty,
                    BankName = string.Empty,
                    Description = string.Empty,
                    BankCode = 0,
                    DiscountAmount = 0,
                    DiscountTitle = "مانده ابتدا",
                    AmountAfterDiscount = 0
                };
                data.Add(latestRecords);
            }
            input.ReportHeader.RowCount = data?.Count() ?? 0;

            return new ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto>(ReportLiterals.BranchEventSummary, input.ReportHeader, data.OrderBy(r => r.RegisterDateJalali));

        }
    }
}
