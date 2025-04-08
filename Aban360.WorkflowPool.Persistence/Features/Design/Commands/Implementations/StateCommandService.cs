using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Domain.Features.Design.Entities;
using Aban360.WorkflowPool.Persistence.Contexts.Contracts;
using Aban360.WorkflowPool.Persistence.Features.Design.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.WorkflowPool.Persistence.Features.Design.Commands.Implementations
{
    internal sealed class StateCommandService : IStateCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<State> _state;
        public StateCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _state = _uow.Set<State>();
            _state.NotNull(nameof(_state));
        }

        public async Task Add(State state)
        {
            await _state.AddAsync(state);
        }
        public async Task Add(ICollection<State> states)
        {
            await _state.AddRangeAsync(states);
        }

        public async Task Remove(State state)
        {
            _state.Remove(state);
        }
    }

}
