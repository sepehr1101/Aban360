using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;

namespace Aban360.CalculationPool.Application.Features.Bill.Services;
public interface IBillListsService
{
    Task<ReportOutput<BillReadingListHeaderOutputDto, BillReadingListDataOutputDto>> BillReadingListGetAsync(BillReadingListInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task<IEnumerable<CollectBillsDetailGetDto>> CollectBillsDetailGetAllAsync(CancellationToken cancellationToken);
}

internal sealed class BillListsService : IBillListsService
{
    public BillListsService(IBedBesQueryService bedBesQueryService, ICommonZoneService commonZoneService, ICollectBillsDetailQueryService collectBillsDetailQueryService)
    {
        _bedBesQueryService = bedBesQueryService;
        _bedBesQueryService.NotNull(nameof(bedBesQueryService));
        _commonZoneService = commonZoneService;
        _commonZoneService.NotNull(nameof(commonZoneService));
        _collectBillsDetailQueryService = collectBillsDetailQueryService;
        _collectBillsDetailQueryService.NotNull(nameof(collectBillsDetailQueryService));
    }

    private readonly IBedBesQueryService _bedBesQueryService;
    private readonly ICommonZoneService _commonZoneService;
    private string _title = ReportLiterals.BillReadingList;
    public async Task<ReportOutput<BillReadingListHeaderOutputDto, BillReadingListDataOutputDto>> BillReadingListGetAsync(BillReadingListInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await _commonZoneService.IsUserInZone(appUser, inputDto.ZoneId);
        IEnumerable<BillReadingListDataOutputDto> data = await _bedBesQueryService.Get(inputDto);
        BillReadingListHeaderOutputDto header = new()
        {
            ZoneId = inputDto.ZoneId,
            ZoneTitle = data?.FirstOrDefault()?.ZoneTitle ?? string.Empty,
            FromReadingNumber = inputDto.FromReadingNumber,
            ToReadingNumber = inputDto.ToReadingNumber,
            RecordCount = data?.Count() ?? 0,
            CustomerCount = data?.Count() ?? 0,
            Title = _title,
        };
        ReportOutput<BillReadingListHeaderOutputDto, BillReadingListDataOutputDto> result = new(_title, header, data);
        return result;
    }

    private readonly ICollectBillsDetailQueryService _collectBillsDetailQueryService;
    public async Task<IEnumerable<CollectBillsDetailGetDto>> CollectBillsDetailGetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<CollectBillsDetailGetDto> result = await _collectBillsDetailQueryService.Get();
        return result;
    }
}
