using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Aban360.UserPool.Persistence.Features.Auth.Commands.Implementations
{
    public class TokenStoreService : ITokenStoreService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UserToken> _tokens;

        public TokenStoreService(
            IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _tokens = _uow.Set<UserToken>();
        }

        public void Add(UserToken userToken)
        {
            _tokens.Add(userToken);
        }
        public void Remove(UserToken userToken)
        {
            _tokens.Remove(userToken);
        }
    }
}
