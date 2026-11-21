using global::Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using global::Aban360.Common.Extensions;
using global::Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using global::Aban360.UserPool.Domain.Constants;
using global::Aban360.UserPool.Domain.Features.Auth.Entities;
using global::Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
namespace BeforeServiceWorkflow10.Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Implementations{
    internal sealed class UserLogoutHandler : IUserLogoutHandler
    {
        private readonly IUserLoginQueryService _userLoginQueryService;
        public UserLogoutHandler(IUserLoginQueryService userLoginQueryService)
        {
            _userLoginQueryService = userLoginQueryService;
            _userLoginQueryService.NotNull(nameof(userLoginQueryService));
        }
        public async Task Handle(Guid userId, LogoutReasonEnum logoutReasonId)
        {
            ICollection<UserLogin> userLogins = await _userLoginQueryService.GetByUserId(userId);
            if (userLogins is null || !userLogins.Any())
            {
                return;
            }
            userLogins.ForEach(l =>
            {
                l.LogoutDateTime = DateTime.Now;
                l.LogoutReasonId = logoutReasonId;
            });
        }
    }
}
