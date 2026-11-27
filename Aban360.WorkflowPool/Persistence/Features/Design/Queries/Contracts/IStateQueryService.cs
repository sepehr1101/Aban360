using Aban360.WorkflowPool.Domain.Features.Design.Entities;

namespace Aban360.WorkflowPool.Persistence.Features.Design.Queries.Contracts
{
    public interface IStateQueryService
    {
        Task<State> Get(int id);
        Task<ICollection<State>> Get();
    }

}
