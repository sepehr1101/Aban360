using Aban360.UserPool.Domain.Constants;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts
{
    internal interface IUserLogoutHandler
    {
        Task Handle(Guid userId, LogoutReasonEnum logoutReasonId);
    }
}