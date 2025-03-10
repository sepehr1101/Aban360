using Aban360.MeterPool.Domain.Features.Management.Entities;

namespace Aban360.MeterPool.Persistence.Features.Management.Queries.Contracts
{
    public interface ICounterStateQueryService
    {
        Task<CounterState> Get(short id);
        Task<ICollection<CounterState>> Get();
    }
}
