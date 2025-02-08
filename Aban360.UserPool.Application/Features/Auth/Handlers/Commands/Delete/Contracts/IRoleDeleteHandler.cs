using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Delete.Contracts
{
    public interface IRoleDeleteHandler
    {
        Task Handle(RoleDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
