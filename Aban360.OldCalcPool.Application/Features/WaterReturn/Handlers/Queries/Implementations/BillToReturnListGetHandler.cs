using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Implementations
{
    internal sealed class BillToReturnListGetHandler : IBillToReturnListGetHandler
    {
        private readonly ICommonMemberQueryService _customerInfoService;
        private readonly ICommonZoneService _zoneService;
        private readonly IBedBesQueryService _billQueryService;
        public BillToReturnListGetHandler(
            ICommonMemberQueryService customerInfoService,
            ICommonZoneService zoneService,
            IBedBesQueryService billQueryService)
        {
            _customerInfoService = customerInfoService;
            _customerInfoService.NotNull(nameof(customerInfoService));

            _zoneService = zoneService;
            _zoneService.NotNull(nameof(zoneService));

            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));
        }

        public async Task<IEnumerable<BillsCanReturnOutputDto>> Handle(SearchInput input,IAppUser appUser, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _customerInfoService.Get(input.Input);
            await _zoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);
            ReturnBillSearchDto returnedBillSearchDto = new(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber);

            IEnumerable<BillsCanReturnOutputDto> billsCanReturned = await _billQueryService.GetToReturned(returnedBillSearchDto);
            return billsCanReturned;
        }
    }
}
