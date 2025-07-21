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
                    	s.ConnectionId,
                        FORMAT(s.CompletionDateTime,'yyyy-MM-dd HH:mm:ss','fa-ir') as CompletionDateTimeJalali,
						FORMAT(s.ErrorDateTime,'yyyy-MM-dd HH:mm:ss','fa-ir') as ErrorDateTimeJalali,
						FORMAT(s.InsertDateTime,'yyyy-MM-dd HH:mm:ss','fa-ir') as InsertDateTimeJalali,
                    	s.IsInformed
                    From [Aban360].ReportPool.ServerReports s
                    Where s.Id=@id";
        }
    }
}
