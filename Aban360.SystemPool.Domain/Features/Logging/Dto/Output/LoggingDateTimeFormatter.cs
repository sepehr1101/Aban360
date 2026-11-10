using System.Globalization;

namespace Aban360.SystemPool.Domain.Features.Logging.Dto.Output;

internal static class LoggingDateTimeFormatter
{
    public static string? ToJalali(DateTime? value)
    {
        var calendar = new PersianCalendar();
        if (value is not { } date || date < calendar.MinSupportedDateTime)
            return null;

        return string.Format(CultureInfo.InvariantCulture,
            "{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}",
            calendar.GetYear(date), calendar.GetMonth(date), calendar.GetDayOfMonth(date),
            date.Hour, date.Minute, date.Second);
    }
}
