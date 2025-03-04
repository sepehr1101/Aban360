using Aban360.ReportPool.Persistence.Base;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    internal class FlatSummeryQueryService : AbstractBaseConnection, IFlatSummeryQueryService
    {
        public FlatSummeryQueryService(IConfiguration configuration)
            :base(configuration)
        {
        }
        public async Task<IEnumerable<ResultFlatDto>> GetInfo(string billId)
        {
            string estateQuery = GetFlatSummeryDtoQuery();
            IEnumerable<ResultFlatDto> result = await _sqlConnection.QueryAsync<ResultFlatDto>(estateQuery , new {id=billId});
            
            return result;
        }

        private string GetFlatSummeryDtoQuery()
        {
            return @" select
                        F.Id, F.PostalCode,F.Storey,F.Description
                      from [ClaimPool].WaterMeter W
                      join [ClaimPool].Estate E on W.EstateId=E.Id
                      join [ClaimPool].Flat F on E.Id=F.EstateId 
                      where W.BillId=@id";
        }
    }
}
