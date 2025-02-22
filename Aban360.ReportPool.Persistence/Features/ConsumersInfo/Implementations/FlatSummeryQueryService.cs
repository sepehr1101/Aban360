using Aban360.Common.Extensions;
using Aban360.ReportPool.Persistence.Base;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    internal class FlatSummeryQueryService : AbstractBaseConnection,IFlatSummeryQueryService
    {
        public FlatSummeryQueryService(IConfiguration configuration)
            :base(configuration)
        {
        }
        public async Task<ResultFlatDto> GetInfo(string billId)
        {
            string estateQuery = GetFlatSummeryDtoQuery();
            ResultFlatDto? result = await _sqlConnection.QuerySingleAsync<ResultFlatDto>(estateQuery , new {id=billId});
            
            return result;
        }

        private string GetFlatSummeryDtoQuery()
        {
            return @" select
                        F.PostalCode,F.Storey,F.Description
                      from WaterMeter W
                      left join Estate E on W.EstateId=E.Id
                      left join Flat F on E.Id=F.EstateId 
                      where W.BillId=@id";
        }
    }
}
