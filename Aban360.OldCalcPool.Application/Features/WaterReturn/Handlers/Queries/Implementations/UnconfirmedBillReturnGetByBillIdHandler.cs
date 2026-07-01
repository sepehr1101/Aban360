using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Implementations
{
    internal sealed class UnconfirmedBillReturnGetByBillIdHandler : IUnconfirmedBillReturnGetByBillIdHandler
    {
        private readonly IAutoBackQueryService _autoBackQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        public UnconfirmedBillReturnGetByBillIdHandler(
            IAutoBackQueryService autoBackQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneService)
        {
            _autoBackQueryService = autoBackQueryService;
            _autoBackQueryService.NotNull(nameof(autoBackQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<ReportOutput<UnconfirmedBillReturnHeaderOutputDto, UnconfirmedBillReturnDataOutputDto>> Handle(string billId, IAppUser appUser, CancellationToken cancellationToken)
        {
            string title = ReportLiterals.UnconfirmedBillReturn;
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(billId);
            await _commonZoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);
            IEnumerable<UnconfirmedBillReturnDataOutputDto> data = await _autoBackQueryService.GetUnconfirmed(zoneIdAndCustomerNumber,billId);
            UnconfirmedBillReturnHeaderOutputDto header = new()
            {
                RecordCount = data?.Count() ?? 0,
                Title = title,
                Amount = data?.Sum(u => u.Amount) ?? 0,
            };

            return new ReportOutput<UnconfirmedBillReturnHeaderOutputDto, UnconfirmedBillReturnDataOutputDto>(title, header, data);
        }
    }
}
