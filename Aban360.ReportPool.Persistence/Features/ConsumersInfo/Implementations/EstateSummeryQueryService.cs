using Aban360.ReportPool.Persistence.Base;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    internal class EstateSummeryQueryService : AbstractBaseConnection, IEstateSummeryQueryService
    {
        public EstateSummeryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<IEnumerable<ResultEstateDto>> GetSummery(string billId)
        {
            string estateQuery = GetEstateSummeryDtoQuery();
            IEnumerable<ResultEstateDto> result = await _sqlConnection.QueryAsync<ResultEstateDto>(estateQuery, new { id = billId });

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
