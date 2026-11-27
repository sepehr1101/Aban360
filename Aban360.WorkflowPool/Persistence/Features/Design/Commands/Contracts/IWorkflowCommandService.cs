using Aban360.WorkflowPool.Domain.Features.Design.Entities;

namespace Aban360.WorkflowPool.Persistence.Features.Design.Commands.Contracts
{
    public interface IWorkflowCommandService
    {
        Task Add(Workflow workflow);
        void Remove(Workflow workflow);
    }
}
