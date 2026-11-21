using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Db70.Services;
public interface ICounterStateLookupService
{
    Task<CounterStateCodeDto> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<CounterStateCodeDto>> GetAllAsync(bool isReadable, CancellationToken cancellationToken);
}

internal sealed class CounterStateLookupService : ICounterStateLookupService
{
    private readonly ICounterStateQueryService _CounterStateQueryService;
    public CounterStateLookupService(ICounterStateQueryService CounterStateQueryService)
    {
        _CounterStateQueryService = CounterStateQueryService;
        _CounterStateQueryService.NotNull(nameof(CounterStateQueryService));
    }

    public async Task<CounterStateCodeDto> GetAsync(int id, CancellationToken cancellationToken)
    {
        CounterStateCodeDto result = await _CounterStateQueryService.Get(id, false);
        return result;
    }

    public async Task<IEnumerable<CounterStateCodeDto>> GetAllAsync(bool isReadable, CancellationToken cancellationToken)
    {
        IEnumerable<CounterStateCodeDto> result = await _CounterStateQueryService.Get(isReadable);
        return result;
    }
}
