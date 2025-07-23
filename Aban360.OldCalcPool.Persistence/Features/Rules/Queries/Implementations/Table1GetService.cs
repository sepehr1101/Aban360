using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Implementations
{
    internal sealed class Table1GetService : AbstractBaseConnection, ITable1GetService
    {
        public Table1GetService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<IEnumerable<Table1GetDto>> Get(int id)
        {
            string Table1GetQueryString = GetTable1GetQuery();
            IEnumerable<Table1GetDto> result = await _sqlReportConnection.QueryAsync<Table1GetDto>(Table1GetQueryString, new { id });

            return result;
        }

        private string GetTable1GetQuery()
        {
            return @$"Select
                		n.date1 AS Date1,
                		n.date2 AS Date2,
                		n.ebt AS Ebt,
                		n.ent AS Ent,
                		n.vaj AS Vaj,
                		n.cod AS Cod,
                		n.olgo AS Olgo,
                		n.[desc] AS [Desc],
                		n.o_vaj AS OVaj,
                		n.o_vaj_faz AS OVajFaz
                	From [OldCalc].dbo.table1 n
                	Where n.Id=@id";
        }
    }
}
