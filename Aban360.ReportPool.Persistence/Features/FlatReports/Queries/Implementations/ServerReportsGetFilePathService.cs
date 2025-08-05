using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;
using Aban360.ReportPool.Persistence.Features.FlatReports.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.FlatReports.Queries.Implementations
{
    internal sealed class ServerReportsGetFilePathService : AbstractBaseConnection, IServerReportsGetFilePathService
    {
        public ServerReportsGetFilePathService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ServerReportsGetFilePathDto> Get(Guid id)
        {
            string serverReportsByIdQueryString = GetServerReportsGetFilePathQuery();
            ServerReportsGetFilePathDto data = await _sqlConnection.QueryFirstOrDefaultAsync<ServerReportsGetFilePathDto>(serverReportsByIdQueryString, new { id });
            return data;
        }
        private string GetServerReportsGetFilePathQuery()
        {
            return @"Select 
                    	s.Id,
                    	s.ReportPath
                    From [Aban360].ReportPool.ServerReports s
                    Where 
                        s.Id=@id  AND
                        s.CompletionDateTime Is NOT Null AND
						s.ErrorDateTime Is Null";
        }
    }
}