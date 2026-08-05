using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations
{
    internal sealed class VillageQueryService : AbstractBaseConnection, IVillageQueryService
    {
        public VillageQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<VillageGetDto>> Get()
        {
            string query = GetQuery();
            IEnumerable<VillageGetDto> result = await _sqlReportConnection.QueryAsync<VillageGetDto>(query);
            return result;
        }
        private string GetQuery()
        {
            return @"Select 
                    	v.Id,
                    	v.ZoneId,
                    	t51.C2 ZoneTitle,
                    	v.VillageId,
                    	TRIM(v.VillageName) VillageName,
                    	v.StringCode
                    From Db70.dbo.village v
                    Left Join Db70.dbo.T51 t51
                    	ON v.ZoneId=t51.C0
                    Order by t51.C2 , v.VillageName";
        }
    }
}
