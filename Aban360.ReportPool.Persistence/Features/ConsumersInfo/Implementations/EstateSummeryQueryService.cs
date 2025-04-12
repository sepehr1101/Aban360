using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
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
            return @" SELECT
                        E.Id, E.Premises,E.X,E.Y,
                        E.ImprovementsCommercial,E.ImprovementsDomestic,E.ImprovementsOther,E.ImprovementsOverall
                      from [ClaimPool].WaterMeter W
                      left join [ClaimPool].Estate E on W.EstateId=E.Id
                      where W.BillId=@id";
        }
    }
}
