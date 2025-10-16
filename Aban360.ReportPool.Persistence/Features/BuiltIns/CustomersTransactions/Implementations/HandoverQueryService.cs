using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class HandoverQueryService : AbstractBaseConnection, IHandoverQueryService
    {
        public HandoverQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<IEnumerable<HandoverQueryDto>> Get()
        {
            string query = GetQuery();
            IEnumerable<HandoverQueryDto> result = await _sqlReportConnection.QueryAsync<HandoverQueryDto>(query);
            return result;
        }
        private string GetQuery()
        {
            return @"Select	
                    	C0 AS Id,
                    	C1 AS Title
                    from [Db70].dbo.T7";
        }
    }
    public record HandoverQueryDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
