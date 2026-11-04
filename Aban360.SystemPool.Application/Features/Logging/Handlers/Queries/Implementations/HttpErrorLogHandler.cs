using Aban360.Common.Exceptions;
using Aban360.Common.Timing;
using Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Conracts;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;
using Aban360.SystemPool.Persistence.Features.Logging.Queries.Contracts;

namespace Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Implementations;

internal sealed class HttpErrorLogHandler(IHttpErrorLogQueryService query) : IHttpErrorLogHandler
{
    public Task<IEnumerable<HttpErrorLogOutput>> Handle(HttpErrorLogInput input, CancellationToken cancellationToken)
    {
        if (input is null)
            throw new CustomValidationException("اطلاعات جست‌وجوی خطا ارسال نشده است.");

        int statusCodeFrom = input.StatusCodeClass?.Trim().ToUpperInvariant() switch
        {
            "4XX" => 400,
            "5XX" => 500,
            _ => throw new CustomValidationException("گروه کد وضعیت باید 4XX یا 5XX باشد.")
        };
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
        return query.Get(from, to.AddMinutes(1), statusCodeFrom, statusCodeFrom + 100,
            input.Page, input.PageSize, cancellationToken);
    }
}
