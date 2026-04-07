using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Implementations
{
    internal sealed class UserRoleQueryService : IUserRoleQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UserRole> _userRoles;
        public UserRoleQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _userRoles = _uow.Set<UserRole>();
            _userRoles.NotNull(nameof(_userRoles));
        }
        public async Task<ICollection<UserRole>> Get(Guid userId)
        {
            return await _userRoles
                .Where(r =>
                    r.UserId == userId &&
                    r.ValidTo == null)
                .ToListAsync();
        }
        public async Task<ICollection<UserRole>> GetIncludeRoles(Guid userId)
        {
            return await _userRoles
                .Where(r => r.UserId == userId)
                .Include(ur => ur.Role)
                .ToListAsync();
        }
        public async Task<ICollection<UserRole>> Get(string roleTitle)
        {
            return await _userRoles
                .Include(ur => ur.Role)
                .Include(ur => ur.User)
                .Where(ur => ur.Role.Title == roleTitle)
                .ToListAsync();
        }
        public async Task<ICollection<UserQueryDto>> Get(int roleId)
        {
            return await _userRoles
                 .Include(ur => ur.Role)
                 .Include(ur => ur.User)
                 .Where(ur => ur.RoleId == roleId)
                 .Select(ur => new UserQueryDto()
                 {
                     Id = ur.User.Id,
                     FullName = ur.User.FullName,
                     DisplayName = ur.User.DisplayName,
                     Username = ur.User.DisplayName,
                     Mobile = ur.User.Mobile,
                     MobileConfirmed = ur.User.MobileConfirmed,
                     HasTwoStepVerification = ur.User.HasTwoStepVerification,
                     IsLocked = ur.User.LockTimespan.HasValue
                 })
                 .ToListAsync();
        }
    }
}