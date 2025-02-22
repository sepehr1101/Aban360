using Aban360.ReportPool.Persistence.Base;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    internal class WaterMeterSummeryQueryService : AbstractBaseConnection,IWaterMeterSummeryQueryService
    {
        public WaterMeterSummeryQueryService(IConfiguration configuration)
            :base(configuration)
        {
        }
        public async Task<WaterMeterSummaryDto> GetInfo(string billId,short meterDiameterId)
        {
            var estateQuery = GetWaterMetereSummeryDtoQuery();
            var result = await _sqlConnection.QuerySingleAsync<WaterMeterSummaryDto>(estateQuery , new { billId = billId, meterDiameterId = meterDiameterId });
            
            return result;
        }

        private string GetWaterMetereSummeryDtoQuery()
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
