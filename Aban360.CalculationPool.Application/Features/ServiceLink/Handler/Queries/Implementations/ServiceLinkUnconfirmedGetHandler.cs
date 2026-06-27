using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Implementations
{
    internal sealed class ServiceLinkUnconfirmedGetHandler : IServiceLinkUnconfirmedGetHandler
    {
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneQueryServcice;
        private readonly IKartQueryService _kartQueryService;
        private string _title = ReportLiterals.ServiceLinkUnconfirmed;
        public ServiceLinkUnconfirmedGetHandler(
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneQueryService,
            IKartQueryService kartQueryService)
        {
            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _commonZoneQueryServcice = commonZoneQueryService;
            _commonZoneQueryServcice.NotNull(nameof(commonZoneQueryService));

            _kartQueryService = kartQueryService;
            _kartQueryService.NotNull(nameof(kartQueryService));
        }

        public async Task<ReportOutput<ServiceLinkUnconfirmedHeaderOutputDto, ServiceLinkUnconfirmedDataOutputDto>> Handle(string billId, IAppUser appUser, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(billId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);
            await _commonZoneQueryServcice.IsUserInZone(appUser, memberInfo.ZoneId);

            IEnumerable<ServiceLinkUnconfirmedDataOutputDto> KartsData = await _kartQueryService.GetTodayInfoByCustomerNumber(memberInfo.CustomerNumber, memberInfo.ZoneId);
            ServiceLinkUnconfirmedHeaderOutputDto header = new()
            {
                ZoneId = zoneIdAndCustomerNumber.ZoneId,
                ZoneTitle = memberInfo.ZoneTitle,
                CustomerNumber = memberInfo.CustomerNumber,
                BillId = billId,
                RecordCount = KartsData?.Count() ?? 0,
                Title = _title
            };
            return new ReportOutput<ServiceLinkUnconfirmedHeaderOutputDto, ServiceLinkUnconfirmedDataOutputDto>(_title, header, KartsData);
        }
    }
}
