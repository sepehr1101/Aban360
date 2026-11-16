using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Constants.Enums;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Implementations
{
    internal sealed class UserRoleQueryService : AbstractBaseConnection, IUserRoleQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UserRole> _userRoles;
        public UserRoleQueryService(IUnitOfWork uow, IConfiguration configuration)
            : base(configuration)
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
                .Where(r => r.UserId == userId && r.ValidTo == null)
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
                 .Where(ur =>
                    ur.RoleId == roleId &&
                    ur.Role.ValidTo == null &&
                    ur.User.ValidTo == null &&
                    ur.ValidTo == null)
                 .Select(ur => new UserQueryDto()
                 {
                     Id = ur.User.Id,
                     FullName = ur.User.FullName,
                     DisplayName = ur.User.DisplayName,
                     Username = ur.User.Username,
                     Mobile = ur.User.Mobile,
                     MobileConfirmed = ur.User.MobileConfirmed,
                     HasTwoStepVerification = ur.User.HasTwoStepVerification,
                     IsLocked = ur.User.LockTimespan.HasValue
                 })
                 .ToListAsync();
        }
        public async Task<ICollection<UserQueryDto>> GetByDefaultZone(int roleId, int zoneId)
        {
            return await _userRoles
                 .Include(ur => ur.Role)
                 .Include(ur => ur.User)
                 .ThenInclude(user => user.UserClaims)
                 .Where(ur =>
                    ur.RoleId == roleId &&
                    ur.Role.ValidTo == null &&
                    ur.User.ValidTo == null &&
                    ur.ValidTo == null &&
                    ur.User.UserClaims.Any(user => user.ValidTo == null && user.ClaimTypeId == ClaimType.DefaultZoneId && user.ClaimValue == zoneId.ToString()))
                 .Select(ur => new UserQueryDto()
                 {
                     Id = ur.User.Id,
                     FullName = ur.User.FullName,
                     DisplayName = ur.User.DisplayName,
                     Username = ur.User.Username,
                     Mobile = ur.User.Mobile,
                     MobileConfirmed = ur.User.MobileConfirmed,
                     HasTwoStepVerification = ur.User.HasTwoStepVerification,
                     IsLocked = ur.User.LockTimespan.HasValue,
                     DefaultZoneId = ur.User.UserClaims.Where(userClaims => userClaims.ClaimTypeId == ClaimType.DefaultZoneId && userClaims.ClaimValue == zoneId.ToString()).FirstOrDefault().ClaimValue
                 })
                 .ToListAsync();
        }
        public async Task<ICollection<UserQueryDto>> GetByZone(int roleId, int zoneId)
        {
            var parameters = new[]
            {
                new SqlParameter("@RoleId", roleId),
                new SqlParameter("@ZoneId", zoneId)
            };
            ICollection<UserQueryDto> data = await _uow.ExecuteQuery<UserQueryDto>(GetUserRoleByZoneAndRoleIdQuery(), parameters);
            return data;
        }
        private string GetUserRoleByZoneAndRoleIdQuery()
        {
            return $@"Select 
                    	u.Id,
                    	u.FullName,
                    	u.DisplayName,
                    	u.Username,
                    	u.Mobile,
                    	Cast(uc_defaultZone.ClaimTypeId AS nvarchar) DefaultZoneId,
                    	uc_defaultZone.ClaimValue DefaultZoneTitle,
                    	r.Title RolesTitle,
                    	u.MobileConfirmed,
                    	u.HasTwoStepVerification,
	                    Cast(IIF(u.LockTimespan IS NULL,1,0) AS Bit) IsLocked
                    From Aban360.UserPool.[User] u
                    Join Aban360.UserPool.UserRole ur
                    	ON u.Id = ur.UserId AND ur.RoleId = @RoleId
                    Join Aban360.UserPool.Role r
                    	ON ur.RoleId = r.Id
                    Join Aban360.UserPool.UserClaim uc
                    	On u.Id = uc.UserId AND ClaimTypeId = 4 AND uc.ClaimValue = @ZoneId
                    Join Aban360.UserPool.UserClaim uc_defaultZone
                    	On u.Id = uc_defaultZone.UserId AND uc_defaultZone.ClaimTypeId = 5
                    Where 
                    	u.ValidTo IS NULL AND
                    	u.LockTimespan IS NULL AND
                    	r.ValidTo IS NULL AND
                    	ur.ValidTo IS NULL AND
                    	uc.ValidTo IS NULL AND
                    	uc_defaultZone.ValidTo IS NULL ";
        }
    }
}