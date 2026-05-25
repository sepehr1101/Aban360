namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts
{
    public interface IUpdateUsersInRoleHandler
    {
        Task Handle(int roleId, CancellationToken cancellationToken);
    }
}