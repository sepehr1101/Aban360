using Aban360.Common.Db.Dapper;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;
using Aban360.SystemPool.Persistence.Features.Logging.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.SystemPool.Persistence.Features.Logging.Queries.Implementations;

internal sealed class HttpRequestReportQueryService(IConfiguration configuration)
    : AbstractBaseConnection(configuration), IHttpRequestReportQueryService
{
    public async Task<IEnumerable<HttpRequestReportOutput>> Get(DateTime from, DateTime to,
        HttpRequestReportInput input, HttpRequestReportKind kind, CancellationToken cancellationToken)
    {
        var group = kind switch
        {
            HttpRequestReportKind.Routes => "RequestMethod, RouteTemplate",
            HttpRequestReportKind.Users => "UserId",
            _ => ""
        };
        var aggregate = $"""
            SELECT {(group.Length > 0 ? group + "," : "")}
                COUNT_BIG(*) RequestCount,
                COALESCE(SUM(CONVERT(bigint, CASE WHEN StatusCode >= 500 THEN 1 ELSE 0 END)),0) ServerErrorCount,
                COALESCE(AVG(Elapsed),0) AverageMilliseconds,
                COALESCE(MAX(Elapsed),0) MaximumMilliseconds,
                COALESCE(SUM(Elapsed),0) TotalMilliseconds
            FROM Filtered
            {(group.Length > 0 ? "GROUP BY " + group : "")}
            """;
        var selection = kind == HttpRequestReportKind.Requests
            ? "SELECT Id, TimeStamp, RequestMethod, RouteTemplate, UserId, TraceId, StatusCode, Elapsed AverageMilliseconds, Elapsed MaximumMilliseconds, Elapsed TotalMilliseconds, CONVERT(bigint,1) RequestCount, CONVERT(bigint,CASE WHEN StatusCode >= 500 THEN 1 ELSE 0 END) ServerErrorCount FROM Filtered"
            : aggregate;
        var order = kind switch
        {
            HttpRequestReportKind.Requests => "MaximumMilliseconds DESC, Id DESC",
            HttpRequestReportKind.Routes => "MaximumMilliseconds DESC, RequestMethod, RouteTemplate",
            HttpRequestReportKind.Users => "AverageMilliseconds DESC, UserId",
            _ => "RequestCount DESC"
        };
        // The existing MSSqlServer sink stores structured properties as XML.
        var sql = $$"""
            WITH Source AS (
                SELECT Id, TimeStamp, TRY_CONVERT(xml, Properties) P
                FROM [Aban360].dbo.Logs
                WHERE TimeStamp >= @from AND TimeStamp < @to
                  AND MessageTemplate = @template
            ), Parsed AS (
                SELECT Id, TimeStamp,
                    P.value('(properties/property[@key="RequestMethod"]/text())[1]', 'nvarchar(20)') RequestMethod,
                    P.value('(properties/property[@key="RouteTemplate"]/text())[1]', 'nvarchar(2048)') RouteTemplate,
                    NULLIF(P.value('(properties/property[@key="UserId"]/text())[1]', 'nvarchar(256)'), '') UserId,
                    P.value('(properties/property[@key="TraceId"]/text())[1]', 'nvarchar(256)') TraceId,
                    TRY_CONVERT(int, P.value('(properties/property[@key="StatusCode"]/text())[1]', 'nvarchar(30)')) StatusCode,
                    TRY_CONVERT(float, P.value('(properties/property[@key="ElapsedMilliseconds"]/text())[1]', 'nvarchar(100)')) Elapsed
                FROM Source
            ), Filtered AS (
                SELECT * FROM Parsed WHERE Elapsed >= 0 AND StatusCode IS NOT NULL
                  AND (@UserId IS NULL OR UserId = @UserId)
                  AND (@RouteTemplate IS NULL OR RouteTemplate = @RouteTemplate)
            ), Results AS ({{selection}})
            SELECT *, COUNT_BIG(*) OVER() TotalGroups FROM Results
            ORDER BY {{order}} OFFSET @offset ROWS FETCH NEXT @PageSize ROWS ONLY;
            """;
        using var connection = _sqlConnection;
        return await connection.QueryAsync<HttpRequestReportOutput>(new CommandDefinition(sql, new
        {
            from, to, input.UserId, input.RouteTemplate, input.PageSize,
            offset = ((long)input.Page - 1) * input.PageSize,
            template = "HTTP {RequestMethod} {RouteTemplate} completed {StatusCode} in {ElapsedMilliseconds} ms; TraceId {TraceId}; UserId {UserId}"
        }, cancellationToken: cancellationToken));
    }
}
