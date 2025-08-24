using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;
using Aban360.ReportPool.Persistence.Features.FlatReports.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.FlatReports.Queries.Implementations
{
    internal sealed class ServerReportsGetAllByThresholdService : AbstractBaseConnection, IServerReportsGetAllByThresholdService
    {
        public ServerReportsGetAllByThresholdService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<IEnumerable<ServerReportsGetAllDto>> Get(Guid userId,int threshold)
        {
            string serverReportsByIdQueryString = GetServerReportsByIdQuery();
            IEnumerable<ServerReportsGetAllDto> data = await _sqlConnection.QueryAsync<ServerReportsGetAllDto>(serverReportsByIdQueryString, new { userId=userId , threshold = threshold });
            return data;
        }
        private string GetServerReportsByIdQuery()
        {
            return @"Select 
                    	s.Id,
                    	s.ReportName,
						FORMAT(s.CompletionDateTime,'yyyy-MM-dd HH:mm:ss','fa-ir') as CompletionDateTimeJalali,
						FORMAT(s.ErrorDateTime,'yyyy-MM-dd HH:mm:ss','fa-ir') as ErrorDateTimeJalali,
						FORMAT(s.InsertDateTime,'yyyy-MM-dd HH:mm:ss','fa-ir') as InsertDateTimeJalali,
                    	s.IsInformed,
						IIF(s.CompletionDateTime IS Null,0,1) AS IsCompleted
                    From [Aban360].ReportPool.ServerReports s
                    Where 
                        s.UserId=@userId  AND
						s.CompletionDateTime >= DATEADD(DAY, -@threshold, GETDATE())";
        }
    }
}