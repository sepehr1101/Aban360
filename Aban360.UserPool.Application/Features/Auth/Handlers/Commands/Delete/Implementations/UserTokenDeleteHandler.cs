using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Delete.Implementations
{
    internal sealed class UserTokenDeleteHandler : IUserTokenDeleteHandler
    {
        private readonly ITokenStoreCommandService _tokenStoreCommandService;
        public UserTokenDeleteHandler(ITokenStoreCommandService tokenStoreCommandService)
        {
            _tokenStoreCommandService = tokenStoreCommandService;
        }
        public async Task Handle(Guid userId, CancellationToken cancellationToken)
        {
           await _tokenStoreCommandService.Remove(userId);
        }
    }
}
