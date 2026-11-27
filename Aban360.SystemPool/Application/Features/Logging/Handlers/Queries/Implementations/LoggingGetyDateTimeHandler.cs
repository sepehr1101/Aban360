using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Conracts;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;
using Aban360.SystemPool.Persistence.Features.Logging.Queries.Contracts;
using Aban360.Common.Timing;

namespace Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Implementations
{
    internal sealed class LoggingGetyDateTimeHandler : ILoggingGetyDateTimeHandler
    {
        private readonly ILoggingGetByDateTimeQueryService _loggingGetByDateTimeService;
        public LoggingGetyDateTimeHandler(ILoggingGetByDateTimeQueryService loggingGetByDateTimeService)
        {
            _loggingGetByDateTimeService = loggingGetByDateTimeService;
            _loggingGetByDateTimeService.NotNull(nameof(loggingGetByDateTimeService));
        }

        public async Task<IEnumerable<LoggingOutputDto>> Handle(LoggingInputByStringDto inputDto, CancellationToken cancellationToken)
        {
            if (inputDto is null)
                throw new CustomValidationException("اطلاعات جست‌وجوی لاگ ارسال نشده است.");
            var fromDateTime = PersianDateTimeParser.Parse(inputDto.FromDate, inputDto.FromTime, "تاریخ شروع", "ساعت شروع");
            var toDateTime = PersianDateTimeParser.Parse(inputDto.ToDate, inputDto.ToTime, "تاریخ پایان", "ساعت پایان");
            if (fromDateTime > toDateTime)
            {
                throw new InvalidDateException("تاریخ و ساعت شروع نباید بعد از تاریخ و ساعت پایان باشد.");
            }

            if (!Enum.IsDefined(inputDto.LogLevel))
                throw new CustomValidationException("سطح لاگ معتبر نیست؛ مقدار LogLevel باید بین 1 و 7 باشد. برای همهٔ سطح‌ها مقدار 7 ارسال کنید یا این فیلد را حذف کنید.");

            IEnumerable<LoggingOutputDto> result = await _loggingGetByDateTimeService.Get(new LoggingInputByDateTimeDto(fromDateTime, toDateTime, inputDto.LogLevel), cancellationToken);
            return result;
        }
    }
}
