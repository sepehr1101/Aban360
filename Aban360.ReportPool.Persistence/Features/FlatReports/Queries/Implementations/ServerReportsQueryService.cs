using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;
using Aban360.ReportPool.Persistence.Features.FlatReports.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.FlatReports.Queries.Implementations
{
    internal sealed class ServerReportsQueryService : AbstractBaseConnection, IServerReportsQueryService
    {
        public ServerReportsQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ServerReportsGetByIdDto?> Get(string jsonInput)
        {
            string serverReportsByIdQueryString = getIncompleteByJsonInputQuery();
            ServerReportsGetByIdDto? data = await _sqlConnection.QueryFirstOrDefaultAsync<ServerReportsGetByIdDto>(serverReportsByIdQueryString, new { jsonInput });
            return data;
        }
        public async Task<ServerReportsGetByIdDto?> Get(Guid userId)
        {
            string serverReportsByIdQueryString = getIncompleteByUserIdQuery();
            ServerReportsGetByIdDto? data = await _sqlConnection.QueryFirstOrDefaultAsync<ServerReportsGetByIdDto>(serverReportsByIdQueryString, new { userId });
            return data;
        }

        private string getIncompleteByJsonInputQuery()
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
                    	s.IsInformed,
						s.HeaderType,
						s.DataType,
						s.ReportInputType,
						s.ReportInputJson,
						s.HandlerKey
                    From [Aban360].ReportPool.ServerReports s
                    Where 
                        s.ReportInputJson=@jsonInput  AND
						s.CompletionDateTime is null AND
						s.InsertDateTime>DATEADD(HOUR,-2,GETDATE())";
        }
        private string getIncompleteByUserIdQuery()
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
                    	s.IsInformed,
						s.HeaderType,
						s.DataType,
						s.ReportInputType,
						s.ReportInputJson,
						s.HandlerKey
                    From [Aban360].ReportPool.ServerReports s
                    Where 
                        s.UserId=@UserId AND
				 		s.InsertDateTime>DATEADD(MINUTE,-30,GETDATE()) AND
						s.CompletionDateTime is null";
        }
    }
}