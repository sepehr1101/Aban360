using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Application.Features.Auth.Services.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    internal sealed class UserTokenFindByRefreshQueryHandler : IUserTokenFindByRefreshQueryHandler
    {
        private readonly ITokenStoreQueryService _tokenStoreService;
        private readonly ITokenFactoryService _tokenFactoryService;
        public UserTokenFindByRefreshQueryHandler(
            ITokenStoreQueryService tokenStoreService,
            ITokenFactoryService tokenFactoryService)
        {
            _tokenStoreService = tokenStoreService;
            _tokenStoreService.NotNull(nameof(tokenStoreService));

            _tokenFactoryService = tokenFactoryService;
            _tokenFactoryService.NotNull(nameof(tokenFactoryService));
        }
        public async Task<UserToken?> Handle(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            //validate refreshtokendto
            string refreshTokenSerial = _tokenFactoryService.GetRefreshTokenSerial(refreshToken.Value);
            if (string.IsNullOrWhiteSpace(refreshTokenSerial))
            {
                return default;
            }
            string refreshTokenHash = await SecurityOperations.GetSha256Hash(refreshTokenSerial);
            UserToken userToken = await _tokenStoreService.Get(refreshTokenHash);
            return userToken;
        }
    }
}
