using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Auth.Commands.Implementations
{
    public class TokenStoreService : ITokenStoreCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UserToken> _userTokens;

        public TokenStoreService(
            IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _userTokens = _uow.Set<UserToken>();
            _userTokens.NotNull(nameof(_userTokens));   
        }

        public async Task Add(UserToken userToken)
        {
            await _userTokens.AddAsync(userToken);
        }
        private void Remove(UserToken userToken)
        {
            _userTokens.Remove(userToken);
        }
        public async Task Remove(Guid userId)
        {            
            await _userTokens
                .Where(u => u.UserId == userId)
                .ExecuteDeleteAsync();
        }
        public async Task RemoveTokensWithSameRefreshTokenSource(Guid userId)
        {           
            await _userTokens
                .Where(t => t.UserId==userId)
                 .ForEachAsync(userToken => _userTokens.Remove(userToken));
        }

    }
}
