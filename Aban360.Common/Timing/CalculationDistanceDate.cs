using Aban360.Common.Literals;
using DNTPersianUtils.Core;

namespace Aban360.Common.Timing
{
    public static class CalculationDistanceDate
    {
        public static string CalcDistance(string? date)
        {
            DateOnly? persianDate = date.ToGregorianDateOnly();
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

            if (!persianDate.HasValue)
                return ExceptionLiterals.InvalidDate;


            int totalDay=currentDate.DayNumber - persianDate.Value.DayNumber;
            return ConvertDaysToDate(totalDay);

        }

        public static string CalcDistance(string? fromDate, string? toDate)
        {
            DateOnly? from = fromDate.ToGregorianDateOnly();
            DateOnly? to = toDate.ToGregorianDateOnly();

            if (!from.HasValue && !to.HasValue)
            {
                return ExceptionLiterals.InvalidDate;
            }

            int totalDay = to.Value.DayNumber - from.Value.DayNumber;
            return totalDay.ToString();
        }

        public static string ConvertDaysToDate(int totalDay)
        {
            string years = (totalDay / (int)365)==0?string.Empty:$"{totalDay / (int)365} سال -";
            int remainedYear = totalDay % 365;
            string months = (remainedYear / 30) == 0 ? string.Empty : $"{remainedYear / 30} ماه -";
            int days = remainedYear % 30;
            
           

            return $"{years} {months} {days} روز";
        }
    }
}
