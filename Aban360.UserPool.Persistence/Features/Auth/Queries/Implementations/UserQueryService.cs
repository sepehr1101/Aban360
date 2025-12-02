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
                .Include(user=>user.UserRoles.Where(userRole=> userRole.ValidTo == null))
                .ThenInclude(userRole=>userRole.Role)
                .Include(user=>user.UserClaims.Where(userClaim=>userClaim.ValidTo==null))
                .SingleAsync(user=>user.Id==userId);
        }

        public async Task<IEnumerable<UserQueryDto>> Search(SearchUserDto input)
        {
            var zoneIds = string.Join(",", input.ZoneIds);
            var endpointIds = string.Join(",", input.EndpointIds);
            var roleIds = string.Join(",", input.RoleIds);

            string query = GetSearchUserQuery(zoneIds,endpointIds,roleIds);
            IEnumerable<UserQueryDto> result=await _uow.ExecuteQuery<UserQueryDto>(query);

            return result;
        }
        private string GetSearchUserQuery(string zoneIds,string endpointIds,string roleIds)
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
                        JOIN [Aban360].UserPool.UserClaim uc_zone
                            ON u.Id = uc_zone.UserId 
                            AND uc_zone.ClaimTypeId = 5 
                            AND uc_zone.ClaimValue IN ({zoneIds})
                            AND uc_zone.ValidTo IS NULL 
                        JOIN [Aban360].UserPool.UserClaim uc_Tree
                            ON u.Id = uc_Tree.UserId 
                            AND uc_Tree.ClaimTypeId = 6 
                            AND uc_Tree.ValidTo IS NULL 
                        JOIN [Aban360].UserPool.Endpoint p
                            ON uc_Tree.ClaimValue = p.AuthValue 
                            AND p.Id IN ({endpointIds})
                    	JOIN [Aban360].UserPool.UserRole ur
                    		ON u.Id=ur.UserId AND ur.RoleId IN ({roleIds})
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
      
    }
}
