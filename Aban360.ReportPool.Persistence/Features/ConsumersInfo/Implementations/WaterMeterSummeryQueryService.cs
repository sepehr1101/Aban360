using Aban360.ReportPool.Persistence.Base;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    internal class WaterMeterSummeryQueryService : AbstractBaseConnection, IWaterMeterSummeryQueryService
    {
        public WaterMeterSummeryQueryService(IConfiguration configuration)
            :base(configuration)
        {
        }
        public async Task<IEnumerable<WaterMeterSummaryDto>> GetInfo(string billId, short meterUseTypeId)
        {
            string estateQuery = GetWaterMetereSummeryDtoQuery();
            IEnumerable<WaterMeterSummaryDto> result = await _sqlConnection.QueryAsync<WaterMeterSummaryDto>(estateQuery , new { billId = billId, meterUseTypeId = meterUseTypeId });
            
            return result;
        }

        private string GetWaterMetereSummeryDtoQuery()
        {
            return @"SELECT 
                      W.Id,
                      W.BodySerial,
                      W.InstallationDate,
                      MUT.Title as MeterUseTypeTitle,
                      MD.Title as MeterDiameterTitle
                    FROM WaterMeter W
                    JOIN MeterUseType MUT on W.MeterUseTypeId=MUT.Id
                    JOIN MeterDiameter MD on W.MeterDiameterId=MD.Id
                    WHERE W.BillId=@billId and MUT.Id=@meterUseTypeId";
        }
    }
}
