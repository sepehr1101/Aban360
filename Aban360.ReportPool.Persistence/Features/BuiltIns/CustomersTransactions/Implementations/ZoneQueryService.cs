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
        { }

        public async Task<IEnumerable<UserZoneIdsOutputDto>> Get()
        {
            string zoneIdsQueryString = GetUserZoneIdsQuery();
            IEnumerable<UserZoneIdsOutputDto> zones = await _sqlReportConnection.QueryAsync<UserZoneIdsOutputDto>(zoneIdsQueryString);

            return zones;
        }
        private string GetUserZoneIdsQuery()
        {
            return @"Select	
                 	t.C0 AS Id,
                 	t.C2 AS Title
                 From [Db70].dbo.T51 t";
        }
    }
}
