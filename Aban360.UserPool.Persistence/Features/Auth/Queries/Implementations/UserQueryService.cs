using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Implementations
{
    internal sealed class UserQueryService : IUserQueryService
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
        public IQueryable<User> GetQuery()
        {
            return _users
                .AsNoTracking()
                .AsQueryable();
        }
        private IQueryable<User> _query 
        { 
            get 
            {
                return _users.
                    AsNoTracking()
                    .Where(user=>user.ValidTo==null) ;
            }
        }
        public async Task<ICollection<User>> Get()
        {
            return await 
                _query
                .ToListAsync();
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
        public async Task<User> GetIncludeUserAndClaims(Guid userId)
        {
            return await 
                _query
                .Include(user=>user.UserRoles)
                .ThenInclude(userRole=>userRole.Role)
                .Include(user=>user.UserClaims)
                .Where(user=> user.UserClaims.Any(userClaim=>userClaim.ValidTo == null))
                .Where(user => user.UserRoles.Any(userRole => userRole.ValidTo == null))
                .SingleAsync(user=>user.Id==userId);
        }
    }
}
