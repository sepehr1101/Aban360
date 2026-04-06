using Aban360.Common.BaseEntities;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Transactions.Handler.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;

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

        public async Task<ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto>> Handle(string input,CancellationToken cancellationToken)
        {
            ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto> result = await _branchEventSummaryQueryService.Get(input);
            if (CanSetRamained(result))
            {
                result.ReportHeader.Remained = result.ReportData.Sum(data => data.DebtAmount) 
                                               - result.ReportData.Sum(data => data.CreditAmount)
                                               - result.ReportData.Sum(data=> data.DiscountAmount);
            }
            return result;
        }
        public async Task<ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto>> HandleWithLastDb(string input,string fromDate,CancellationToken cancellationToken)
        {
            bool hasBillId = await _billQueryService.HasBillId(input);
            if (!hasBillId)
                throw new InvalidBillIdException(ExceptionLiterals.BillIdNotFound);

            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(input);
            CardexInputDto cardexInfo = new(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber, fromDate);

            ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto> result = await _branchEventWithLastDbQueryService.Get(cardexInfo);
            if (CanSetRamained(result))
            {
                result.ReportHeader.Remained = result.ReportData.Sum(data => data.DebtAmount) 
                                               - result.ReportData.Sum(data => data.CreditAmount)
                                               - result.ReportData.Sum(data=> data.DiscountAmount);
            }
            return result;
        }
        private bool CanSetRamained(ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto> result)
        {
            return
                result is not null &&
                result.ReportData is not null &&
                result.ReportData.Any() &&
                result.ReportHeader is not null;
        }
    }
}
