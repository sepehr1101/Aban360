using Aban360.WorkflowPool.Domain.Features.Assignment.Entities;

namespace Aban360.WorkflowPool.Persistence.Features.Assignment.Commands.Contracts
{
    public interface IWorkflowCommandService
    {
        Task Add(Workflow workflow);
        Task Remove(Workflow workflow);
    }
}
