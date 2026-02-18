using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Implementations
{
    internal sealed class GhestAbQueryService : AbstractBaseConnection, IGhestAbQueryService
    {
        public GhestAbQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<IEnumerable<BillInstallmentOutputDto>> Get(ZoneIdAndCustomerNumberOutputDto inputDto)
        {
            string dbName = GetDbName(inputDto.ZoneId);
            string query = GetCustomerInstallmentsQuery(dbName);
            IEnumerable<BillInstallmentOutputDto> result = await _sqlReportConnection.QueryAsync<BillInstallmentOutputDto>(query, inputDto);
            return result;
        }
        private string GetCustomerInstallmentsQuery(string dbName)
        {
            return $@"Select 
                    	g.ID,
                    	g.town ZoneId,
                    	g.radif CustomerNumber,
                    	g.enshab ReadingNumber,
                    	g.barge Brage,
                    	g.date_bed RegisterDateJalali,
                    	g.mohlat DeadLineDateJalali,
                    	g.pard Payable,
                    	g.cod_enshab UsageId,
                    	t41.C1 UsageTitle,
                    	g.enshab MeterDiamterId,
                    	t5.C2 MeterDiameterTitle
                    From [{dbName}].dbo.ghest_ab g
                    Join [Db70].dbo.T41 t41
                    	ON g.cod_enshab=t41.C0
                    Join [Db70].dbo.T5 t5
                    	ON g.enshab=t5.C0
                    Where 
                    	g.town=@zoneId AND
                    	g.radif=@customerNumber";
        }
    }
}
