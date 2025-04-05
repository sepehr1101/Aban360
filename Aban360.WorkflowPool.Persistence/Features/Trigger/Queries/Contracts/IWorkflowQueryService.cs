using Aban360.WorkflowPool.Domain.Features.Design.Entities;

namespace Aban360.WorkflowPool.Persistence.Features.Trigger.Queries.Contracts
{
    public interface IWorkflowQueryService
    {
        Task<Workflow> Get(int id);
        Task<ICollection<Workflow>> Get();
    }
}
