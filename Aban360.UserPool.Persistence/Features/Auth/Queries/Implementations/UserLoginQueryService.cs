using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Implementations
{
    public class UserLoginQueryService : IUserLoginQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UserLogin> _userLogins;
        public UserLoginQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _userLogins = _uow.Set<UserLogin>();
            _userLogins.NotNull(nameof(_userLogins));
        }
        public async Task<UserLogin?> Get(Guid id)
        {
            return await _userLogins
                .Include(u => u.User)
                .SingleOrDefaultAsync(u => u.Id == id);
        }
        public async Task<ICollection<UserLogin>?> GetByUserId(Guid userId)
        {
            return await _userLogins
                .Where(userLogin=> 
                        userLogin.UserId == userId &&
                        userLogin.LogoutDateTime==null)
                .ToListAsync();
        }
    }
}
