using Aban360.ReportPool.Persistence.Base;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    internal class SiphonSummeryQueryService : AbstractBaseConnection, ISiphonSummeryQueryService
    {
        public SiphonSummeryQueryService(IConfiguration configuration)
            :base(configuration)
        {
        }
        public async Task<IEnumerable<SiphonSummaryDto>> GetInfo(string billId)
        {
            string query = GetSiphonSummeryDtoQuery();
            IEnumerable<SiphonSummaryDto> result = await _sqlConnection.QueryAsync<SiphonSummaryDto>(query , new { billId = billId });
            
            return result;
        }

        private string GetSiphonSummeryDtoQuery()
        {
            return @"SELECT 
                       S.Id,
                       S.InstallationLocation,
                       S.InstallationDate,
                       ST.Title as SiphonTypeTitle,
                       SD.Title as SiphonDiameterTitle
                    from WaterMeter W
                    join WaterMeterSiphon WS on W.Id=WS.WaterMeterId
                    join Siphon S on WS.SiphonId=S.Id                   
                    join SiphonDiameter SD on S.SiphonDiameterId=SD.Id
                    LEFT OUTER JOIN SiphonType ST on S.SiphonTypeId=ST.Id
                    where W.BillId=@billId";
        }
    }
}
