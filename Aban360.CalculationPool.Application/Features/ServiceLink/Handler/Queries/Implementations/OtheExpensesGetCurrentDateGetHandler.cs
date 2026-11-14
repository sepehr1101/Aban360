using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Base;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Implementations
{
    internal sealed class OtheExpensesGetCurrentDateGetHandler : IOtheExpensesGetCurrentDateGetHandler
    {
        private readonly IConnectDisconnectQueryService _connectDisconnectQueryService;
        private readonly IKartQueryService _kartQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private string _title = ReportLiterals.ServiceLinkOtherExpenses;
        public OtheExpensesGetCurrentDateGetHandler(
            IConnectDisconnectQueryService connectDisconnectQueryService,
            IKartQueryService kartQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneService)
        {
            _connectDisconnectQueryService = connectDisconnectQueryService;
            _connectDisconnectQueryService.NotNull(nameof(connectDisconnectQueryService));

            _kartQueryService = kartQueryService;
            _kartQueryService.NotNull(nameof(kartQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<ReportOutput<OtherExpensesHeaderOutputDto, OtherExpensesDataOutputDto>> Handle(int id, IAppUser appUser, CancellationToken cancellationToken)
        {
            ConnectDisconnectGetDto connectDisconnectInfo = await _connectDisconnectQueryService.Get(id, ReportLiterals.ConnectId);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(connectDisconnectInfo.BillId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);
            await _commonZoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);

            IEnumerable<KartGetDto> kartsInfo = await _kartQueryService.Get(zoneIdAndCustomerNumber.CustomerNumber, zoneIdAndCustomerNumber.ZoneId, connectDisconnectInfo.CommandDateJalali);
            return GetResult(memberInfo, kartsInfo);
        }
        private ReportOutput<OtherExpensesHeaderOutputDto, OtherExpensesDataOutputDto> GetResult(MemberInfoGetDto memberInfo, IEnumerable<KartGetDto> kartsInfo)
        {
            string paymentId = TransactionIdGenerator.GeneratePaymentId(kartsInfo?.FirstOrDefault()?.TotalServicesAmount ?? 0, memberInfo.BillId, $"{CommonLiterals.BranchPayIdUniqueCode}00");

            OtherExpensesHeaderOutputDto header = new()
            {
                ZoneId = memberInfo.ZoneId,
                ZoneTitle = memberInfo.ZoneTitle,
                CustomerNumber = memberInfo.CustomerNumber,
                BillId = memberInfo.BillId,
                FirstName = memberInfo.FirstName,
                Surname = memberInfo.Surname,
                FullName = memberInfo.FullName,
                UsageId = memberInfo.UsageId,
                UsageTitle = memberInfo.UsageTitle,
                MobileNumber = memberInfo.MobileNumber,
                PaymentId = paymentId,
                TrackNumber = kartsInfo?.FirstOrDefault()?.StringTrackNumber ?? string.Empty,
                FinalAmount = kartsInfo?.Sum(s => s.Amount) ?? 0,
                Title = _title,
                RecordCount = kartsInfo?.Count() ?? 0,
            };
            ICollection<OtherExpensesDataOutputDto> data = new List<OtherExpensesDataOutputDto>();
            foreach (var item in kartsInfo)
            {
                data.Add(new OtherExpensesDataOutputDto(item.AmountItemId, item.AmountItemTitle, item.Amount));
            }
            return new ReportOutput<OtherExpensesHeaderOutputDto, OtherExpensesDataOutputDto>(_title, header, data);
        }
    }
}
