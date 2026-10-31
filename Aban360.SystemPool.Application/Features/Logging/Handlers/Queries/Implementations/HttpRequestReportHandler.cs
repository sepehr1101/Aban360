using System.Globalization;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Conracts;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;
using Aban360.SystemPool.Persistence.Features.Logging.Queries.Contracts;

namespace Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Implementations;

internal sealed class HttpRequestReportHandler(IHttpRequestReportQueryService query) : IHttpRequestReportHandler
{
    public Task<IEnumerable<HttpRequestReportOutput>> Handle(HttpRequestReportInput input,
        HttpRequestReportKind kind, CancellationToken cancellationToken)
    {
        var from = Parse(input.FromDate, input.FromTime);
        var to = Parse(input.ToDate, input.ToTime);
        if (from >= to || to - from > TimeSpan.FromDays(31) || input.Page < 1 ||
            input.PageSize is < 1 or > 200 || !Enum.IsDefined(kind))
            throw new InvalidDateException(ExceptionLiterals.InvalidDate);
        // Summary always returns its single aggregate, independently of paging.
        if (kind == HttpRequestReportKind.Summary)
            input = new HttpRequestReportInput { UserId = input.UserId, RouteTemplate = input.RouteTemplate, Page = 1, PageSize = 1 };
        return query.Get(from, to, input, kind, cancellationToken);
    }

    private static DateTime Parse(string date, string time)
    {
        try
        {
            // Accept Persian and Arabic digits, while keeping a strict date/time shape.
            string Normalize(string text) => string.Concat(text.Select(c => c switch
            {
                >= '۰' and <= '۹' => (char)('0' + c - '۰'),
                >= '٠' and <= '٩' => (char)('0' + c - '٠'),
                _ => c
            }));
            date = Normalize(date);
            time = Normalize(time);
            if (date.Length != 10 || date[4] != '/' || date[7] != '/' ||
                date.Where((_, i) => i != 4 && i != 7).Any(c => c < '0' || c > '9') ||
                !TimeOnly.TryParseExact(time, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var clock))
                throw new FormatException();
            var parts = date.Split('/').Select(p => int.Parse(p, CultureInfo.InvariantCulture)).ToArray();
            return new PersianCalendar().ToDateTime(parts[0], parts[1], parts[2], clock.Hour, clock.Minute, 0, 0);
        }
        catch (Exception ex) when (ex is ArgumentException or FormatException or OverflowException or NullReferenceException)
        {
            throw new InvalidDateException(ExceptionLiterals.InvalidDate);
        }
    }
}
