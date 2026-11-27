using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;
using Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Conracts;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;
using Aban360.SystemPool.Persistence.Features.Logging.Queries.Contracts;
using System.Globalization;

namespace Aban360.SystemPool.Application.Features.Logging.Services;
public interface IHttpLogQueriesService
{
    Task<HttpErrorLogDetailOutput?> HttpErrorLogGetDetail(long id, CancellationToken cancellationToken);
    Task<IEnumerable<HttpErrorLogOutput>> HttpErrorLogAsync(HttpErrorLogInput input, CancellationToken cancellationToken);
    Task<IEnumerable<HttpRequestReportOutput>> HttpRequestReportAsync(HttpRequestReportInput input, HttpRequestReportKind kind, CancellationToken cancellationToken);
}

internal sealed class HttpLogQueriesService(IHttpErrorLogQueryService query, IHttpRequestReportQueryService httpRequestReportquery) : IHttpLogQueriesService
{
    public Task<HttpErrorLogDetailOutput?> HttpErrorLogGetDetail(long id, CancellationToken cancellationToken) => query.GetDetail(id, cancellationToken);
    public Task<IEnumerable<HttpErrorLogOutput>> HttpErrorLogAsync(HttpErrorLogInput input, CancellationToken cancellationToken)
    {
        if (input is null)
            throw new CustomValidationException("اطلاعات جست‌وجوی خطا ارسال نشده است.");
        int statusCodeFrom = input.StatusCodeClass?.Trim().ToUpperInvariant() switch
        {
            "4XX" => 400,
            "5XX" => 500,
            _ => throw new CustomValidationException("گروه کد وضعیت باید 4XX یا 5XX باشد.")};
        var from = PersianDateTimeParser.Parse(input.FromDate, input.FromTime, "تاریخ شروع", "ساعت شروع");
        var to = PersianDateTimeParser.Parse(input.ToDate, input.ToTime, "تاریخ پایان", "ساعت پایان");
        if (from > to)
            throw new InvalidDateException("تاریخ و ساعت شروع نباید بعد از تاریخ و ساعت پایان باشد.");
        if (to - from > TimeSpan.FromDays(31))
            throw new InvalidDateException("بازه جست‌وجوی خطا نباید بیشتر از ۳۱ روز باشد.");
        if (to > DateTime.MaxValue.AddMinutes(-1))
            throw new InvalidDateException("تاریخ پایان خارج از بازه قابل جست‌وجو است.");
        if (input.Page < 1 || input.PageSize is < 1 or > 200)
            throw new CustomValidationException("شماره صفحه باید مثبت و اندازه صفحه بین ۱ تا ۲۰۰ باشد.");
        // Include all seconds of the last minute selected by the client.
        return query.Get(from, to.AddMinutes(1), statusCodeFrom, statusCodeFrom + 100, input.Page, input.PageSize, cancellationToken);
    }

    public Task<IEnumerable<HttpRequestReportOutput>> HttpRequestReportAsync(HttpRequestReportInput input, HttpRequestReportKind kind, CancellationToken cancellationToken)
    {
        var from = HttpRequestReportParse(input.FromDate, input.FromTime);
        var to = HttpRequestReportParse(input.ToDate, input.ToTime);
        if (from >= to || to - from > TimeSpan.FromDays(31) || input.Page < 1 || input.PageSize is < 1 or > 200 || !Enum.IsDefined(kind))
            throw new InvalidDateException(ExceptionLiterals.InvalidDate);
        // Summary always returns its single aggregate, independently of paging.
        if (kind == HttpRequestReportKind.Summary)
            input = new HttpRequestReportInput
            {
                UserId = input.UserId,
                RouteTemplate = input.RouteTemplate,
                Page = 1,
                PageSize = 1
            };
        return httpRequestReportquery.Get(from, to, input, kind, cancellationToken);
    }

    private static DateTime HttpRequestReportParse(string date, string time)
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
            if (date.Length != 10 || date[4] != '/' || date[7] != '/' || date.Where((_, i) => i != 4 && i != 7).Any(c => c < '0' || c > '9') || !TimeOnly.TryParseExact(time, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var clock))
                throw new FormatException();
            var parts = date.Split('/').Select(p => int.Parse(p, CultureInfo.InvariantCulture)).ToArray();
            return new PersianCalendar().ToDateTime(parts[0], parts[1], parts[2], clock.Hour, clock.Minute, 0, 0);
        }
        catch (Exception ex)when (ex is ArgumentException or FormatException or OverflowException or NullReferenceException)
        {
            throw new InvalidDateException(ExceptionLiterals.InvalidDate);
        }
    }
}
