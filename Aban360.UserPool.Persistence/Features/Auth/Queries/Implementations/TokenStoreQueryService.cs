using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Implementations
{
    internal sealed class TokenStoreQueryService : ITokenStoreQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UserToken> _userTokens;
        public TokenStoreQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _userTokens = _uow.Set<UserToken>();
            _userTokens.NotNull(nameof(_userTokens));
        }
        public async Task<UserToken?> Get(string refreshTokenHash)
        {
            return await _userTokens
                .AsNoTracking()
                .Include(u => u.User)
                .FirstOrDefaultAsync(u =>
                    u.RefreshTokenIdHash == refreshTokenHash &&
                    u.RefreshTokenExpiresDateTime>=DateTime.Now);
        }
        public async Task<bool> IsValid(string accessToken, Guid userId)
        {
            var accessTokenHash = await SecurityOperations.GetSha512Hash(accessToken);
            var userToken = await _userTokens
                .FirstOrDefaultAsync(userToken=>
                    userToken.AccessTokenHash == accessTokenHash &&
                    userToken.UserId == userId);
            return userToken?.AccessTokenExpiresDateTime >= DateTime.Now;
        }
    }
}
