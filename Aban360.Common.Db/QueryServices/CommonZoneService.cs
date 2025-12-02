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
        Task<IEnumerable<int>> Get(IAppUser appUser);
    }
    public sealed class CommonZoneService : AbstractBaseConnection, ICommonZoneService
    {
        public CommonZoneService(IConfiguration configuration)
            :base(configuration)
        {
        }

        public async Task<IEnumerable<int>> Get(IAppUser appUser)
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

        private string GetIdQuery()
        {
            return @"Select ClaimValue
                    From Aban360.UserPool.userClaim
                    where 
                    	UserId=@userId AND
                    	ClaimTypeId=4";
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
                    	uc.ClaimTypeId=4";
        }
    }
}
