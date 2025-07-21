using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Commands;
using Aban360.ReportPool.Persistence.Features.FlatReports.Commands.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.FlatReports.Commands.Implementations
{
    internal sealed class ServerReportsUpdateService : AbstractBaseConnection, IServerReportsUpdateService
    {
        public ServerReportsUpdateService(IConfiguration configuration)
            : base(configuration)
        { }

        public async void Update(ServerReportsUpdateDto input)
        {
            string UpdateQuery = GetServerReportsUpdateQuery();
            var @params = new
            {
                id = input.Id,
                completionDateTime = input.CompletionDateTime,
                errorDateTime = input.ErrorDateTime,
                isInformed = input.IsInformed,
                reportPath=input.ReportPath,
            };
            await _sqlConnection.ExecuteAsync(UpdateQuery, @params);
        }
        private string GetServerReportsUpdateQuery()
        {
            return @"Update [Aban360].ReportPool.ServerReports
                     Set
                     	CompletionDateTime=@completionDateTime,
                     	ErrorDateTime=@errorDateTime,
                     	IsInformed=@isInformed,
                        ReportPath=@reportPath
                    Where Id=@id";
        }
    }
}
