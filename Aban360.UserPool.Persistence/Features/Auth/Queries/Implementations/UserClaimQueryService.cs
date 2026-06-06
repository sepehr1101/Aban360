using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Constants.Enums;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Implementations
{
    internal sealed class UserClaimQueryService : IUserClaimQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UserClaim> _userClaims;
        private readonly DbSet<UserRole> _userRoles;
        public UserClaimQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _userClaims = _uow.Set<UserClaim>();
            _userClaims.NotNull(nameof(_userClaims));

            _userRoles = _uow.Set<UserRole>();
            _userRoles.NotNull(nameof(_userRoles));
        }

        public IQueryable<UserClaim> GetQuery()
        {
            return _userClaims.AsQueryable();
        }
        public async Task<ICollection<UserClaim>> Get(Guid userId)
        {
            return await _userClaims
                .Where(uc =>
                    uc.UserId == userId &&
                    uc.ValidTo == null)
                .ToListAsync();
        }
        public async Task<ICollection<UserClaim>> Get(Guid userId, ClaimType claimType)
        {
            return await _userClaims
                .Where(userClaim =>
                    userClaim.UserId == userId &&
                    userClaim.ClaimTypeId == claimType)
                .ToListAsync();
        }
        public async Task<ICollection<UserClaim>> GetValid(Guid userId, ClaimType claimType)
        {
            return await _userClaims
                .Where(userClaim =>
                    userClaim.UserId == userId &&
                    userClaim.ClaimTypeId == claimType &&
                    userClaim.ValidTo == null)
                .ToListAsync();
        }
        public async Task<bool> HasValue(Guid userId, ClaimType claimType, string value)
        {
            return await _userClaims
                .AnyAsync(userClaim => 
                    userClaim.UserId==userId &&
                    userClaim.ValidTo == null &&
                    userClaim.ClaimTypeId == claimType &&
                    userClaim.ClaimValue == value);
        }

        public async Task<(ICollection<Guid>, ICollection<UserClaim>)> Get(int roleId, ClaimType claimType)
        {
            List<Guid> userIds = await _userRoles
                .Where(userRole => userRole.RoleId == roleId &&
                       userRole.ValidTo == null)
                .Select(userRole => userRole.UserId)
                .Distinct()
                .ToListAsync();
            var userClaims = await _userClaims
                .Where(userClaim =>
                    userClaim.ClaimTypeId == claimType &&
                    userClaim.ValidTo == null &&
                    userIds.Contains(userClaim.UserId))
                .ToListAsync();
            return (userIds, userClaims);
            //return await _userClaims
            //       .Where(userClaim =>
            //           userClaim.ClaimTypeId == claimType &&
            //           userClaim.RemoveLogInfo == null &&
            //           userClaim.User.UserRoles
            //            .Any(userRole => userRole.RoleId == roleId && userRole.RemoveLogInfo == null))
            //       //.Include(userClaim => userClaim.User)
            //       //.ThenInclude(user => user.UserRoles)
            //       //.AsSplitQuery()
            //       .ToListAsync();
        }
    }
}