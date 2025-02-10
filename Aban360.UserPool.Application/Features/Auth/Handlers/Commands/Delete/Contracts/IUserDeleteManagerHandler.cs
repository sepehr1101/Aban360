using Aban360.UserPool.Domain.Features.Auth.Dto.Base;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Delete.Contracts
{
    public interface IUserDeleteManagerHandler
    {
        Task Handle(UserIdDto userId, CancellationToken cancellationToken);
    }
}
