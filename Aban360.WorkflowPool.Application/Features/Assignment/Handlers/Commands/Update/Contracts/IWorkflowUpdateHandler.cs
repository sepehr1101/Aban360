using Aban360.WorkflowPool.Domain.Features.Assignment.Dto.Commands;

namespace Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Commands.Update.Contracts
{
    public interface IWorkflowUpdateHandler
    {
        Task Handle(WorkflowUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
