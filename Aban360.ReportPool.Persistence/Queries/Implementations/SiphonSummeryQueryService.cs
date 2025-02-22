using Aban360.Common.Extensions;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    internal class SiphonSummeryQueryService : ISiphonSummeryQueryService
    {
        private readonly IConfiguration _configuration;
        public SiphonSummeryQueryService(IConfiguration configuration)
        {
            _configuration = configuration;
            _configuration.NotNull(nameof(configuration));
        }
        public async Task<SiphonSummaryDto> GetSummery(string billId)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var connection = new SqlConnection(connectionString);
            string? estateQuery = SiphonGetQuery();
            SiphonSummaryDto? result = await connection.QuerySingleAsync<SiphonSummaryDto>(estateQuery , new { billId = billId });
            
            return result;
        }

        private string SiphonGetQuery()
        {
            return @"select 
                       S.InstallationLocation,S.InstallationDate,
                       ST.Title as SiphonTypeTitle,SD.Title as SiphonDiameterTitle
                    from WaterMeter W
                    left join WaterMeterSiphon WS on W.Id=WS.WaterMeterId
                    left join Siphon S on WS.SiphonId=S.Id
                    left join SiphonType ST on S.SiphonTypeId=ST.Id
                    left join SiphonDiameter SD on S.SiphonDiameterId=SD.Id
                    where W.BillId='@billId'";
        }
    }
}
