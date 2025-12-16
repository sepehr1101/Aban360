using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffStringChecker;

namespace Aban360.OldCalcPool.Application.Features.Processing.Helpers
{
    internal static class TariffRuleChecker
    {
        private static bool CheckConditions(int id, int[] values)
        {
            return values.Contains(id);
        }
        internal static bool IsVillage(int zoneId)
        {
            return zoneId > 140000;
        }
        internal static bool IsGardenOrDwelty(int usageId)
        {
            return CheckConditions(usageId, [ 1, 3, 25, 34]);
        }
        internal static bool IsDomestic(int usageId)
        {
            return CheckConditions(usageId, [0, 1, 3]);
        }
        internal static bool IsDomesticCategory(int usageId)
        {
            return CheckConditions(usageId, [0, 1, 3, 25, 34]);
        }
        internal static bool IsDomesticWithoutUnspecified(int usageId)
        {
            return CheckConditions(usageId, [1, 3]);
        }
        internal static bool IsReligious(int usageId)
        {
            return CheckConditions(usageId, [10, 12, 13, 29, 32]);
        }
        internal static bool IsIndustrial(int usageId)
        {
            return CheckConditions(usageId, [4]);
        }
        internal static bool IsCharityOrSchool(int usageId)
        {
            return CheckConditions(usageId, [8, 7, 12, 13, 29, 30, 32]);
        }
        internal static bool IsHandoverDiscount(int branchTypeId)
        {
            return CheckConditions(branchTypeId, [3, 6, 7]);
        }
        internal static bool IsUnderSocialService(int branchTypeId)
        {
            return CheckConditions(branchTypeId, [ 6, 7]);
        }
        internal static bool IsReligiousWithCharity(int usageId)
        {
            return CheckConditions(usageId, [12, 13, 29, 30, 32]);
        }
        internal static bool IsVillageCollectorMeter(int usageId)
        {
            return CheckConditions(usageId, [15]);
        }
        internal static bool IsGardenAndResidence(int usageId)
        {
            return CheckConditions(usageId, [25, 34]);
        }
        internal static bool IsUsageConstructor(int usageId)
        {
            return CheckConditions(usageId, [5, 39]);
        }
        internal static bool IsTankerSale(int usageId)
        {
            return CheckConditions(usageId, [14]);
        }
        internal static bool IsEducationOrBath(int usageId)
        {
            return CheckConditions(usageId, [7, 8, 41, 11]);
        }
        internal static bool IsBath(int usageId)
        {
            return CheckConditions(usageId, [11]);
        }
        internal static bool IsSpecialEducation(int usageId, bool isSpecial)
        {
            return CheckConditions(usageId, [7, 8]) && isSpecial;
        }
        internal static bool IsMullah(int branchTypeId)
        {
            return CheckConditions(branchTypeId, [3]);
        }
        internal static bool IsConstruction(int branchTypeId)
        {
            return CheckConditions(branchTypeId, [4]);
        }
        internal static bool IsSpecialIndustrial(int branchTypeId)
        {
            return CheckConditions(branchTypeId, [8]);
        }

        /// <summary>
        /// روستاهایی که اگرچه در ناحیه روستایی هستند اما محاسبه بصورت شهری
        /// </summary>
        /// <param name="zoneId"></param>
        /// <param name="villageCode"></param>
        /// <returns></returns>
        internal static bool RuralButIsMetro(int zoneId, int villageCode)
        {
            int[] village142618 = [1037, 1038, 1039];
            int[] village144311 = [1090, 1093];
            int[] village144411 = [1016];
            int[] village143012 = [1010, 1013, 1016, 1017, 1029];
            int[] village142714 = [1019];
            int[] village141911 = [1034];
            int[] village141914 = [1061];
            int[] village141611 = [1006];

            return
                (zoneId == 142618 && village142618.Contains(villageCode)) ||
                (zoneId == 144311 && village144311.Contains(villageCode)) ||
                (zoneId == 144411 && village144411.Contains(villageCode)) ||
                (zoneId == 143012 && village143012.Contains(villageCode)) ||
                (zoneId == 142714 && village142714.Contains(villageCode)) ||
                (zoneId == 141911 && village141911.Contains(villageCode)) ||
                (zoneId == 141914 && village141914.Contains(villageCode)) ||
                (zoneId == 141611 && village141611.Contains(villageCode));
        }
        internal static bool RuralButIsMetro(int zoneId, string readingNumber)
        {
            return
                IsBetween(141911, zoneId, readingNumber, "10340005001", "10340908000") ||
                IsBetween(141914, zoneId, readingNumber, "10610001000", "10610800000") ||
                IsBetween(144015, zoneId, readingNumber, "60000000000", "60999999999") ||
                IsBetween(144015, zoneId, readingNumber, "62000000000", "62999999999") ||
                IsBetween(144016, zoneId, readingNumber, "22000000000", "22999999999") ||
                IsBetween(144016, zoneId, readingNumber, "24000000000", "24999999999") ||
                IsBetween(141611, zoneId, readingNumber, "10060001000", "10060797000") ||
                IsBetween(144411, zoneId, readingNumber, "10160001000", "10161024000") ||
                IsBetween(143411, zoneId, readingNumber, "10930000000", "10939999999") ||
                IsBetween(143411, zoneId, readingNumber, "71093000000", "71093999999") ||
                IsBetween(143411, zoneId, readingNumber, "81093000000", "81093999999") ||
                IsBetween(143411, zoneId, readingNumber, "10900000000", "10909999999") ||
                IsBetween(143411, zoneId, readingNumber, "71090000000", "71090999999") ||
                IsBetween(143411, zoneId, readingNumber, "81090000000", "81090999999") ||
                IsBetween(143012, zoneId, readingNumber, "10100000000", "10109999999") ||
                IsBetween(143012, zoneId, readingNumber, "10170000000", "10179999999") ||
                IsBetween(143012, zoneId, readingNumber, "10160000000", "10169999999") ||
                IsBetween(143012, zoneId, readingNumber, "10290000000", "10299999999") ||
                IsBetween(143012, zoneId, readingNumber, "10130000000", "10139999999") ||
                IsBetween(142211, zoneId, readingNumber, "10340000000", "10349999999") ||
                IsBetween(142211, zoneId, readingNumber, "10370000000", "10379999999") ||
                IsBetween(142211, zoneId, readingNumber, "10380000000", "10389999999") ||
                IsBetween(142215, zoneId, readingNumber, "10220000000", "10229999999");

        }

        internal static bool IsDolatabadOrHabibabadWithConditionEshtrak(int zoneId, string readingNumber)
        {
            return
                (zoneId == 134013 && IsBetween(readingNumber, "57000000", "57999999")) ||
                (zoneId == 134016 && IsBetween(readingNumber, "57000000", "57999999")) ||
                 MetroButIsRural(zoneId, readingNumber, 4);
        }
        internal static bool MetroButIsRural(int zoneId, string readingNumber, int thresholdSkip)
        {
            if (string.IsNullOrWhiteSpace(readingNumber)) return false;
            if (readingNumber.Trim().Length < thresholdSkip) return false;

            string shortReadingNumber = readingNumber.Trim().Substring(0, thresholdSkip);
            if (zoneId == 132220 &&
                (IsBetween(shortReadingNumber, "1610", "1628") ||
                IsBetween(shortReadingNumber, "1633", "1648") ||
                IsBetween(shortReadingNumber, "1651", "1661") ||
                IsBetween(shortReadingNumber, "6042", "6052") ||
                IsBetween(shortReadingNumber, "6060", "6072"))
                )
                return true;

            if (zoneId == 132211 &&
                 (IsBetween(shortReadingNumber, "1103", "1108") ||
                 IsBetween(shortReadingNumber, "1109", "1113") ||
                 IsBetween(shortReadingNumber, "1143", "1165") ||
                 IsBetween(shortReadingNumber, "1161", "1184") ||
                 IsBetween(shortReadingNumber, "1403", "1499") ||
                 IsBetween(shortReadingNumber, "1450", "1472") ||
                 IsBetween(shortReadingNumber, "1574", "1599"))
               )
                return true;

            return false;
        }
        internal static bool CheckZero(double duration, double monthlyConsumption, string? vaj)
        {
            return duration <= 0 ||
                   monthlyConsumption == 0 ||
                   string.IsNullOrWhiteSpace(vaj);
        }
        internal static (bool, int) HasVillageCode(string villageId)
        {
            if (string.IsNullOrWhiteSpace(villageId) || villageId.Length < 5)
            {
                return (false, 0);
            }
            bool canParse = int.TryParse(villageId.Substring(0, 4), out int villageCode);
            return (canParse, villageCode);
        }
    }
}
