using Aban360.Common.Extensions;
using Aban360.ReportPool.Persistence.Base;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    internal class EstateSummeryQueryService : AbstractBaseConnection, IEstateSummeryQueryService
    {
        public EstateSummeryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ResultEstateDto> GetSummery(string billId)
        {
            var estateQuery = GetEstateSummeryDtoQuery();
            ResultEstateDto? result = await _sqlConnection.QuerySingleAsync<ResultEstateDto>(estateQuery, new { id = billId });

            return result;
        }

        private string GetEstateSummeryDtoQuery()
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
