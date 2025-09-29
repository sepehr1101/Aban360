using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class BillQueryService : AbstractBaseConnection, IBillQueryService
    {
        public BillQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<bool> HasBillId(string billId)
        {
            string getQuery = GetQuery();
            int hasBillId = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(getQuery, new { billId });
            return hasBillId == 1 ? true : false;
        }

        private string GetQuery()
        {
            return @"Select 1
                    From [CustomerWarehouse].dbo.Clients c
                    Where 
                    	c.BillId=@billId AND
                    	c.ToDayJalali IS NULL";
        }
    }
}
