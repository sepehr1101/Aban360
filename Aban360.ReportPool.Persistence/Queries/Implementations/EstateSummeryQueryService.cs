using Aban360.Common.Extensions;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    internal class EstateSummeryQueryService : IEstateSummeryQueryService
    {
        private readonly IConfiguration _configuration;
        public EstateSummeryQueryService(IConfiguration configuration)
        {
            _configuration = configuration;
            _configuration.NotNull(nameof(configuration));
        }
        public async Task<ResultEstateDto> GetSummery(string billId)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var connection = new SqlConnection(connectionString);
            var estateQuery = EstateGetQuery();
            ResultEstateDto? result = await connection.QuerySingleAsync<ResultEstateDto>(estateQuery , new {id=billId});
            
            return result;
        }

        private string EstateGetQuery()
        {
            return @" select
                        E.Premises,E.X,E.Y,
                        E.ImprovementsCommercial,E.ImprovementsDomestic,E.ImprovementsOther,E.ImprovementsOverall
                      from WaterMeter W
                      left join Estate E on W.EstateId=E.Id
                      where W.BillId=@id";
        }
    }
}
