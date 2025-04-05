using Aban360.WorkflowPool.Domain.Features.Design;

namespace Aban360.WorkflowPool.Persistence.Features.Design.Commands.Contracts
{
    public interface IWorkflowStatusCommandService
    {
        Task Add(WorkflowStatus workflowStatus);
        void AddSync(ICollection<WorkflowStatus> workflowStatuses);
    }
}