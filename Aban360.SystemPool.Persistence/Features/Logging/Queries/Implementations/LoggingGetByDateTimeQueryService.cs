using Aban360.Common.Db.Dapper;
using Aban360.SystemPool.Domain.Contants;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;
using Aban360.SystemPool.Persistence.Features.Logging.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace Aban360.SystemPool.Persistence.Features.Logging.Queries.Implementations
{
    internal sealed class LoggingGetByDateTimeQueryService : AbstractBaseConnection, ILoggingGetByDateTimeQueryService
    {
        public LoggingGetByDateTimeQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<IEnumerable<LoggingOutputDto>> Get(LoggingInputByDateTimeDto input, CancellationToken cancellationToken = default)
        {
            string loggingQueryString = GetLoggingQuery(input.LogLevel);
            var @params = new
            {
                fromDateTime = input.FromDateTime,
                toDateTime = input.ToDateTime,
                logLevel = input.LogLevel.ToString()
            };
            using var connection = _sqlConnection;
            var command = new CommandDefinition(loggingQueryString, @params, cancellationToken: cancellationToken);
            var rows = await connection.QueryAsync<LoggingRow>(command);
            var calendar = new PersianCalendar();
            return rows.Select(row => new LoggingOutputDto
            {
                DateJalali = string.Format(CultureInfo.InvariantCulture, "{0:0000}/{1:00}/{2:00}",
                    calendar.GetYear(row.TimeStamp), calendar.GetMonth(row.TimeStamp), calendar.GetDayOfMonth(row.TimeStamp)),
                Time = row.TimeStamp.ToString("HH:mm", CultureInfo.InvariantCulture),
                LogLevel = row.LogLevel,
                Message = row.Message,
                Exception = row.Exception,
                Properties = row.Properties
            }).ToList();
        }
        private sealed record LoggingRow : LoggingOutputDto
        {
            public DateTime TimeStamp { get; set; }
        }
        private string GetLoggingQuery(LogLevelEnum logLevel)
        {
            string logLevelQueryPart = logLevel == LogLevelEnum.None ? "" : " AND l.Level=@logLevel";
            return @$"Select top 10000
                        l.TimeStamp,
                    	l.Level as LogLevel,
                    	l.Message ,
                    	l.Exception,
                        l.Properties
                    From [Aban360].dbo.Logs l
                    Where 
                    	l.TimeStamp BETWEEN @fromDateTime AND @toDateTime
                        {logLevelQueryPart}
                    Order By l.TimeStamp DESC";
        }
    }
}
