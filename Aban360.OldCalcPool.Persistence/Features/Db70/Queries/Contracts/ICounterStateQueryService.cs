using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;

namespace Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts
{
    public interface ICounterStateQueryService
    {
        Task<CounterStateCodeDto> Get(int id);
        Task<IEnumerable<CounterStateCodeDto>> Get();
    }
}
