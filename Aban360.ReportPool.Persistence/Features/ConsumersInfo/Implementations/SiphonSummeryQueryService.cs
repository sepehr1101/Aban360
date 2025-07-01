using Aban360.Common.Db.Exceptions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
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
            if (!result.Any())
                throw new InvalidIdException();

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
                    from [ClaimPool].WaterMeter W
                    join [ClaimPool].WaterMeterSiphon WS on W.Id=WS.WaterMeterId
                    join [ClaimPool].Siphon S on WS.SiphonId=S.Id                   
                    join [ClaimPool].SiphonDiameter SD on S.SiphonDiameterId=SD.Id
                    LEFT OUTER JOIN [ClaimPool].SiphonType ST on S.SiphonTypeId=ST.Id
                    where W.BillId=@billId";
        }
    }
}
