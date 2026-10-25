using System.Globalization;
using Aban360.Common.Exceptions;

namespace Aban360.PaymentPool.Application.Features.BankReceipt.Implementations
{
    internal static class BankReceiptDateRange
    {
        public static (DateTime FromUtc, DateTime ToUtcExclusive) Get(string fromDate, string toDate)
        {
            DateTime from = ParseDate(fromDate), to = ParseDate(toDate);
            if (from > to) throw new BaseException("از تاریخ نباید بعد از تا تاریخ باشد.");
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Dubai");
            try
            {
                return (TimeZoneInfo.ConvertTimeToUtc(from, timezone), TimeZoneInfo.ConvertTimeToUtc(to.AddDays(1), timezone));
            }
            catch (ArgumentException) { throw new BaseException("بازه تاریخ خارج از محدوده قابل پشتیبانی است."); }
        }
        private static DateTime ParseDate(string value)
        {
            string normalized = value?.Replace("/", string.Empty) ?? string.Empty;
            if (!(value?.Length == 8 || (value?.Length == 10 && value[4] == '/' && value[7] == '/')))
                throw new BaseException("تاریخ باید با قالب yyyyMMdd یا yyyy/MM/dd باشد.");
            if (normalized.Length != 8 || normalized.Any(c => c < '0' || c > '9'))
                throw new BaseException("تاریخ باید شمسی و با قالب yyyyMMdd یا yyyy/MM/dd باشد.");
            try
            {
                return new PersianCalendar().ToDateTime(int.Parse(normalized[..4], CultureInfo.InvariantCulture),
                    int.Parse(normalized.Substring(4, 2), CultureInfo.InvariantCulture),
                    int.Parse(normalized.Substring(6, 2), CultureInfo.InvariantCulture), 0, 0, 0, 0);
            }
            catch (ArgumentOutOfRangeException) { throw new BaseException("تاریخ شمسی معتبر نیست."); }
        }
    }
}
