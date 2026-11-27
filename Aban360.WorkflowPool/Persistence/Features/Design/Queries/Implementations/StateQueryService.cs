using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Domain.Features.Design.Entities;
using Aban360.WorkflowPool.Persistence.Contexts.Contracts;
using Aban360.WorkflowPool.Persistence.Features.Design.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.WorkflowPool.Persistence.Features.Design.Queries.Implementations
{
    internal sealed class StateQueryService : IStateQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<State> _state;
        public StateQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _state = _uow.Set<State>();
            _state.NotNull(nameof(_state));
        }

        public async Task<State> Get(int id)
        {
            return await _uow.FindOrThrowAsync<State>(id);
        }

        public async Task<ICollection<State>> Get()
        {
            return await _state.ToListAsync();
        }
    }

}
