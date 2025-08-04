using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Implementations
{
    internal sealed class Table1GetAllService : AbstractBaseConnection, ITable1GetAllService
    {
        public Table1GetAllService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<IEnumerable<Table1GetDto>> Get()
        {
            string Table1GetQueryString = GetTable1GetQuery();
            IEnumerable<Table1GetDto> result = await _sqlReportConnection.QueryAsync<Table1GetDto>(Table1GetQueryString);

            return result;
        }

        private string GetTable1GetQuery()
        {
            return @$"Select
                		t.Id,
                    	t.town,
                    	t.z1,
                    	t.z2,
                    	t.olgo,
                    	t.darsa_gh
                    From [OldCalc].dbo.table1 t";
        }
    }
}
