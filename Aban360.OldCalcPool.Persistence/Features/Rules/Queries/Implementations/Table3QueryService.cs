using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Implementations
{
    internal sealed class Table3QueryService : AbstractBaseConnection, ITable3QueryService
    {
        public Table3QueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<IEnumerable<Table3GetDto>> Get(Table3InputDto input)
        {
            string query = GetQuery();
            IEnumerable<Table3GetDto> result = await _sqlReportConnection.QueryAsync<Table3GetDto>(query, input);
            return result;
        }
        private string GetQuery()
        {
            return $@"Select 
                    	ZoneId,
                    	ZoneTitle,
                    	UsageGroupId,
                    	UsageGroupTitle,
                    	CompanyServiceId,
                    	CompanyServiceTitle,
                    	Price
                    From OldCalc.dbo.Table3 t3
                    Join Db70.dbo.T41 t41
                    	ON t3.UsageGroupId=t41.C10
                    Where
                    	t3.ZoneId=@ZoneId AND
                    	t41.C0=@UsageId AND
                    	t3.CompanyServiceId IN @CompanyServiceIds";
        }
    }
}
