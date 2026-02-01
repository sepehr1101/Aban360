using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class ZoneQueryService : AbstractBaseConnection, IZoneQueryService
    {
        public ZoneQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<IEnumerable<UserZoneIdsOutputDto>> Get()
        {
            string query = GetUserZoneIdsQuery();
            IEnumerable<UserZoneIdsOutputDto> zones = await _sqlReportConnection.QueryAsync<UserZoneIdsOutputDto>(query);

            return zones;
        }
        public async Task<bool> GetArticle2(int zoneId)
        {
            string query = GetArticle2Query();
            bool hasArticle11 = await _sqlReportConnection.QueryFirstOrDefaultAsync<bool>(query, new { zoneId });
            return hasArticle11;
        }
        private string GetUserZoneIdsQuery()
        {
            return @"Select	
                     	t51.C0 AS Id,
                     	t51.C2+' ('+t46.C2+')' AS Title,
                        IIF(t51.C0>140000, 1, 0) IsVillage
                     From [Db70].dbo.T51 t51
			    	 Join [Db70].dbo.T46 t46 
			    		On t51.C1=t46.C0";
        }
        private string GetArticle2Query()
        {
            return @"Select C8
                    From [Db70].dbo.T51
                    Where C0=@zoneId";
        }
    }
}
