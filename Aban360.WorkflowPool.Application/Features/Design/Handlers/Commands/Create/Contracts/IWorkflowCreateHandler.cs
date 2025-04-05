using Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands;

namespace Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Commands.Create.Contracts
{
    public interface IWorkflowCreateHandler
    {
        Task Handle(WorkflowCreateDto createDto, CancellationToken cancellationToken);
    }
}
