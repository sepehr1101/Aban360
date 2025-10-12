using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using DNTPersianUtils.Core;
using System.Runtime.InteropServices;

namespace Aban360.Common.Timing
{
    public static class CalculationDistanceDate
    {
        public static CalcDistanceResultDto? CalcDistance(string? date, [Optional] bool canThrow)
        {
            DateOnly? persianDate = date.ToGregorianDateOnly();
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

            if (!persianDate.HasValue && canThrow)
                throw new InvalidDateException(ExceptionLiterals.InvalidDate);

            if (!persianDate.HasValue)
            {
                return new CalcDistanceResultDto(true, true, ExceptionLiterals.InvalidFromDate);
            }

            int totalDay = currentDate.DayNumber - persianDate.Value.DayNumber;
            string distance = ConvertDayToDate(totalDay);

            return new CalcDistanceResultDto(totalDay, distance);
        }

        public static CalcDistanceResultDto CalcDistance(string? fromDate, string? toDate)
        {
            DateOnly? from = fromDate.ToGregorianDateOnly();
            DateOnly? to = toDate.ToGregorianDateOnly();

            if (!from.HasValue)
            {
                if (!to.HasValue)
                {
                    return new CalcDistanceResultDto(true, true, ExceptionLiterals.InvalidFromAndToDate, true);
                }
                return new CalcDistanceResultDto(true, true, ExceptionLiterals.InvalidFromDate);
            }
            if (!to.HasValue)
            {
                return new CalcDistanceResultDto(true, false, ExceptionLiterals.InvalidToDate, true);
            }


            int totalDay = Math.Abs(to.Value.DayNumber - from.Value.DayNumber);
            string distance = ConvertDayToDate(totalDay);

            return new CalcDistanceResultDto(totalDay, distance);
        }

        public record CalcDistanceResultDto
        {
            public bool HasError { get; set; }
            public bool? HasFromDateError { get; set; }
            public bool? HasToDateError { get; set; }
            public string? ErrorText { get; set; }
            public int Distance { get; set; }
            public string DistanceText { get; set; }
            public CalcDistanceResultDto(int distance, string distanceText)
            {
                HasError = false;
                HasFromDateError = false;
                HasToDateError = false;
                Distance = distance;
                DistanceText = distanceText;
            }
            public CalcDistanceResultDto(bool hasError, bool? hasFromDateError, string errorText, [Optional] bool? hasToDateError)
            {
                HasError = hasError;
                HasFromDateError = hasFromDateError;
                HasToDateError = hasToDateError;
                ErrorText = errorText;
                Distance = 0;
                DistanceText = "";
            }
            public CalcDistanceResultDto()
            {
            }
        }

        public static string ConvertDayToDate(int totalDay)
        {
            string years = (totalDay / (int)365) == 0 ? string.Empty : $"{totalDay / (int)365} سال -";
            int remainedYear = totalDay % 365;
            string months = (remainedYear / 30) == 0 ? string.Empty : $"{remainedYear / 30} ماه -";
            int days = remainedYear % 30;



            return $"{years} {months} {days} روز";
        }
    }
}
