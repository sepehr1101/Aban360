using Aban360.Common.Extensions;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Aban360.MeterPool.Persistence.Features.Management.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.MeterPool.Persistence.Features.Management.Commands.Implementations
{
    internal sealed class CounterStateCommandService : ICounterStateCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CounterState> _counterState;
        public CounterStateCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _counterState = _uow.Set<CounterState>();
            _counterState.NotNull(nameof(_counterState));
        }

        public async Task Add(CounterState counterState)
        {
            await _counterState.AddAsync(counterState);
        }

        public async Task Remove(CounterState counterState)
        {
            _counterState.Remove(counterState);
        }
    }
}
