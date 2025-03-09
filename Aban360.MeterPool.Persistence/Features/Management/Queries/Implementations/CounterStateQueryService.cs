using Aban360.Common.Extensions;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Aban360.MeterPool.Persistence.Features.Management.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.MeterPool.Persistence.Features.Management.Queries.Implementations
{
    internal sealed class CounterStateQueryService : ICounterStateQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CounterState> _counterState;
        public CounterStateQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _counterState = _uow.Set<CounterState>();
            _counterState.NotNull(nameof(_counterState));
        }

        public async Task<CounterState> Get(short id)
        {
            return await _uow.FindOrThrowAsync<CounterState>(id);
        }

        public async Task<ICollection<CounterState>> Get()
        {
            return await _counterState.ToListAsync();
        }
    }
}
