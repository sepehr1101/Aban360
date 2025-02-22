using Aban360.Common.Extensions;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    internal class FlatSummeryQueryService : IFlatSummeryQueryService
    {
        private readonly IConfiguration _configuration;
        public FlatSummeryQueryService(IConfiguration configuration)
        {
            _configuration = configuration;
            _configuration.NotNull(nameof(configuration));
        }
        public async Task<ResultFlatDto> GetInfo(string billId)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var connection = new SqlConnection(connectionString);
            string estateQuery = FlatGetQuery();
            ResultFlatDto? result = await connection.QuerySingleAsync<ResultFlatDto>(estateQuery , new {id=billId});
            
            return result;
        }

        private string FlatGetQuery()
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
