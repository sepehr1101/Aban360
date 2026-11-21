using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Services;
public interface IBillReturnLookupsService
{
    Task<IEnumerable<BillsCanReturnOutputDto>> BillToReturnListGetAsync(SearchInput input, IAppUser appUser, CancellationToken cancellationToken);
    Task<IEnumerable<ReturnedBillsOutputDto>> ReturedBillAfterDateLockGetAsync(int zoneId, IAppUser appUser, CancellationToken cancellationToken);
    Task<ReportOutput<UnconfirmedBillReturnHeaderOutputDto, UnconfirmedBillReturnDataOutputDto>> UnconfirmedBillReturnGetByBillIdAsync(string billId, IAppUser appUser, CancellationToken cancellationToken);
    Task<ReportOutput<UnconfirmedBillReturnHeaderOutputDto, UnconfirmedBillReturnDataOutputDto>> UnconfirmedBillReturnGetByZoneAsync(int zoneId, IAppUser appUser, CancellationToken cancellationToken);
}

internal sealed class BillReturnLookupsService : IBillReturnLookupsService
{
    private readonly ICommonMemberQueryService _customerInfoService;
    private readonly ICommonZoneService _zoneService;
    private readonly IBedBesQueryService _billQueryService;
    private readonly IBedBesQueryService _bedBedQueryService;
    private readonly IAutoBackQueryService _autoBackQueryService;
    private readonly ICommonMemberQueryService _commonMemberQueryService;
    private readonly ICommonZoneService _commonZoneService;
    public BillReturnLookupsService(ICommonMemberQueryService customerInfoService, ICommonZoneService zoneService, IBedBesQueryService billQueryService, IAutoBackQueryService autoBackQueryService)
    {
        _customerInfoService = customerInfoService;
        _customerInfoService.NotNull(nameof(customerInfoService));
        _zoneService = zoneService;
        _zoneService.NotNull(nameof(zoneService));
        _billQueryService = billQueryService;
        _billQueryService.NotNull(nameof(billQueryService));
        _bedBedQueryService = billQueryService;
        _bedBedQueryService.NotNull(nameof(billQueryService));
        _autoBackQueryService = autoBackQueryService;
        _autoBackQueryService.NotNull(nameof(autoBackQueryService));
        _commonMemberQueryService = customerInfoService;
        _commonMemberQueryService.NotNull(nameof(customerInfoService));
        _commonZoneService = zoneService;
        _commonZoneService.NotNull(nameof(zoneService));
    }

    public async Task<IEnumerable<BillsCanReturnOutputDto>> BillToReturnListGetAsync(SearchInput input, IAppUser appUser, CancellationToken cancellationToken)
    {
        ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _customerInfoService.Get(input.Input);
        await _zoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);
        ReturnBillSearchDto returnedBillSearchDto = new(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber);
        IEnumerable<BillsCanReturnOutputDto> billsCanReturned = await _billQueryService.GetToReturned(returnedBillSearchDto);
        return billsCanReturned;
    }

    public async Task<IEnumerable<ReturnedBillsOutputDto>> ReturedBillAfterDateLockGetAsync(int zoneId, IAppUser appUser, CancellationToken cancellationToken)
    {
        await _zoneService.IsUserInZone(appUser, zoneId);
        IEnumerable<ReturnedBillsOutputDto> result = await _bedBedQueryService.Returned(zoneId);
        return result;
    }

    public async Task<ReportOutput<UnconfirmedBillReturnHeaderOutputDto, UnconfirmedBillReturnDataOutputDto>> UnconfirmedBillReturnGetByBillIdAsync(string billId, IAppUser appUser, CancellationToken cancellationToken)
    {
        string title = ReportLiterals.UnconfirmedBillReturn;
        ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(billId);
        await _commonZoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);
        IEnumerable<UnconfirmedBillReturnDataOutputDto> data = await _autoBackQueryService.GetUnconfirmed(zoneIdAndCustomerNumber, billId);
        UnconfirmedBillReturnHeaderOutputDto header = new()
        {
            RecordCount = data?.Count() ?? 0,
            Title = title,
            Amount = data?.Sum(u => u.Amount) ?? 0,
        };
        return new ReportOutput<UnconfirmedBillReturnHeaderOutputDto, UnconfirmedBillReturnDataOutputDto>(title, header, data);
    }

    public async Task<ReportOutput<UnconfirmedBillReturnHeaderOutputDto, UnconfirmedBillReturnDataOutputDto>> UnconfirmedBillReturnGetByZoneAsync(int zoneId, IAppUser appUser, CancellationToken cancellationToken)
    {
        string title = ReportLiterals.UnconfirmedBillReturn;
        await _commonZoneService.IsUserInZone(appUser, zoneId);
        IEnumerable<UnconfirmedBillReturnDataOutputDto> data = await UnconfirmedBillReturnGetByZoneGetDate(zoneId);
        UnconfirmedBillReturnHeaderOutputDto header = new()
        {
            RecordCount = data?.Count() ?? 0,
            Title = title,
            Amount = data?.Sum(u => u.Amount) ?? 0,
        };
        return new ReportOutput<UnconfirmedBillReturnHeaderOutputDto, UnconfirmedBillReturnDataOutputDto>(title, header, data);
    }

    private async Task<IEnumerable<UnconfirmedBillReturnDataOutputDto>> UnconfirmedBillReturnGetByZoneGetDate(int zoneId)
    {
        IEnumerable<UnconfirmedBillReturnDataOutputDto> data = await _autoBackQueryService.GetUnconfirmed(zoneId);
        foreach (var item in data)
        {
            item.BillId = (await _commonMemberQueryService.Get(new ZoneIdAndCustomerNumber(item.ZoneId, item.CustomerNumber))).BillId;
        }

        return data;
    }
}
