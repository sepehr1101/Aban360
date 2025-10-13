using Aban360.Common.Db.Dapper;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;
using Aban360.SystemPool.Persistence.Features.Logging.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.SystemPool.Persistence.Features.Logging.Queries.Implementations
{
    internal sealed class LoggingGetByDateTimeQueryService : AbstractBaseConnection, ILoggingGetByDateTimeQueryService
    {
        public LoggingGetByDateTimeQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<IEnumerable<LoggingOutputDto>> Get(LoggingInputByDateTimeDto input)
        {
            string loggingQueryString = GetLoggingQuery();
            var @params = new
            {
                fromDateTime = input.FromDateTime,
                toDateTime = input.ToDateTime,
                logLevel = input.LogLevel.ToString()
            };
            IEnumerable<LoggingOutputDto> result = await _sqlConnection.QueryAsync<LoggingOutputDto>(loggingQueryString, @params);
            return result;
        }
        private string GetLoggingQuery()
        {
            return @"Select
                        Format(l.TimeStamp ,'yyyy/MM/dd', 'fa-IR') as DateJalali,
                        Format(l.TimeStamp,'HH:mm','fa-IR') as Time,
                    	l.Level as LogLevel,
                    	l.Message ,
                    	l.Exception,
                        l.Properties
                    From [Aban360].dbo.Logs l
                    Where 
                    	l.TimeStamp BETWEEN @fromDateTime AND @toDateTime AND
                    	l.Level=@logLevel";
        }
    }
}
