using Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands;

namespace Aban360.WorkflowPool.Application.Features.Design.Handlers.Commands.Update.Contracts
{
    public interface IStateUpdateHandler
    {
        Task Handle(StateUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
