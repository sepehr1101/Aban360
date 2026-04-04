using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class KartQueryService : AbstractBaseConnection, IKartQueryService
    {
        public KartQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<CalculationRequestDisplayDataOutputDto>> Get(string stringTrackNumber, int zoneId)
        {
            string dbName = GetDbName(zoneId);
            string query = GetByTrackNumberQuery(dbName);
            IEnumerable<CalculationRequestDisplayDataOutputDto> data = await _sqlReportConnection.QueryAsync<CalculationRequestDisplayDataOutputDto>(query, new { stringTrackNumber });
            return data;
        }
        public async Task<CalculationRequestDisplayDataOutputDto> Get(int id, int zoneId)
        {
            string dbName = GetDbName(zoneId);
            string query = GetByIdQuery(dbName);
            CalculationRequestDisplayDataOutputDto data = await _sqlReportConnection.QueryFirstOrDefaultAsync<CalculationRequestDisplayDataOutputDto>(query, new { id });
            return data;
        }

        private string GetByTrackNumberQuery(string dbName)
        {
            return $@"Select 
                    	t100.C1 Title,
                    	k.pard Amount,
                    	k.takhfif Discount,
                        k.cod_takh  DiscountTypeId
                    From [{dbName}].dbo.kart k
                    Join [Db70].dbo.T100 t100	
                    	ON k.noe_bed=t100.C0
                    where k.par_no=@stringTrackNumber";
        }
        private string GetByIdQuery(string dbName)
        {
            return $@"Select 
                    	t100.C1 Title,
                    	k.pard Amount,
                    	k.takhfif Discount,
                        k.cod_takh  DiscountTypeId
                    From [{dbName}].dbo.kart k
                    Join [Db70].dbo.T100 t100	
                    	ON k.noe_bed=t100.C0
                    where k.id=@id";
        }
    }
}
