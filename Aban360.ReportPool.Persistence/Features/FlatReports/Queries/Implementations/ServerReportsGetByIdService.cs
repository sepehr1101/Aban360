using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;
using Aban360.ReportPool.Persistence.Features.FlatReports.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.FlatReports.Queries.Implementations
{
    internal sealed class ServerReportsGetByIdService : AbstractBaseConnection, IServerReportsGetByIdService
    {
        public ServerReportsGetByIdService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ServerReportsGetByIdDto> GetById(Guid id)
        {
            string serverReportsByIdQueryString = GetServerReportsByIdQuery();
            ServerReportsGetByIdDto data = await _sqlConnection.QueryFirstOrDefaultAsync<ServerReportsGetByIdDto>(serverReportsByIdQueryString, new { id });
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
                    	s.IsInformed
                    From [Aban360].ReportPool.ServerReports s
                    Where s.Id=@id";
        }
    }
}
