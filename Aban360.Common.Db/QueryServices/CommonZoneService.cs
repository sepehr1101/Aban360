using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.Common.Db.QueryServices
{
    public interface ICommonZoneService
    {
        Task<IEnumerable<NumericDictionary>> GetIdTitle(IAppUser appUser);
        Task<IEnumerable<int>> GetMyZoneIds(IAppUser appUser);
        Task<NumericDictionary> GetDefault(IAppUser appUser);
        Task<NumericDictionary> GetDefaultRegion(IAppUser appUser);
    }
    public sealed class CommonZoneService : AbstractBaseConnection, ICommonZoneService
    {
        public CommonZoneService(IConfiguration configuration)
            :base(configuration)
        {
        }

        public async Task<IEnumerable<int>> GetMyZoneIds(IAppUser appUser)
        {
            string query = GetIdQuery();
            IEnumerable<int> result = await _sqlConnection.QueryAsync<int>(query, new { userId = appUser.UserId });
            
            return result;
        }
        public async Task<IEnumerable<NumericDictionary>> GetIdTitle(IAppUser appUser)
        {
            string query = GetIdTitleQuery();
            IEnumerable<NumericDictionary> result = await _sqlConnection.QueryAsync<NumericDictionary>(query, new { userId = appUser.UserId });

            return result;
        }
        public async Task<NumericDictionary> GetDefault(IAppUser appUser)
        {
            string query = GetUserDefaultZoneQuery();
            NumericDictionary? result = await _sqlConnection.QueryFirstOrDefaultAsync<NumericDictionary>(query, new { userId = appUser.UserId });
            if(result is null)
            {
                return new NumericDictionary();
            }
            return result;
        }
        public async Task<NumericDictionary> GetDefaultRegion(IAppUser appUser)
        {
            string query = GetUserDefaultRegionQuery();
            NumericDictionary? result = await _sqlConnection.QueryFirstOrDefaultAsync<NumericDictionary>(query, new { userId = appUser.UserId });
            if (result is null)
            {
                return new NumericDictionary();
            }
            return result;
        }

        private string GetIdQuery()
        {
            return @"Select ClaimValue
                    From Aban360.UserPool.userClaim
                    where 
                    	UserId=@userId AND
                    	ClaimTypeId=4 AND
                        ValidTo IS NULL";
        }
        private string GetIdTitleQuery()
        {
            return @"Select 
                    	uc.ClaimValue as Id,
                    	z.Title
                    From Aban360.UserPool.userClaim uc
                    Left Join Aban360.LocationPool.Zone z
                    	On uc.ClaimValue=z.Id
                    where 
                    	uc.UserId=@userId AND
                    	uc.ClaimTypeId=4 AND
                        uc.ValidTo IS NULL";
        }
        private string GetUserDefaultZoneQuery()
        {
            return @"Select TOP 1
                    	uc.ClaimValue as Id,
                    	z.Title
                    From Aban360.UserPool.userClaim uc
                    Left Join Aban360.LocationPool.Zone z
                    	On uc.ClaimValue=z.Id
                    where 
                    	uc.UserId=@userId AND
                    	uc.ClaimTypeId=5 AND
                        uc.ValidTo Is NULL";
        }
        private string GetUserDefaultRegionQuery()
        {
            return @"Select TOP 1
                    	r.Id as Id,
                    	r.Title
                    From Aban360.UserPool.userClaim uc
                    Left Join Aban360.LocationPool.Zone z
                    	On uc.ClaimValue=z.Id
                    LEFT JOIN Aban360.LocationPool.Region r
                        ON z.RegionId=r.Id
                    where 
                    	uc.UserId=@userId AND
                    	uc.ClaimTypeId=5 AND
                        uc.ValidTo Is NULL";
        }
    }
}
