using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;
using DNTPersianUtils.Core;
using static Aban360.Common.Timing.CalculationDistanceDate;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffStringChecker;

namespace Aban360.OldCalcPool.Application.Features.Processing.Helpers
{
    internal static class TariffDateOperations
    {
        internal static DateOnly ConvertJalaliToGregorian(string dateJalali)
        {
            DateOnly? grogorianDate = dateJalali.ToGregorianDateOnly();
            if (!grogorianDate.HasValue)
            {
                throw new BaseException(ExceptionLiterals.InvalidDate);
            }

            return grogorianDate.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// 
        internal static int PartTime(string date1, string date2, string previousDate, string currentDate, object metaData)
        {
            int partMethod = 0;
            partMethod = IsBetween(previousDate, date1, date2) && IsBetween(currentDate, date1, date2) ?
               GetDistance(previousDate, currentDate, metaData) : partMethod;

            partMethod = previousDate.CompareTo(date1) <= 0 && IsBetween(currentDate, date1, date2) ?
                GetDistance(date1, currentDate, metaData) : partMethod;

            partMethod = currentDate.CompareTo(date2) >= 0 && IsBetween(previousDate, date1, date2) ?
                GetDistance(previousDate, date2, metaData) : partMethod;

            partMethod = previousDate.CompareTo(date1) <= 0 && currentDate.CompareTo(date2) >= 0 ?
                GetDistance(date1, date2, metaData) : partMethod;

            return partMethod;
        }
        internal static int GetDistance(string fromDate, string toDate, object metaData)
        {
            CalcDistanceResultDto calcDistance = CalcDistance(fromDate, toDate, true, metaData);
            if (calcDistance.HasError)
            {
                throw new TariffDateException(ExceptionLiterals.Incalculable);
            }
            return calcDistance.Distance;
        }       
    }
}
