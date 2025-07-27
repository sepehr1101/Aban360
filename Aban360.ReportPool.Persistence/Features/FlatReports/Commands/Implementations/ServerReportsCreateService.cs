using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Commands;
using Aban360.ReportPool.Persistence.Features.FlatReports.Commands.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.FlatReports.Commands.Implementations
{
    internal sealed class ServerReportsCreateService : AbstractBaseConnection, IServerReportsCreateService
    {
        public ServerReportsCreateService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Create(ServerReportsCreateDto input)
        {
            string createQuery = GetServerReportsCreateQuery();
            var @params = new
            {
                id = input.Id,
                userId = input.UserId,
                reportName = input.ReportName,
                connectionId = input.ConnectionId,
                isInformed = false,
                insertDateTime = DateTime.Now,
                headerType=input.HeaderType,
                dataType=input.DataType,
                reportInputType = input.ReportInputType,
                reportInputJson = input.ReportInputJson,
                handlerKey = input.HandlerKey,
            };
            await _sqlConnection.ExecuteAsync(createQuery, @params);
        }

        private string GetServerReportsCreateQuery()
        {
           return @"Insert Into [Aban360].ReportPool.ServerReports(
                            Id,UserId,ReportName,ConnectionId,InsertDateTime,
                            IsInformed,HeaderType,DataType,ReportInputType,ReportInputJson,HandlerKey)
                    Values(@id,@userId,@reportName,@connectionId,@insertDateTime,
                           @isInformed,@headerType,@dataType,@reportInputType,@reportInputJson,@handlerKey)";
        }
    }
}
