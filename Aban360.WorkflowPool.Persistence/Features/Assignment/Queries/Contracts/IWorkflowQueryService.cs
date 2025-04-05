using Aban360.WorkflowPool.Domain.Features.Assignment.Entities;

namespace Aban360.WorkflowPool.Persistence.Features.Assignment.Queries.Contracts
{
    public interface IWorkflowQueryService
    {
        Task<Workflow> Get(int id);
        Task<ICollection<Workflow>> Get();
    }
}
