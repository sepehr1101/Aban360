using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Application.Features.Processing.Helpers
{
    internal static class TariffStringChecker
    {
        internal static bool LessThanEq(string baseString, string @from)
        {
            return baseString.CompareTo(from) <= 0;
        }
        internal static bool IsGtFromLqTo(string baseString, string @from, string @to)
        {
            return baseString.CompareTo(from) > 0 && baseString.CompareTo(to) <= 0;
        }
        internal static bool IsBetween(string baseString, string @from, string @to)
        {
            return baseString.CompareTo(from) >= 0 && baseString.CompareTo(to) <= 0;
        }
        internal static bool IsBetween(double number, double min, double max)
        {
            return number >= min && number <= max;
        }
        internal static bool IsBetween(int baseZoneId, int zoneIdParam, string readingNumber, string fromNumber, string toNumber)
        {
            return
                baseZoneId == zoneIdParam &&
                readingNumber.Trim().CompareTo(fromNumber) >= 0 &&
                readingNumber.Trim().CompareTo(toNumber) <= 0;
        }
        internal static bool IsGardenOrDweltyAfter1400_12_24(int usageId, string nerkhDate1)
        {
            string baseDate = "1400/24/12";
            int[] usageIds = [25, 34];
            return usageIds.Contains(usageId) && nerkhDate1.CompareTo(baseDate) >= 0;
        }
        internal static bool IsLessThan1403_09_13(string nerkhDate2)
        {
            string baseDate = "1403/09/13";
            return nerkhDate2.CompareTo(baseDate) <= 0;
        }
        internal static bool MoreOrEq(this string date1, string date2)
        {
            DateOnly? from = date1.ToGregorianDateOnly();
            DateOnly? to = date2.ToGregorianDateOnly();
            if (!from.HasValue && !to.HasValue)
            {
                throw new BaseException(ExceptionLiterals.InvalidDate);
            }

            if (from.Value >= to.Value)
                return true;

            return false;
        }
        internal static bool IsMoreThan1404_01_01(string nerkhDate2)
        {
            string baseDate = "1404/01/01";
            return nerkhDate2.CompareTo(baseDate) >= 0;
        }
    }
}
