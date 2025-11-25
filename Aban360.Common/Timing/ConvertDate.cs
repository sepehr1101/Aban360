using Aban360.Common.Literals;
using DNTPersianUtils.Core;

namespace Aban360.Common.Timing
{
    public static class ConvertDate
    {
        public static string JalaliToGregorian(string dateJalali)
        {
            DateOnly? dateGregorian = dateJalali.ToGregorianDateOnly();
            if (dateGregorian.HasValue)
            {
                return dateGregorian.Value.ToString("yyyy-MM-dd");
            }

            return ExceptionLiterals.Incalculable;
        }
        public static string GregorianToJalali(string dateGregorian)
        {
            if (string.IsNullOrWhiteSpace(dateGregorian))
                return ExceptionLiterals.Incalculable;

            string[] formats = {
                     "yyyy-MM-dd",
                     "yyyy-MM-dd HH:mm:ss",
                     "yyyy/MM/dd",
                     "yyyy/MM/dd HH:mm:ss",
                     "MM/dd/yyyy",
                     "MM/dd/yyyy HH:mm:ss",
                     "yyyyMMdd",
                     "dd-MM-yyyy",
                     "dd/MM/yyyy"
            };

            if (DateTime.TryParseExact(dateGregorian.Trim(), formats,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateTime date))
            {
                return date.ToShortPersianDateTimeString(); 
            }

            if (DateTime.TryParse(dateGregorian, out date))
                return date.ToShortPersianDateString();

            return ExceptionLiterals.Incalculable;
        }

    }
}
