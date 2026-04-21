using Aban360.Common.Literals;
using DNTPersianUtils.Core;

namespace Aban360.Common.Timing
{
    public static class ConvertDate
    {
        public static string JalaliToGregorian(string dateJalali)
        {
            DateOnly? dateGregorian = dateJalali.ToGregorianDateOnly();
            if (dateGregorian.HasValue && DateValidation(dateJalali))
            {
                return dateGregorian.Value.ToString("yyyy-MM-dd");
            }

            return ExceptionLiterals.Incalculable;
        }
        public static DateTime GregorianStringToDateTime(string dateGregorian)
        {
            string[] values = dateGregorian.Split('-');
            DateTime date = new(Convert.ToInt32(values[0]), Convert.ToInt32(values[1]), Convert.ToInt32(values[2]));
            return date;
        }
        private static bool DateValidation(string dateJalali)
        {
            var part = dateJalali.Split('/');
            if (dateJalali.Length != 10 || part.Count() != 3 || part[1].Length != 2 || part[2].Length != 2)
            {
                return false;
            }
            return true;
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
