using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Implementations
{
    public class UserQueryService : IUserQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<User> _users;
        public UserQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _users = _uow.Set<User>();
            _users.NotNull(nameof(_users));
        }
        public async Task<User> Get(Guid id)
        {
            return await _uow.FindOrThrowAsync<User>(id);
        }
        public async Task<User?> Get(string username)
        {
            return await _users
                .SingleOrDefaultAsync(u => u.Username == username);
        }
    }
}
