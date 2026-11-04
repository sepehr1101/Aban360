using System.Globalization;
using Aban360.Common.Exceptions;

namespace Aban360.Common.Timing;

public static class PersianDateTimeParser
{
    public static DateTime Parse(string? date, string? time,
        string dateLabel = "تاریخ", string timeLabel = "ساعت")
    {
        if (string.IsNullOrWhiteSpace(date))
            throw new InvalidDateException($"وارد کردن {dateLabel} الزامی است؛ تاریخ شمسی را با قالب yyyy/MM/dd ارسال کنید.");
        if (string.IsNullOrWhiteSpace(time))
            throw new InvalidDateException($"وارد کردن {timeLabel} الزامی است؛ ساعت را با قالب HH:mm ارسال کنید.");

        static string Normalize(string text) => string.Concat(text.Select(c => c switch
        {
            >= '۰' and <= '۹' => (char)('0' + c - '۰'),
            >= '٠' and <= '٩' => (char)('0' + c - '٠'),
            _ => c
        }));

        date = Normalize(date);
        time = Normalize(time);
        if (date.Length != 10 || date[4] != '/' || date[7] != '/' ||
            date.Where((_, i) => i != 4 && i != 7).Any(c => c < '0' || c > '9'))
            throw new InvalidDateException($"قالب {dateLabel} معتبر نیست؛ تاریخ شمسی را با قالب yyyy/MM/dd، مانند 1405/08/11، ارسال کنید.");
        if (!TimeOnly.TryParseExact(time, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var clock))
            throw new InvalidDateException($"{timeLabel} معتبر نیست؛ از قالب ۲۴ ساعتهٔ HH:mm، مانند 09:30، استفاده کنید.");

        try
        {
            var parts = date.Split('/').Select(p => int.Parse(p, CultureInfo.InvariantCulture)).ToArray();
            return new PersianCalendar().ToDateTime(parts[0], parts[1], parts[2], clock.Hour, clock.Minute, 0, 0);
        }
        catch (ArgumentException)
        {
            throw new InvalidDateException($"{dateLabel} در تقویم شمسی معتبر نیست؛ سال، ماه و روز را بررسی کنید.");
        }
    }
}
