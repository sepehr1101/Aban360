using Aban360.Common.Extensions;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public class WaterMeterSummeryQueryService : IWaterMeterSummeryQueryService
    {
        private readonly IConfiguration _configuration;
        public WaterMeterSummeryQueryService(IConfiguration configuration)
        {
            _configuration = configuration;
            _configuration.NotNull(nameof(configuration));
        }
        public async Task<WaterMeterSummaryDto> GetSummery(string billId,short meterDiameterId)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var connection = new SqlConnection(connectionString);
            var estateQuery = SiphoneGetQuery();
            var result = await connection.QuerySingleAsync<WaterMeterSummaryDto>(estateQuery , new { billId = billId, meterDiameterId = meterDiameterId });
            
            return result;
        }

        private string SiphoneGetQuery()
        {
            return @"select 
                      W.BodySerial,W.InstallationDate,
                      MUT.Title as MeterUseTypeTitle,MD.Title as MeterDiameterTitle
                    from WaterMeter W
                    left join MeterUseType MUT on W.MeterUseTypeId=MUT.Id
                    left join MeterDiameter MD on W.MeterDiameterId=MD.Id
                    where W.BillId=@billId and MUT.Id=@meterDiameterId";
        }
    }
}
