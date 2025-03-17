using Aban360.ClaimPool.Domain.Features.Draff.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Draff.Handlers.Commands.Create.Contracts
{
    public interface IRequestUserCreateHandler
    {
        Task Handle(RequestUserCommandDto createDto, CancellationToken cancellationToken);
    }
}
