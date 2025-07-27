using Aban360.Common.Db.Dapper;
using Aban360.SystemPool.Domain.Features.Loging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Loging.Dto.Output;
using Aban360.SystemPool.Persistence.Features.Loging.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.SystemPool.Persistence.Features.Loging.Queries.Implementations
{
    internal sealed class LogingGetByDateTimeQueryService : AbstractBaseConnection, ILogingGetByDateTimeQueryService
    {
        public LogingGetByDateTimeQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<IEnumerable<LogingOutputDto>> Get(LogingInputByDateTimeDto input)
        {

            string logingQueryString = GetLogingQuery();
            var @params = new
            {
                fromDateTime = input.FromDateTime,
                toDateTime = input.ToDateTime,
                logLevel = input.LogLevel.ToString()
            };
            IEnumerable<LogingOutputDto> result = await _sqlConnection.QueryAsync<LogingOutputDto>(logingQueryString, @params);
            return result;
        }
        private string GetLogingQuery()
        {
            return @"Select
                     	l.TimeStamp as DateTimeGrogorian,
                    	l.Level as LogLevel,
                    	l.Message ,
                    	l.Exception
                    From [Aban360].dbo.Logs l
                    Where 
                    	l.TimeStamp BETWEEN @fromDateTime AND @toDateTime AND
                    	l.Level=@logLevel";
        }
    }
}
