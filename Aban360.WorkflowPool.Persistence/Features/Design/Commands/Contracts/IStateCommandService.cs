using Aban360.WorkflowPool.Domain.Features.Design.Entities;

namespace Aban360.WorkflowPool.Persistence.Features.Design.Commands.Contracts
{
    public interface IStateCommandService
    {
        Task Add(State state);
        Task Add(ICollection<State> states);
        Task Remove(State state);
    }

}
