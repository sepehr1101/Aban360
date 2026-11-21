using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Services;
public interface IBillReturnRepairService
{
    Task<RepairGetDto> RepairGetAsync(SearchInput input, CancellationToken cancellationToken);
    Task<IEnumerable<RepairGetDto>> RepairGetByBillIdAsync(SearchInput input, CancellationToken cancellationToken);
}

internal sealed class BillReturnRepairService : IBillReturnRepairService
{
    private readonly IRepairQueryService _queryService;
    public BillReturnRepairService(IRepairQueryService queryService)
    {
        _queryService = queryService;
        _queryService.NotNull(nameof(queryService));
    }

    public async Task<RepairGetDto> RepairGetAsync(SearchInput input, CancellationToken cancellationToken)
    {
        RepairGetDto result = await _queryService.Get(int.Parse(input.Input));
        return result;
    }

    public async Task<IEnumerable<RepairGetDto>> RepairGetByBillIdAsync(SearchInput input, CancellationToken cancellationToken)
    {
        IEnumerable<RepairGetDto> result = await _queryService.Get(input.Input);
        return result;
    }
}
