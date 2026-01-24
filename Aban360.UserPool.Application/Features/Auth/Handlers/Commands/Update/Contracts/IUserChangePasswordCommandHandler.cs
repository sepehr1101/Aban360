using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts
{
    public interface IUserChangePasswordCommandHandler
    {
        Task Handle(ChangePasswordInput changePasswordInput, Guid userId, CancellationToken cancellationToken);
    }
}
