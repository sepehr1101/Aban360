using Aban360.WorkflowPool.Domain.Features.Assignment.Dto.Commands;

namespace Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Commands.Delete.Contracts
{
    public interface IWorkflowDeleteHandler
    {
        Task Handle(WorkflowDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
