using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using DNTPersianUtils.Core;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Aban360.Common.Timing
{
    public static class CalculationDistanceDate
    {
        private static int DefaultDistance = 0;
        private static string DefaultDistanceText = ExceptionLiterals.Incalculable;
        public static CalcDistanceResultDto CalcDistance(string? fromDate, [Optional] string toDate, [Optional] bool canThrow, [Optional] object metaData)
        {
            DateOnly? from = fromDate.ToGregorianDateOnly();
            DateOnly? to = toDate == null ? DateOnly.FromDateTime(DateTime.Now) : toDate.ToGregorianDateOnly();


            if ((!from.HasValue || !to.HasValue) && canThrow)
            {
                string metaDataJson = "";
                if (metaData is { })
                {
                    metaDataJson = JsonSerializer.Serialize(metaData);
                }
                if (!from.HasValue && canThrow)
                    throw new InvalidDateException(ExceptionLiterals.InvalidFromDate + metaDataJson);
                if (!to.HasValue && canThrow)
                    throw new InvalidDateException(ExceptionLiterals.InvalidToDate + metaDataJson);
            }

            if (!from.HasValue)
            {
                if (!to.HasValue)
                {
                    return new CalcDistanceResultDto(true, ExceptionLiterals.InvalidFromAndToDate, DefaultDistance, DefaultDistanceText);
                }
                return new CalcDistanceResultDto(true, ExceptionLiterals.InvalidFromDate, DefaultDistance, DefaultDistanceText);
            }
            if (!to.HasValue)
            {
                return new CalcDistanceResultDto(true, ExceptionLiterals.InvalidToDate, DefaultDistance, DefaultDistanceText);
            }


            int totalDay = Math.Abs(to.Value.DayNumber - from.Value.DayNumber);
            string distance = ConvertDayToDate(totalDay);

            return new CalcDistanceResultDto(false, null, totalDay, distance);

            #region lastCode
            //DateOnly? persianDate = fromDate.ToGregorianDateOnly();
            //DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

            //if (!persianDate.HasValue && canThrow)
            //    throw new InvalidDateException(ExceptionLiterals.InvalidDate);

            //if (!persianDate.HasValue)
            //{
            //    return new CalcDistanceResultDto(true, ExceptionLiterals.InvalidFromDate, DefaultDistance, DefaultDistanceText);
            //}

            //int totalDay = currentDate.DayNumber - persianDate.Value.DayNumber;
            //string distance = ConvertDayToDate(totalDay);

            //return new CalcDistanceResultDto(false, null, totalDay, distance);
            /////
            #endregion
        }

        #region lastMethod
        //public static CalcDistanceResultDto CalcDistance(string? fromDate, string? toDate)
        //{
        //    DateOnly? from = fromDate.ToGregorianDateOnly();
        //    DateOnly? to = toDate.ToGregorianDateOnly();

        //    if (!from.HasValue)
        //    {
        //        if (!to.HasValue)
        //        {
        //            return new CalcDistanceResultDto(true, ExceptionLiterals.InvalidFromAndToDate, DefaultDistance, DefaultDistanceText);
        //        }
        //        return new CalcDistanceResultDto(true, ExceptionLiterals.InvalidFromDate, DefaultDistance, DefaultDistanceText);
        //    }
        //    if (!to.HasValue)
        //    {
        //        return new CalcDistanceResultDto(true, ExceptionLiterals.InvalidToDate, DefaultDistance, DefaultDistanceText);
        //    }


        //    int totalDay = Math.Abs(to.Value.DayNumber - from.Value.DayNumber);
        //    string distance = ConvertDayToDate(totalDay);

        //    return new CalcDistanceResultDto(false, null, totalDay, distance);
        //}
        #endregion
        public record CalcDistanceResultDto
        {
            public bool HasError { get; set; }
            public string? ErrorText { get; set; }
            public int Distance { get; set; }
            public string DistanceText { get; set; }
            public CalcDistanceResultDto(bool hasError, string? errorText, int distance, string distanceText)
            {
                HasError = hasError;
                ErrorText = errorText;
                Distance = distance;
                DistanceText = distanceText;
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
