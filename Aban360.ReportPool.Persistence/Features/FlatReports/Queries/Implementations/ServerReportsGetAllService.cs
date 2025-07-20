using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;
using Aban360.ReportPool.Persistence.Features.FlatReports.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.FlatReports.Queries.Implementations
{
    internal sealed class ServerReportsGetAllService : AbstractBaseConnection, IServerReportsGetAllService
    {
        public ServerReportsGetAllService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<IEnumerable<ServerReportsGetAllDto>> Get(Guid userId)
        {
            string serverReportsByIdQueryString = GetServerReportsByIdQuery();
            IEnumerable<ServerReportsGetAllDto> data = await _sqlConnection.QueryAsync<ServerReportsGetAllDto>(serverReportsByIdQueryString, new { userId });
            return data;
        }
        private string GetServerReportsByIdQuery()
        {
            return @"Select 
                    	s.Id,
                    	s.UserId,
                    	s.ReportName,
                    	s.ReportPath,
                    	s.CompletionDateJalali,
                    	s.ConnectionId,
                    	s.ErrorDateJalali,
                    	s.InsertDateJalali,
                    	s.IsInformed,
						IIF(s.CompletionDateJalali IS Null,0,1) AS IsCompleted
                    From [Aban360].ReportPool.ServerReports s
                    Where s.UserId=@userId";
        }
    }
}
