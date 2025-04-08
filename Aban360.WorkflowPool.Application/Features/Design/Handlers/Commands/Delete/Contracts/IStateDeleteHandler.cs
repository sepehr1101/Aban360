using Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands;

namespace Aban360.WorkflowPool.Application.Features.Design.Handlers.Commands.Delete.Contracts
{
    public interface IStateDeleteHandler
    {
        Task Handle(StateDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
