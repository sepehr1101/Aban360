using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.Data.SqlClient;
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
                    .Where(user => user.ValidTo == null);
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
                .Include(user => user.UserRoles.Where(userRole => userRole.ValidTo == null))
                .ThenInclude(userRole => userRole.Role)
                .Include(user => user.UserClaims.Where(userClaim => userClaim.ValidTo == null))
                .SingleAsync(user => user.Id == userId);
        }

        public async Task<IEnumerable<UserQueryDto>> Search(SearchUserDto input)
        {
            string zoneIdCondition = string.Empty, endpointIdCondition = string.Empty, roleIdCondition = string.Empty,
            joinTypeZone = "LEFT", joinTypeEndpoint = "LEFT", joinTypeRole = "LEFT";

            if(input.ZoneIds != null && input.ZoneIds.Any())
            {
               string zoneIds= string.Join(",", input.ZoneIds);
                zoneIdCondition = $" AND uc_zone.ClaimValue IN ({zoneIds})";
                joinTypeZone = string.Empty;
            }
            if(input.EndpointIds!=null && input.EndpointIds.Any())
            {
                string endpointIds = string.Join(",", input.EndpointIds);
                endpointIdCondition = $"AND p.Id IN ({endpointIds})";
                joinTypeEndpoint = string.Empty;
            }
            if(input.RoleIds!=null && input.RoleIds.Any())
            {
                string roleIds = string.Join(",", input.RoleIds);
                roleIdCondition = $"AND ur.RoleId IN ({roleIds})";
                joinTypeRole = string.Empty;
            }

            string query = GetSearchUserQuery(zoneIdCondition, endpointIdCondition, roleIdCondition,
                joinTypeZone, joinTypeEndpoint, joinTypeRole);
            IEnumerable<UserQueryDto> result = await _uow.ExecuteQuery<UserQueryDto>(query);

            return result;
        }
        public async Task<IEnumerable<UserQueryDto>> Get(UserSearchByRoleTitleAndZoneIdDto inputDto)
        {
            string query = GetSearchByRoleTitleAndZoneIdQuery(inputDto);
            IEnumerable<UserQueryDto> result = await _uow.ExecuteQuery<UserQueryDto>(query);

            return result;
        }
        public async Task<IEnumerable<StringDictionary>> GetDictionary(UserSearchByRoleTitleAndZoneIdDto inputDto)
        {
            string query = GetSearchByRoleTitleAndZoneIdDictionaryQuery(inputDto);
            IEnumerable<StringDictionary> result = await _uow.ExecuteQuery<StringDictionary>(query);

            return result;
        }


        private string GetSearchUserQuery(string zoneIdCondition, string endpointIdCondition, string roleIdCondition,
            string joinTypeZone, string joinTypeEndpoint, string joinTypeRole)
        {
            return @$";WITH OneRowPerUser AS
                    (
                        SELECT 
                            u.Id,
                            u.FullName,
                            u.DisplayName,
                            u.Username,
                            u.Mobile,
                            u.MobileConfirmed,
                            u.HasTwoStepVerification,
                            CAST(0 AS bit) IsLocked,
                            ROW_NUMBER() OVER(PARTITION BY u.Id ORDER BY u.Id) AS rn
                        FROM [Aban360].UserPool.[User] u
                        {joinTypeZone} JOIN [Aban360].UserPool.UserClaim uc_zone
                            ON u.Id = uc_zone.UserId 
                            AND uc_zone.ClaimTypeId = 5 
                            AND uc_zone.ValidTo IS NULL 
                            {zoneIdCondition}
                        {joinTypeEndpoint} JOIN [Aban360].UserPool.UserClaim uc_Tree
                            ON u.Id = uc_Tree.UserId 
                            AND uc_Tree.ClaimTypeId = 6 
                            AND uc_Tree.ValidTo IS NULL 
                        {joinTypeEndpoint} JOIN [Aban360].UserPool.Endpoint p
                            ON uc_Tree.ClaimValue = p.AuthValue 
                           {endpointIdCondition}
                    	{joinTypeRole} JOIN [Aban360].UserPool.UserRole ur
                    		ON u.Id=ur.UserId 
                            AND ur.ValidTo is null
                           {roleIdCondition}
                        WHERE u.ValidTo IS NULL 
                    )
                    SELECT 
                        Id, 
                    	FullName,
                    	DisplayName, 
                    	Username,
                    	Mobile, 
                    	MobileConfirmed,
                        HasTwoStepVerification, 
                    	IsLocked
                    FROM OneRowPerUser
                    WHERE rn = 1;";
        }
        private string GetSearchByRoleTitleAndZoneIdQuery(UserSearchByRoleTitleAndZoneIdDto _params)
        {
            return @$"Select 
                    	u.Id, 
                    	u.FullName,
                    	u.DisplayName, 
                    	u.Username,
                    	u.Mobile, 
                    	u.MobileConfirmed,
                    	u.HasTwoStepVerification, 
                    	IIF(u.LockTimespan Is NUll,0,1) IsLocked
                    From Aban360.UserPool.UserClaim uc
                    Left Join Aban360.UserPool.UserRole ur
                    	ON uc.UserId=ur.UserId
                    Left Join Aban360.UserPool.Role r
                    	ON ur.RoleId=r.Id
                    Left Join Aban360.UserPool.[User] u
                    	ON ur.UserId=u.Id
                    Where 
                    	uc.ClaimTypeId={(int)_params.ClaimType} AND
                    	uc.ClaimValue='{_params.ZoneId}' AND
                    	r.Name='{_params.RoleName}'";
        }
        private string GetSearchByRoleTitleAndZoneIdDictionaryQuery(UserSearchByRoleTitleAndZoneIdDto _params)
        {
            return @$"Select Distinct 
                    	u.Username Id,
                    	u.FullName Title
                    From Aban360.UserPool.UserClaim uc
                    Left Join Aban360.UserPool.UserRole ur
                    	ON uc.UserId=ur.UserId
                    Left Join Aban360.UserPool.Role r
                    	ON ur.RoleId=r.Id
                    Left Join Aban360.UserPool.[User] u
                    	ON ur.UserId=u.Id
                    Where 
                    	uc.ClaimTypeId={(int)_params.ClaimType} AND
                    	uc.ClaimValue='{_params.ZoneId}' AND
                    	r.Name='{_params.RoleName}'";
        }

    }
}
