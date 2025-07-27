using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.SystemPool.Application.Features.Loging.Handlers.Queries.Conracts;
using Aban360.SystemPool.Domain.Features.Loging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Loging.Dto.Output;
using Aban360.SystemPool.Persistence.Features.Loging.Queries.Contracts;
using DNTPersianUtils.Core;
using System.Globalization;

namespace Aban360.SystemPool.Application.Features.Loging.Handlers.Queries.Implementations
{
    internal sealed class LogingGetyDateTimeHandler : ILogingGetyDateTimeHandler
    {
        private readonly ILogingGetByDateTimeQueryService _logingGetByDateTimeService;
        public LogingGetyDateTimeHandler(ILogingGetByDateTimeQueryService logingGetByDateTimeService)
        {
            _logingGetByDateTimeService = logingGetByDateTimeService;
            _logingGetByDateTimeService.NotNull(nameof(logingGetByDateTimeService));
        }

        public async Task<IEnumerable<LogingOutputDto>> Handle(LogingInputByStringDto inputDto, CancellationToken cancellationToken)
        {
            DateOnly? from = inputDto.FromDate.ToGregorianDateOnly();
            DateOnly? to = inputDto.ToDate.ToGregorianDateOnly();
            if (!from.HasValue || !to.HasValue)
            {
                throw new BaseException(ExceptionLiterals.InvalidDate);
            }

            string fromDateTimeString = $"{from.Value:yyyy/MM/dd} {inputDto.FromTime}";
            string toDateTimeString = $"{to.Value:yyyy/MM/dd} {inputDto.ToTime}";

            DateTime fromDateTime = DateTime.ParseExact(fromDateTimeString, "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);
            DateTime toDateTime = DateTime.ParseExact(toDateTimeString, "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);

            IEnumerable<LogingOutputDto> result = await _logingGetByDateTimeService.Get(new LogingInputByDateTimeDto(fromDateTime, toDateTime, inputDto.LogLevel));
            result.ForEach(x =>
            {
                string date = x.DateTimeGrogorian.ToString("yyyy/MM/dd");
                x.DateJalali = date.ToGregorianDateOnly().ToShortPersianDateString();
                x.Time = x.DateTimeGrogorian.ToString("HH:mm:ss");
            });
            return result;
        }
    }
}
