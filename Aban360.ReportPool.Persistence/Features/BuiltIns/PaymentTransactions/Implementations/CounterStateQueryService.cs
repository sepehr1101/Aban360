using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal class CounterStateQueryService : AbstractBaseConnection, ICounterStateQueryService
    {
        public CounterStateQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<StringDictionary>> Get()
        {
            string query = GetQuery();
            IEnumerable<StringDictionary> result = await _sqlReportConnection.QueryAsync<StringDictionary>(query);

            return result;
        }

        private string GetQuery()
        {
            return @"Select 
	                	MoshtarakinId as Id,
	                	Title
	                From [Db70].dbo.CounterVaziat ";
        }
    }
}
