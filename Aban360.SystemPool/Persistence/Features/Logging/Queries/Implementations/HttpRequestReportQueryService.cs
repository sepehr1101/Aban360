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
        var sql = BuildSql(kind, input.UserId is not null, input.RouteTemplate is not null);
        using var connection = _sqlConnection;
        return await connection.QueryAsync<HttpRequestReportOutput>(new CommandDefinition(sql, new
        {
            from, to, input.UserId, input.RouteTemplate, input.PageSize,
            offset = ((long)input.Page - 1) * input.PageSize,
            template = "HTTP {RequestMethod} {RouteTemplate} completed {StatusCode} in {ElapsedMilliseconds} ms; TraceId {TraceId}; UserId {UserId}"
        }, cancellationToken: cancellationToken));
    }

    internal static string BuildSql(HttpRequestReportKind kind, bool filterUser, bool filterRoute)
    {
        if (!Enum.IsDefined(kind)) throw new ArgumentOutOfRangeException(nameof(kind));
        var needRoute = kind is HttpRequestReportKind.Routes or HttpRequestReportKind.Requests || filterRoute;
        var needUser = kind != HttpRequestReportKind.Summary || filterUser;
        var projection = new List<string>
        {
            "TRY_CONVERT(int, P.value('(properties/property[@key=\"StatusCode\"]/text())[1]', 'nvarchar(30)')) StatusCode",
            "TRY_CONVERT(float, P.value('(properties/property[@key=\"ElapsedMilliseconds\"]/text())[1]', 'nvarchar(100)')) Elapsed"
        };
        if (needUser)
            projection.Add("NULLIF(P.value('(properties/property[@key=\"UserId\"]/text())[1]', 'nvarchar(256)'), '') UserId");
        if (needRoute)
            projection.Add("P.value('(properties/property[@key=\"RouteTemplate\"]/text())[1]', 'nvarchar(2048)') RouteTemplate");
        if (kind is HttpRequestReportKind.Routes or HttpRequestReportKind.Requests)
            projection.Add("P.value('(properties/property[@key=\"RequestMethod\"]/text())[1]', 'nvarchar(20)') RequestMethod");
        if (kind == HttpRequestReportKind.Requests)
            projection.Add("P.value('(properties/property[@key=\"TraceId\"]/text())[1]', 'nvarchar(256)') TraceId");

        var group = kind switch
        {
            HttpRequestReportKind.Routes => "RequestMethod, RouteTemplate",
            HttpRequestReportKind.Users => "UserId",
            _ => ""
        };
        var aggregate = $"""
            SELECT {(group.Length > 0 ? group + "," : "")}
                COUNT_BIG(*) RequestCount,
                {(kind != HttpRequestReportKind.Summary ? "MAX(CASE WHEN StatusCode BETWEEN 400 AND 599 THEN Id END) ErrorId," : "")}
                COALESCE(SUM(CONVERT(bigint, CASE WHEN StatusCode >= 500 THEN 1 ELSE 0 END)),0) ServerErrorCount,
                COALESCE(AVG(Elapsed),0) AverageMilliseconds,
                COALESCE(MAX(Elapsed),0) MaximumMilliseconds,
                COALESCE(SUM(Elapsed),0) TotalMilliseconds
            FROM Filtered
            {(group.Length > 0 ? "GROUP BY " + group : "")}
            """;
        var selection = kind == HttpRequestReportKind.Requests
            ? "SELECT Id, CASE WHEN StatusCode BETWEEN 400 AND 599 THEN Id END ErrorId, TimeStamp, RequestMethod, RouteTemplate, UserId, TraceId, StatusCode, Elapsed AverageMilliseconds, Elapsed MaximumMilliseconds, Elapsed TotalMilliseconds, CONVERT(bigint,1) RequestCount, CONVERT(bigint,CASE WHEN StatusCode >= 500 THEN 1 ELSE 0 END) ServerErrorCount FROM Filtered"
            : aggregate;
        var order = kind switch
        {
            HttpRequestReportKind.Requests => "r.MaximumMilliseconds DESC, r.Id DESC",
            HttpRequestReportKind.Routes => "r.MaximumMilliseconds DESC, r.RequestMethod, r.RouteTemplate",
            HttpRequestReportKind.Users => "r.AverageMilliseconds DESC, r.UserId",
            _ => "r.RequestCount DESC"
        };
        var includeDetails = kind != HttpRequestReportKind.Summary;
        var userId = kind == HttpRequestReportKind.Routes ? "error.UserId" : "r.UserId";
        // The existing MSSqlServer sink stores structured properties as XML.
        return $$"""
            WITH Source AS (
                SELECT Id, TimeStamp, TRY_CONVERT(xml, Properties) P
                FROM [Aban360].dbo.Logs
                WHERE TimeStamp >= @from AND TimeStamp < @to
                  AND MessageTemplate = @template
            ), Parsed AS (
                SELECT Id, TimeStamp,
                    {{string.Join(",\n                    ", projection)}}
                FROM Source
            ), Filtered AS (
                SELECT * FROM Parsed WHERE Elapsed >= 0 AND StatusCode IS NOT NULL
                  {{(filterUser ? "AND UserId = @UserId" : "")}}
                  {{(filterRoute ? "AND RouteTemplate = @RouteTemplate" : "")}}
            ), Results AS ({{selection}})
            SELECT r.*, COUNT_BIG(*) OVER() TotalGroups
                {{(includeDetails ? ", error.UserId ErrorUserId, u.DisplayName" : "")}}
            FROM Results r
            {{(includeDetails ? $"LEFT JOIN Filtered error ON error.Id = r.ErrorId\n            LEFT JOIN [Aban360].UserPool.[User] u ON u.Id = TRY_CONVERT(uniqueidentifier, {userId})" : "")}}
            ORDER BY {{order}} OFFSET @offset ROWS FETCH NEXT @PageSize ROWS ONLY
            OPTION (RECOMPILE);
            """;
    }
}
