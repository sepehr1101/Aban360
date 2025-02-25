using Aban360.Common.Db.Exceptions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Constants.Enums;

namespace Aban360.UserPool.Application.Common.Base
{
    internal abstract class UserBaseCreateOrUpdateService
    {
        internal ICollection<UserClaim> CreateUserClaim(ICollection<string> value, ClaimType claimType, string logInfo, Guid operationGroupId, Guid userId)
        {
            return value.Select(x => new UserClaim()
            {
                ClaimTypeId = claimType,
                ClaimValue = x,
                InsertGroupId = operationGroupId,
                InsertLogInfo = logInfo,
                //ValidFrom = DateTime.Now,
                ValidTo = null,
                UserId = userId
            }).ToList();

        }
        internal void Validate(int zoneCount, int dtoZoneCount, int endpointCount, int dtoEndpointCount)
        {
            if (zoneCount != dtoZoneCount || endpointCount != dtoEndpointCount)
            {
                throw new InvalidIdException();
            }
        }
        internal ICollection<UserRole> CreateUserRoles(ICollection<int> roleIds, string logInfoString, Guid operationGroupId, Guid userId)
        {
            return roleIds
                .Select(roleId => CreateUserRole(roleId, logInfoString, operationGroupId, userId))
                .ToList();

            UserRole CreateUserRole(int roleId, string logInfoString, Guid operationGroupId, Guid userId)
            {
                return new UserRole()
                {
                    RoleId = roleId,
                    InsertGroupId = operationGroupId,
                    InsertLogInfo = logInfoString,
                    //ValidFrom = DateTime.Now,
                    ValidTo = null,
                    UserId = userId
                };
            }
        }
    }
}
