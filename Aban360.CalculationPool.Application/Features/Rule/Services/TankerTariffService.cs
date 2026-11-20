using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Rule.Services;
public interface ITankerTariffService
{
    Task<IEnumerable<TankerTariffGetDto>> TankerTariffGetAllAsync(CancellationToken cancellationToken);
}

internal sealed class TankerTariffService : ITankerTariffService
{
    public TankerTariffService(ITankerTariffQueryService queryService)
    {
        _queryService = queryService;
        _queryService.NotNull(nameof(queryService));
    }

    private readonly ITankerTariffQueryService _queryService;
    public async Task<IEnumerable<TankerTariffGetDto>> TankerTariffGetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<TankerTariffGetDto> result = await _queryService.Get();
        return result;
    }
}
