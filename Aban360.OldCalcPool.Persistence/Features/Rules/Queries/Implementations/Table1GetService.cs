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
        {
        }

        public async Task<Table1GetDto> Get(int id)
        {
            string Table1GetQueryString = GetTable1GetQuery();
            Table1GetDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<Table1GetDto>(Table1GetQueryString, new { id });
            return result;
        }
        public async Task<Table1GetDto> GetByTown(int town)
        {
            string query = GetQuery();
            Table1GetDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<Table1GetDto>(query, new { town });
            return result;
            string GetQuery()
            {
                return @$"use OldCalc
                    Select 
                    	t.Id,
                    	t.town,
                    	t.z1,
                    	t.z2,
                    	t.olgo,
                    	t.darsa_gh
                    From [OldCalc].dbo.table1 t
                	Where t.town=@town";
            }
        }

        private string GetTable1GetQuery()
        {
            return @$"use OldCalc
                    Select 
                    	t.Id,
                    	t.town,
                    	t.z1,
                    	t.z2,
                    	t.olgo,
                    	t.darsa_gh
                    From [OldCalc].dbo.table1 t
                	Where t.Id=@id";
        }
    }
}
