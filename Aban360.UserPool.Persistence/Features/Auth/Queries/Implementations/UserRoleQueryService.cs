using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Implementations
{
    public class UserRoleQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UserRole> _userRoles;
        public UserRoleQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _userRoles=_uow.Set<UserRole>();
            _userRoles.NotNull(nameof(_userRoles));
        }
        public async Task<ICollection<UserRole>> Get(Guid userId)
        {
            return await _userRoles.Where(r=>r.UserId == userId)
                .ToListAsync();
        }
    }
}
