using Aban360.Common.Extensions;
using Aban360.ReportPool.Persistence.Base;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    internal class SiphonSummeryQueryService : AbstractBaseConnection,ISiphonSummeryQueryService
    {
        public SiphonSummeryQueryService(IConfiguration configuration)
            :base(configuration)
        {
        }
        public async Task<SiphonSummaryDto> GetInfo(string billId)
        {
            string? estateQuery = GetSiphonSummeryDtoQuery();
            SiphonSummaryDto? result = await _sqlConnection.QuerySingleAsync<SiphonSummaryDto>(estateQuery , new { billId = billId });
            
            return result;
        }

        private string GetSiphonSummeryDtoQuery()
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
