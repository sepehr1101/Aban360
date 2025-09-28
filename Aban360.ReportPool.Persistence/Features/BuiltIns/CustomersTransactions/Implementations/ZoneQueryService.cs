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
            string zoneIdsQueryString = GetUserZoneIdsQuery();
            IEnumerable<UserZoneIdsOutputDto> zones = await _sqlReportConnection.QueryAsync<UserZoneIdsOutputDto>(zoneIdsQueryString);

            return zones;
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
    }
}
