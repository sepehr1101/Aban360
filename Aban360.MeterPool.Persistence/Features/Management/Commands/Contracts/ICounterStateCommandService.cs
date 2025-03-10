using Aban360.MeterPool.Domain.Features.Management.Entities;

namespace Aban360.MeterPool.Persistence.Features.Management.Commands.Contracts
{
    public interface ICounterStateCommandService
    {
        Task Add(CounterState counterState);
        Task Remove(CounterState counterState);
    }
}
