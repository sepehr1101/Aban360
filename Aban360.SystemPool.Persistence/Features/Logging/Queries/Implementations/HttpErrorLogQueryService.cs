using System.Globalization;
using Aban360.Common.Db.Dapper;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;
using Aban360.SystemPool.Persistence.Features.Logging.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.SystemPool.Persistence.Features.Logging.Queries.Implementations;

internal sealed class HttpErrorLogQueryService(IConfiguration configuration)
    : AbstractBaseConnection(configuration), IHttpErrorLogQueryService
{
    private const string RequestTemplate =
        "HTTP {RequestMethod} {RouteTemplate} completed {StatusCode} in {ElapsedMilliseconds} ms; TraceId {TraceId}; UserId {UserId}";

    public async Task<HttpErrorLogDetailOutput?> GetDetail(long id, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnection;
        var detail = await connection.QuerySingleOrDefaultAsync<HttpErrorLogDetailOutput>(
            new CommandDefinition(DetailSql, new { id, template = RequestTemplate }, cancellationToken: cancellationToken));
        if (detail is null) return null;

        var calendar = new PersianCalendar();
        detail.DateJalali = string.Format(CultureInfo.InvariantCulture, "{0:0000}/{1:00}/{2:00}",
            calendar.GetYear(detail.TimeStamp), calendar.GetMonth(detail.TimeStamp), calendar.GetDayOfMonth(detail.TimeStamp));
        detail.Time = detail.TimeStamp.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
        return detail;
    }

    public async Task<IEnumerable<HttpErrorLogOutput>> Get(DateTime from, DateTime toExclusive,
        int statusCodeFrom, int statusCodeToExclusive, int page, int pageSize, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnection;
        var rows = await connection.QueryAsync<ErrorRow>(new CommandDefinition(Sql, new
        {
            from, toExclusive, statusCodeFrom, statusCodeToExclusive, pageSize,
            offset = ((long)page - 1) * pageSize,
            template = RequestTemplate
        }, cancellationToken: cancellationToken));
        var calendar = new PersianCalendar();
        return rows.Select(row =>
        {
            row.DateJalali = string.Format(CultureInfo.InvariantCulture, "{0:0000}/{1:00}/{2:00}",
                calendar.GetYear(row.TimeStamp), calendar.GetMonth(row.TimeStamp), calendar.GetDayOfMonth(row.TimeStamp));
            row.Time = row.TimeStamp.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
            return (HttpErrorLogOutput)row;
        }).ToList();
    }

    private sealed record ErrorRow : HttpErrorLogOutput
    {
        public DateTime TimeStamp { get; set; }
    }

    internal const string DetailSql = """
        SELECT l.Id, l.TimeStamp, l.Level, l.Message, l.Exception,
            status.StatusCode,
            p.XmlProperties.value('(properties/property[@key="RequestMethod"]/text())[1]', 'nvarchar(20)') RequestMethod,
            p.XmlProperties.value('(properties/property[@key="RouteTemplate"]/text())[1]', 'nvarchar(2048)') RouteTemplate,
            p.XmlProperties.value('(properties/property[@key="TraceId"]/text())[1]', 'nvarchar(256)') TraceId,
            NULLIF(p.XmlProperties.value('(properties/property[@key="UserId"]/text())[1]', 'nvarchar(256)'), '') UserId,
            TRY_CONVERT(float, p.XmlProperties.value('(properties/property[@key="ElapsedMilliseconds"]/text())[1]', 'nvarchar(64)')) ElapsedMilliseconds
        FROM [Aban360].dbo.Logs l
        CROSS APPLY (VALUES (TRY_CONVERT(xml, l.Properties))) p(XmlProperties)
        CROSS APPLY (VALUES (TRY_CONVERT(int,
            p.XmlProperties.value('(properties/property[@key="StatusCode"]/text())[1]', 'nvarchar(30)')))) status(StatusCode)
        WHERE l.Id = @id AND l.MessageTemplate = @template AND status.StatusCode BETWEEN 400 AND 599;
        """;

    internal const string Sql = """
        WITH Source AS (
            SELECT Id, TimeStamp, Message, TRY_CONVERT(xml, Properties) P
            FROM [Aban360].dbo.Logs
            WHERE TimeStamp >= @from AND TimeStamp < @toExclusive
              AND MessageTemplate = @template
        ), Parsed AS (
            SELECT Id, TimeStamp, Message,
                TRY_CONVERT(int, P.value('(properties/property[@key="StatusCode"]/text())[1]', 'nvarchar(30)')) StatusCode,
                P.value('(properties/property[@key="RequestMethod"]/text())[1]', 'nvarchar(20)') RequestMethod,
                P.value('(properties/property[@key="RouteTemplate"]/text())[1]', 'nvarchar(2048)') RouteTemplate,
                P.value('(properties/property[@key="TraceId"]/text())[1]', 'nvarchar(256)') TraceId,
                NULLIF(P.value('(properties/property[@key="UserId"]/text())[1]', 'nvarchar(256)'), '') UserId
            FROM Source
        )
        SELECT Id, TimeStamp, Message, StatusCode, RequestMethod, RouteTemplate, TraceId, UserId
        FROM Parsed
        WHERE StatusCode >= @statusCodeFrom AND StatusCode < @statusCodeToExclusive
        ORDER BY TimeStamp DESC, Id DESC
        OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;
        """;
}
