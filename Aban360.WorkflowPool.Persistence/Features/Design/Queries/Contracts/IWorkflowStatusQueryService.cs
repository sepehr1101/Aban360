using Aban360.WorkflowPool.Domain.Features.Design.Entities;

namespace Aban360.WorkflowPool.Persistence.Features.Design.Queries.Contracts
{
    public interface IWorkflowStatusQueryService
    {
        bool AnySync();
        Task<bool> Any();
        Task<ICollection<WorkflowStatus>> Get();
    }
}