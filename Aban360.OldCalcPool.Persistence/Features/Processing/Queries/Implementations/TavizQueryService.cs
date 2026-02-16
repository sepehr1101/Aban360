using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Implementations
{
    internal sealed class TavizQueryService : AbstractBaseConnection, ITavizQueryService
    {
        public TavizQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<LatesTavizInfo> Get(ZoneIdAndCustomerNumberOutputDto input)
        {
            string dbName = GetDbName(input.ZoneId);
            string query=GetQuery(dbName);
            LatesTavizInfo result=await _sqlReportConnection.QueryFirstOrDefaultAsync<LatesTavizInfo>(query,input);
            return result;
        }
        private string GetQuery(string dbName)
        {
            return $@"Select 
					t.taviz_date as TavizDateJalali,
					t.elat as TavizCause,
					t.date_sabt as TavizRegisterDateJalali,
					t.taviz_no as TavizNumber
				From [{dbName}].dbo.taviz t
				Where
					t.town=@zoneId AND
					t.radif=@customerNumber;";
        }

    }
}
