using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using DNTPersianUtils.Core;
using System.Runtime.InteropServices;
using static Aban360.Common.Timing.CalculationDistanceDate;

namespace Aban360.CalculationPool.Application.Features.Base
{
    internal abstract class BaseOldTariffEngine : BaseExpressionCalculator
    {
        int monthDays = 30;

        protected BaseOldTariffEngine()
        {
            
        }

        /// <summary>
        /// تنها تابع با دسترسی پابلیک بابت محاسبه تک رکورد جدول نرخ
        /// </summary>
        /// <returns>مقدار خروجی بعد از اتمام نوشتن کد، اصلاح شود</returns>

        public BaseOldTariffEngineOutputDto CalculateWaterBill(NerkhGetDto nerkh, AbAzadFormulaDto abAzad, ZaribGetDto zarib, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, double dailyAverage, string currentDateJalali, int consumption, int duration, int _olgoo, int finalDomesticCount, [Optional] int? c, [Optional] IEnumerable<int>? tagIds)
        {
            DateOnly previousDate = ConvertJalaliToGregorian(meterInfo.PreviousDateJalali);
            DateOnly currentDate = ConvertJalaliToGregorian(currentDateJalali);
            nerkh.DailyAverageConsumption = dailyAverage;
            (nerkh, nerkh.Duration, nerkh.PartialConsumption) = CalcPartial(nerkh, previousDate, currentDate, dailyAverage, consumption, duration);

            int olgoo = GetOlgoo(nerkh.Date2, _olgoo);
            bool isVillageCalculation = IsVillage(customerInfo.ZoneId);
            double monthlyConsumption = nerkh.DailyAverageConsumption * monthDays;
            decimal multiplierAbBaha = Multiplier(zarib, olgoo, IsDomesticCategory(customerInfo.UsageId), isVillageCalculation, monthlyConsumption, customerInfo.BranchType);

            CalculateAbBahaOutputDto abBahaResult = CalculateAbBaha(nerkh, customerInfo, meterInfo, zarib, abAzad, currentDateJalali, isVillageCalculation, monthlyConsumption, olgoo, multiplierAbBaha, c, tagIds);
            (double, double) boodje = CalculateBoodje(nerkh, customerInfo, currentDateJalali, monthlyConsumption, olgoo, consumption, duration, finalDomesticCount);
            double fazelab = CalculateFazelab(nerkh.Date1, nerkh.Date2, nerkh.Duration, customerInfo, abBahaResult.AbBahaAmount, currentDateJalali, false);
            (int, double) hotSeasonAbBaha = CalcHotSeasonAbBaha(nerkh, abBahaResult.AbBahaAmount, customerInfo, monthlyConsumption);
            (int, double) hotSeasonFazelab = CalcHotSeasonFaselab(nerkh, customerInfo, fazelab, monthlyConsumption);
            double avarez = CalculateAvarez(nerkh, customerInfo, monthlyConsumption);
            double javani = CalculateJavaniJamiat(nerkh, customerInfo, abBahaResult.AbBahaAmount, monthlyConsumption, olgoo);
                        
            //Discounts
            double abBahaDiscount = CalculateItemDiscount(customerInfo, nerkh, olgoo, (long)abBahaResult.AbBahaAmount, false, multiplierAbBaha, finalDomesticCount);
            double fazelabDiscount = CalculateFazelabDiscount(abBahaDiscount, fazelab, customerInfo, nerkh);// CalculateItemDiscount(customerInfo, nerkh, olgoo, (long)fazelab, false, multiplierAbBaha);
            double hotSeasonAbDiscount = CalculateFasleGarmDiscount(nerkh, abBahaDiscount, hotSeasonAbBaha, customerInfo);//CalculateItemDiscount(customerInfo, nerkh, olgoo, (long)hotSeasonAbBaha, false, multiplierAbBaha);
            double hotSeasonFazelabDiscount = CalculateFasleGarmDiscount(nerkh, fazelabDiscount, hotSeasonFazelab, customerInfo);// CalculateItemDiscount(customerInfo, nerkh, olgoo, (long)hotSeasonFazelab, false, multiplierAbBaha);
            double avarezDiscount = 0;//CalculateItemDiscount(customerInfo, nerkh, olgoo, (long)avarez);
            double javaniDiscount = 0;// CalculateItemDiscount(customerInfo, nerkh, olgoo, (long)javani, false, multiplierAbBaha);
            double boodjeDiscount = CalculateBoodjeDiscount(customerInfo,abBahaDiscount,boodje, nerkh);
            return new BaseOldTariffEngineOutputDto(abBahaResult, fazelab, hotSeasonAbBaha.Item2, hotSeasonFazelab.Item2, boodje.Item1, boodje.Item2, abBahaDiscount, hotSeasonAbDiscount, fazelabDiscount, 0, avarez, javani,
                0, 0, avarezDiscount, javaniDiscount, boodjeDiscount);
        }

        /// <summary>
        /// محاسبه فرمول
        /// </summary>
        /// <param name="formula">متن فرمول که در آن X متوسط مصرف ماهانه است</param>
        /// <param name="monthlyAverageConsumption">متوسط مصرف ماهانه</param>
        /// <returns></returns>
        private double CalcFormulaByRate(string formula, double monthlyAverageConsumption, int olgoo, [Optional] int? c, [Optional] IEnumerable<int> tagIds)
        {
            object parameters = new { X = monthlyAverageConsumption, C = c, S=olgoo, tags = tagIds.ToArray() };
            double value = Eval<double>(formula, parameters);
            return value;
        }

        private bool CheckConditions(int id, int[] values)
        {
            return values.Contains(id);
        }
        private bool IsVillage(int zoneId)
        {
            return zoneId > 140000;
        }
        private bool IsDomestic(int usageId)
        {
            return CheckConditions(usageId, [0, 1, 3]);
        }
        private bool IsDomesticCategory(int usageId)
        {
            return CheckConditions(usageId, [0, 1, 3, 25, 34]);
        }
        private bool IsDomesticWithoutUnspecified(int usageId)
        {
            return CheckConditions(usageId, [1, 3]);
        }
        private bool IsReligious(int usageId)
        {
            return CheckConditions(usageId, [10, 12, 13, 29, 32]);
        }
        private bool IsIndustrial(int usageId)
        {
            return CheckConditions(usageId, [4]);
        }
        private bool IsCharityOrSchool(int usageId)
        {
            return CheckConditions(usageId, [8, 7, 12, 13, 29, 30, 32]);
        }
        private bool IsHandoverDiscount(int branchTypeId)
        {
            return CheckConditions(branchTypeId, [3, 6, 7]);
        }
        private bool IsReligiousWithCharity(int usageId)
        {
            return CheckConditions(usageId, [12, 13, 29, 30, 32]);
        }
        bool IsVillageCollectorMeter(int usageId)
        {
            return CheckConditions(usageId, [12, 13, 29, 30, 32]);
        }
        private bool IsGardenAndResidence(int usageId)
        {
            return CheckConditions(usageId, [25, 34]);
        }
        private bool IsUsageConstructor(int usageId)
        {
            return CheckConditions(usageId, [5, 39]);
        }
        private bool IsTankerSaleAndVillage(int usageId)
        {
            return CheckConditions(usageId, [14, 15]);
        }
        private bool IsTankerSaleAndHousehold(int usageId)
        {
            return CheckConditions(usageId, [14, 15, 19]);
        }
        private bool IsTankerSale(int usageId)
        {
            return CheckConditions(usageId, [14]);
        }
        private bool IsEducation(int usageId)
        {
            return CheckConditions(usageId, [7, 8, 41]);
        }
        private bool IsEducationOrBath(int usageId)
        {
            return CheckConditions(usageId, [7, 8, 41, 11]);
        }
        public bool IsSpecialEducation(int usageId, bool isSpecial)
        {
            return CheckConditions(usageId, [7,8]) && isSpecial;
        }

        private bool IsConstruction(int branchTypeId)
        {
            return CheckConditions(branchTypeId, [4]);
        }
        private bool IsSpecialIndustrial(int branchTypeId)
        {
            return CheckConditions(branchTypeId, [8]);
        }
        private bool HasDiscountBranch(int branchTypeId)
        {
            return CheckConditions(branchTypeId, [3, 6, 7]);
        }

        /// <summary>
        /// روستاهایی که اگرچه در ناحیه روستایی هستند اما محاسبه بصورت شهری
        /// </summary>
        /// <param name="zoneId"></param>
        /// <param name="villageCode"></param>
        /// <returns></returns>
        private bool RuralButIsMetro(int zoneId, int villageCode)
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
        private bool IsGreater(string baseString, string @from)
        {
            return baseString.CompareTo(from) > 0;
        }

        private bool LessThanEq(string baseString, string @from)
        {
            return baseString.CompareTo(from) <= 0;
        }
        private bool IsGtFromLqTo(string baseString, string @from, string @to)
        {
            return baseString.CompareTo(from) > 0 && baseString.CompareTo(to) <= 0;
        }

        private bool IsBetween(string baseString, string @from, string @to)
        {
            return baseString.CompareTo(from) >= 0 && baseString.CompareTo(to) <= 0;
        }
        private bool IsBetween(int number, int min, int max)
        {
            return number >= min && number <= max;
        }
        private bool IsBetween(double number, double min, double max)
        {
            return number >= min && number <= max;
        }
        private bool IsBetween(ulong number, ulong min, ulong max)
        {
            return number >= min && number <= max;
        }

        private (double, double, bool) MultiplyCalculation(double abBaha, double oldAbBaha, double multiplier)
        {
            return (abBaha * multiplier, oldAbBaha * multiplier, true);
        }

        private bool IsDolatabadOrHabibabadWithConditionEshtrak(int zoneId, string readingNumber)
        {
            return
                (zoneId == 134013 && IsBetween(readingNumber, "57000000", "57999999")) ||
                (zoneId == 134016 && IsBetween(readingNumber, "57000000", "57999999")) ||
                 MetroButIsRural(zoneId, readingNumber, 4);
        }
        private bool MetroButIsRural(int zoneId, string readingNumber, int thresholdSkip)
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
        private decimal Multiplier(ZaribGetDto zarib, int olgo, bool isDomestic, bool isVillage, double monthlyConsumption, int branchType)
        {
            double zbSelection = 1;

            if (IsConstruction(branchType) && !isVillage)
            {
                return zarib.Zb;
            }
            if (isVillage && isDomestic)
            {
                return zarib.Zarib_baha;
            }
            else if (!isDomestic && !isVillage)
            {
                return zarib.Zb;
            }
            else if (!isDomestic && isVillage)
            {
                return zarib.Zb_r;
            }
            else if (isDomestic && !isVillage)
            {
                if (IsBetween(monthlyConsumption, 0, 5))
                    return zarib.Zb1;
                else if (IsBetween(monthlyConsumption, 5, 10))
                    return zarib.Zb2;
                else if (IsBetween(monthlyConsumption, 10, olgo))
                    return zarib.Zb3;
                else if (IsBetween(monthlyConsumption, olgo, olgo * 1.5))
                    return zarib.Zb4;
                else if (IsBetween(monthlyConsumption, olgo * 1.5, olgo * 2))
                    return zarib.Zb5;
                else if (IsBetween(monthlyConsumption, olgo * 2, olgo * 3))
                    return zarib.Zb6;
                else if (monthlyConsumption > olgo * 3)
                    return zarib.Zb7;
            }

            return 1;
        }
        private bool IsLessThan1401_12_28(string nerkhDate2)
        {
            string baseDate = "1401/12/28";
            return nerkhDate2.CompareTo(baseDate) < 0;
        }
        private bool IsLessThan1403_12_30(string nerkhDate2)
        {
            string baseDate = "1403/12/30";
            return nerkhDate2.CompareTo(baseDate) < 0;
        }
        private bool IsMoreThan1404_01_01(string nerkhDate2)
        {
            string baseDate = "1404/01/01";
            return nerkhDate2.CompareTo(baseDate) >= 0;
        }
        private bool IsMoreThan1398_12_29(string nerkhDate2)
        {
            string baseDate = "1398/12/29";
            return nerkhDate2.CompareTo(baseDate) > 0;

        }

        private bool CheckZero(double duration, double monthlyConsumption, string? vaj)
        {
            return duration <= 0 ||
                   monthlyConsumption == 0 ||
                   string.IsNullOrWhiteSpace(vaj);
        }

        /// <summary>
        /// محاسبه آب بها 
        /// </summary>
        /// <returns>عدد محاسبه شده‌ی آب‌بها</returns>
        private CalculateAbBahaOutputDto CalculateAbBaha(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, ZaribGetDto zarib, AbAzadFormulaDto abAzad8And39, string currentDateJalali, bool isVillageCalculation, double monthlyConsumption, int _olgoo, decimal multiplierAbBaha, [Optional] int? c, [Optional] IEnumerable<int> tagIds)
        {
            double abBahaAmount = 0, oldAbBahaAmount = 0, abBahaFromExpression = 0, oldAbBahaZarib = 1.15;
            double duration = nerkh.Duration;
            abBahaFromExpression = CalcFormulaByRate(nerkh.Vaj, monthlyConsumption,_olgoo, c, tagIds);
            (double, double) abBahaValues = (0, 0);

            if (CheckZero(duration, monthlyConsumption, nerkh.Vaj))
            {
                return new CalculateAbBahaOutputDto(0, (0, 0));
            }

            if (IsGardenOrDweltyAfter1400_12_24OrIsDomestic(customerInfo, nerkh) &&
                !IsReligious(customerInfo.UsageId) &&
                !IsConstruction(customerInfo.BranchType))
            {
                abBahaFromExpression = CalcFormulaByRate(nerkh.Vaj, monthlyConsumption, _olgoo, c, tagIds);
                abBahaAmount = abBahaFromExpression * nerkh.PartialConsumption;

                if (IsLessThan1403_09_13AndOvajNotZero(nerkh))
                {
                    double oldAbBahaFromExpression = CalcFormulaByRate(nerkh.OVaj, monthlyConsumption, _olgoo, c, tagIds);
                    oldAbBahaAmount = nerkh.PartialConsumption * oldAbBahaFromExpression * oldAbBahaZarib;
                }

                if (IsLessThan1403_09_13(nerkh.Date2) &&
                    monthlyConsumption <= _olgoo &&
                    abBahaAmount > oldAbBahaAmount &&
                    IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
                    !IsConstruction(customerInfo.BranchType))
                {
                    //?????
                    abBahaAmount = oldAbBahaAmount;
                }
            }
            else
            {
                // foxpro:1139
                if (HasCapacityAndNotConstruction(customerInfo) ||
                    IsReligious(customerInfo.UsageId))
                {
                    double contractualCapacityInDuration = ((double)customerInfo.ContractualCapacity / monthDays) * duration;

                    if (IsCharitySchoolOrConsumptionGtCapacity(nerkh, customerInfo, contractualCapacityInDuration))
                    {
                        double disallowedPartialConsumption = nerkh.PartialConsumption - contractualCapacityInDuration;
                        double allowedPartialConsumption = contractualCapacityInDuration;

                        if (nerkh.PartialConsumption < contractualCapacityInDuration ||
                            IsReligiousAndZeroCapacity(customerInfo))
                        {
                            disallowedPartialConsumption = 0;
                            allowedPartialConsumption = nerkh.PartialConsumption;
                        }//L 1153

                        (long, long) _2Amount = Get2Amount(nerkh, customerInfo, abAzad8And39, abBahaFromExpression, _olgoo);

                        abBahaValues.Item1 = _2Amount.Item1 * allowedPartialConsumption;
                        abBahaValues.Item2 = _2Amount.Item2 * disallowedPartialConsumption;
                        abBahaAmount = abBahaValues.Item1 + abBahaValues.Item2;
                    }
                    else
                    {
                        abBahaAmount = nerkh.PartialConsumption * abBahaFromExpression;
                    }
                }
                else
                {
                    if (!IsConstruction(customerInfo.BranchType))
                    {
                        abBahaAmount = nerkh.PartialConsumption * abBahaFromExpression;
                    }
                }
            }
            if (IsConstruction(customerInfo.BranchType))
            {
                abBahaAmount = CalcFormulaByRate(abAzad8And39.Formula, monthlyConsumption, _olgoo,  c, tagIds) * nerkh.PartialConsumption;
            }
            //L 1553
            //L 1558

            if (IsVillageDomesticNotConstruction(customerInfo) &&
                !IsRuralButIsMetro(customerInfo) &&
                abBahaAmount != 0)
            {//L 1578
                float multiplier = IsLessThan1403_09_13OrMonthlyBelowEqOlgoo(nerkh, monthlyConsumption, _olgoo) ? 0.5f : 0.65f;
                (abBahaAmount, oldAbBahaAmount, isVillageCalculation) = MultiplyCalculation(abBahaAmount, oldAbBahaAmount, multiplier);
            }

            //L 1600 approximately
            if (IsDolatabadOrHabibabadAndDomesticAndNotConstruction(customerInfo, abBahaAmount))
            {//L 1604
                float multiplier = IsLessThan1403_09_13OrMonthlyBelowEqOlgoo(nerkh, monthlyConsumption, _olgoo) ? 0.5f : 0.65f;
                (abBahaAmount, oldAbBahaAmount, isVillageCalculation) = MultiplyCalculation(abBahaAmount, oldAbBahaAmount, multiplier);
            }//foxpro:1620

            abBahaAmount = abBahaAmount * (double)multiplierAbBaha;
            oldAbBahaAmount = oldAbBahaAmount * (double)multiplierAbBaha;// foxpro:1755
            abBahaValues = CheckAbBahaValues(abBahaAmount, abBahaValues);

            return new CalculateAbBahaOutputDto(abBahaAmount, abBahaValues);

            #region Local Functions
            bool IsLessThan1403_09_13AndOvajNotZero(NerkhGetDto nerkh)
            {
                return !string.IsNullOrWhiteSpace(nerkh.OVaj) &&
                                    nerkh.OVaj.Trim() != "0" &&
                                    IsLessThan1403_09_13(nerkh.Date2);
            }

            bool IsLessThan1403_09_13OrMonthlyBelowEqOlgoo(NerkhGetDto nerkh, double monthlyConsumption, int _olgoo)
            {
                return IsLessThan1403_09_13(nerkh.Date2) || monthlyConsumption <= _olgoo;
            }

            bool IsDolatabadOrHabibabadAndDomesticAndNotConstruction(CustomerInfoOutputDto customerInfo, double abBahaAmount)
            {
                return IsDolatabadOrHabibabadWithConditionEshtrak(customerInfo.ZoneId, customerInfo.ReadingNumber) &&
                       IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
                       !IsConstruction(customerInfo.BranchType) &&
                       abBahaAmount != 0;
            }

            bool IsGardenOrDweltyAfter1400_12_24OrIsDomestic(CustomerInfoOutputDto customerInfo, NerkhGetDto nerkh)
            {
                return IsGardenOrDweltyAfter1400_12_24(customerInfo.UsageId, nerkh.Date1) || IsDomestic(customerInfo.UsageId);
            }

            bool HasCapacityAndNotConstruction(CustomerInfoOutputDto customerInfo)
            {
                return (customerInfo.ContractualCapacity > 0 && !IsConstruction(customerInfo.BranchType));
            }

            bool IsRuralButIsMetro(CustomerInfoOutputDto customerInfo)
            {
                var (hasVillageCode, villageCode) = HasVillageCode(customerInfo.VillageId);
                if (!hasVillageCode)
                {
                    return false;
                }
                return RuralButIsMetro(customerInfo.ZoneId, customerInfo.ReadingNumber) ||
                       RuralButIsMetro(customerInfo.ZoneId, villageCode);
            }

            bool IsVillageDomesticNotConstruction(CustomerInfoOutputDto customerInfo)
            {
                return IsVillage(customerInfo.ZoneId) &&
                       IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
                       !IsConstruction(customerInfo.BranchType);
            }

            bool IsReligiousAndZeroCapacity(CustomerInfoOutputDto customerInfo)
            {
                return customerInfo.ContractualCapacity == 0 &&
                       IsReligiousWithCharity(customerInfo.UsageId);
            }

            (double, double) CheckAbBahaValues(double abBahaAmount, (double, double) abBahaValues)
            {
                if (abBahaValues == (0, 0))
                {
                    abBahaValues = (abBahaAmount, 0);
                }

                return abBahaValues;
            }

            bool IsCharitySchoolOrConsumptionGtCapacity(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, double contractualCapacityInDuration)
            {
                return nerkh.PartialConsumption > contractualCapacityInDuration ||
                       IsCharityOrSchool(customerInfo.UsageId);
            }

            (long, long) Get2Amount(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, AbAzadFormulaDto abAzad8And39, double abBahaFromExpression, int olgoo)
            {
                if (IsReligiousWithCharity(customerInfo.UsageId))
                {
                    return IsConstruction(customerInfo.BranchType) ? (450000, 450000) : Get2PartAmount(nerkh.Date2);//  foxpro:1178
                }
                return GetEducationOrBathMultiplier(customerInfo.UsageId, nerkh.Date1, nerkh.Date2, customerInfo.IsSpecial, (long)CalcFormulaByRate(abAzad8And39.Formula, monthlyConsumption, olgoo, c, tagIds), abBahaFromExpression);//Azad:39
            }
            #endregion
        }
        private int GetOlgoo(string nerkhDate2, int olgo)
        {
            string baseDate = "1403/12/30";
            return nerkhDate2.CompareTo(baseDate) <= 0 ? 14 : olgo;
        }
        private (NerkhGetDto, int, double) CalcPartial(NerkhGetDto nerkh, DateOnly previousDate, DateOnly currentDate, double dailyAverage, int consumption, int duration)
        {
            DateOnly fromDate = ConvertJalaliToGregorian(nerkh.Date1);
            DateOnly toDate = ConvertJalaliToGregorian(nerkh.Date2);

            DateOnly startSegment = fromDate > previousDate ? fromDate : previousDate;
            DateOnly endSegment = toDate < currentDate ? toDate : currentDate;

            nerkh.Date1 = startSegment.ToDateTime(TimeOnly.MinValue).ToShortPersianDateString();
            nerkh.Date2 = endSegment.ToDateTime(TimeOnly.MinValue).ToShortPersianDateString();

            int durationPartial = endSegment.DayNumber - startSegment.DayNumber;
            double partialConsumption = (double)consumption / (double)duration * durationPartial;
            return (nerkh, durationPartial, partialConsumption);
        }
        private DateOnly ConvertJalaliToGregorian(string dateJalali)
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
        private int PartTime(string date1, string date2, string previousDate, string currentDate, object metaData)
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
        private int GetDistance(string fromDate, string toDate, object metaData)
        {
            CalcDistanceResultDto calcDistance = CalculationDistanceDate.CalcDistance(fromDate, toDate, true, metaData);
            if (calcDistance.HasError)
            {
                throw new TariffDateException(ExceptionLiterals.Incalculable);
            }
            return calcDistance.Distance;
        }
        private (int,double) CalcHotSeasonAbBaha(NerkhGetDto nerkh, double abBahaAmount, CustomerInfoOutputDto customerInfo, double monthlyConsumption)
        {
            if (IsDomesticCategory(customerInfo.UsageId) && 
                !IsConstruction(customerInfo.BranchType) && 
                monthlyConsumption <= 25)
            {
                return (0, 0);
            }
            string date_02_31 = "/02/31";
            string date_06_31 = "/06/31";
            string hotSeasonStart = nerkh.Date2.Substring(0, 4) + date_02_31;
            string hotSeasonEnd = nerkh.Date2.Substring(0, 4) + date_06_31;

            int hotSeasonDuration = PartTime(hotSeasonStart, hotSeasonEnd, nerkh.Date1, nerkh.Date2, new { BillId = customerInfo.BillId, ZoneId = customerInfo.ZoneId, UsageId = customerInfo.UsageId });
            double hotSeasonAmount= hotSeasonDuration > 0 ? (int)((hotSeasonDuration * abBahaAmount / nerkh.Duration) * 0.2) : 0;
            return (hotSeasonDuration, hotSeasonAmount);
        }

        private (int,double) CalcHotSeasonFaselab(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, double fazelabAmount, double monthlyConsumption)
        {
            if (IsDomesticCategory(customerInfo.UsageId) && monthlyConsumption <= 25)
            {
                return (0,0);
            }
            string date_02_31 = "/02/31";
            string date_06_31 = "/06/31";
            string hotSeasonStart = nerkh.Date2.Substring(0, 4) + date_02_31;
            string hotSeasonEnd = nerkh.Date2.Substring(0, 4) + date_06_31;
            int hotSeasonDuration = 0;
            double amount = 0;

            if (customerInfo.SewageCalcState == 0)
            {
                return (0,0);
            }
            else if (customerInfo.SewageCalcState == 1)
            {
                hotSeasonDuration = PartTime(hotSeasonStart, hotSeasonEnd, customerInfo.SewageInstallationDateJalali, nerkh.Date2, new { BillId = customerInfo.BillId, ZoneId = customerInfo.ZoneId, UsageId = customerInfo.UsageId });
                amount = hotSeasonDuration > 0 ? (int)((hotSeasonDuration * fazelabAmount / nerkh.Duration) * 0.2) : 0;
                return (hotSeasonDuration, amount);
            }

            hotSeasonDuration = PartTime(hotSeasonStart, hotSeasonEnd, nerkh.Date1, nerkh.Date2, new { BillId = customerInfo.BillId, ZoneId = customerInfo.ZoneId, UsageId = customerInfo.UsageId });
            amount= hotSeasonDuration > 0 ? (int)((hotSeasonDuration * fazelabAmount / nerkh.Duration) * 0.2) : 0;
            return (hotSeasonDuration, amount);
        }

        private bool IsGardenOrDweltyAfter1400_12_24(int usageId, string nerkhDate1)
        {
            string baseDate = "1400/24/12";
            int[] usageIds = [25, 34];
            return usageIds.Contains(usageId) && nerkhDate1.CompareTo(baseDate) >= 0;
        }
        private bool IsLessThan1403_09_13(string nerkhDate2)
        {
            string baseDate = "1403/09/13";
            return nerkhDate2.CompareTo(baseDate) <= 0;
        }
        private bool StringConditionMoreThan(string fromDate, string toDate)
        {
            DateOnly? from = fromDate.ToGregorianDateOnly();
            DateOnly? to = toDate.ToGregorianDateOnly();
            if (!from.HasValue && !to.HasValue)
            {
                throw new BaseException(ExceptionLiterals.InvalidDate);
            }

            if (from.Value >= to.Value)
                return true;

            return false;
        }
        private (long, long) Get2PartAmount(string nerkhDate2)
        {
            string date1400_12_25 = "1400/12/25";
            string date1402_04_23 = "1402/04/23";
            string date1403_06_25 = "1403/06/25";
            string date1403_09_13 = "1403/09/13";
            string date1404_02_31 = "1404/02/31";

            if (StringConditionMoreThan(date1400_12_25, nerkhDate2))
            {
                return (3776, 168110);
            }
            else if (StringConditionMoreThan(nerkhDate2, date1400_12_25) && StringConditionMoreThan(date1402_04_23, nerkhDate2))
            {
                return (4040, 168110);
            }
            else if (StringConditionMoreThan(nerkhDate2, date1402_04_23) && StringConditionMoreThan(date1403_06_25, nerkhDate2))
            {
                return (4040, 168110);
            }
            else if (StringConditionMoreThan(nerkhDate2, date1403_06_25) && StringConditionMoreThan(date1403_09_13, nerkhDate2))
            {
                return (4323, 350000);
            }
            else if (StringConditionMoreThan(nerkhDate2, date1403_09_13) && StringConditionMoreThan(date1404_02_31, nerkhDate2))
            {
                return (7000, 350000);
            }
            else if (StringConditionMoreThan(nerkhDate2, date1404_02_31)) //nerkhDate2 > '1404/02/31'
            {
                return (9000, 450000);
            }
            return (0, 0);
        }

        private (long, long) GetEducationOrBathMultiplier(int usageId, string nerkhDate1, string nerkhDate2, bool isSpecial, long abAzad, double abBahaFromExpression)
        {
            string date1402_04_23 = "1402/04/23";
            string date1403_06_25 = "1403/06/25";
            string date1403_09_13 = "1403/09/13";
            string date1404_02_31 = "1404/02/31";
            (long, long) _8644_8644 = (8644, 8644);
            (long, long) _4323_225000 = (4323, 225000);
            (long, long) _4323_350000 = (4323, 350000);
            (long, long) _7000_350000 = (7000, 350000);
            (long, long) _9000_450000 = (9000, 450000);
            //start line 1228

            if (IsEducationOrBath(usageId))
            {
                if (LessThanEq(nerkhDate2, date1402_04_23))
                {
                    return usageId == 11 ? _8644_8644 : _4323_225000;
                }
                if (IsGtFromLqTo(nerkhDate2, date1402_04_23, date1403_06_25))
                {
                    return usageId == 11 ? _8644_8644 : _4323_225000;
                }
                else if (IsGtFromLqTo(nerkhDate2, date1403_06_25, date1403_09_13))
                {
                    return usageId == 11 ? _8644_8644 : _4323_350000;
                }
                else if (IsGtFromLqTo(nerkhDate2, date1403_09_13, date1404_02_31))
                {
                    return _7000_350000;
                }
                return _9000_450000;
            }
            else
            {
                return ((long)abBahaFromExpression, abAzad);
            }
        }

        private static bool IsBetween(int baseZoneId, int zoneIdParam, string readingNumber, string fromNumber, string toNumber)
        {
            return
                baseZoneId == zoneIdParam &&
                readingNumber.Trim().CompareTo(fromNumber) >= 0 &&
                readingNumber.Trim().CompareTo(toNumber) <= 0;
        }
        private static bool RuralButIsMetro(int zoneId, string readingNumber)
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

        private (double, double) CalculateBoodje(NerkhGetDto nerkhDto, CustomerInfoOutputDto customerInfo, string currentDateJalali, double monthlyConsumption, double olgoo, int consumption, int duration, int finalDomesticCount)
        {
            double consumptionAfter1404 = 0;
            string _1404_01_01 = "1404/01/01";
            string _1403_12_30 = "1403/12/30";
            if (nerkhDto.Date2.CompareTo(_1404_01_01) < 0)
            {
                return (0, 0);
            }

            if (nerkhDto.Date1.CompareTo(_1404_01_01) < 0 && nerkhDto.Date2.CompareTo(_1404_01_01) >= 0)
            {
                CalcDistanceResultDto calcDistance = CalculationDistanceDate.CalcDistance(_1403_12_30, nerkhDto.Date2, true, customerInfo);
                int durationAfter1404 = 0;
                if (calcDistance.HasError)
                {
                    throw new TariffDateException(customerInfo.BillId + " - " + ExceptionLiterals.Incalculable);
                }
                durationAfter1404 = calcDistance.Distance;


                consumptionAfter1404 = ((double)consumption / duration) * (double)durationAfter1404;
            }
            else
            {
                consumptionAfter1404 = nerkhDto.PartialConsumption;
            }
            if (IsConstruction(customerInfo.BranchType))
            {
                return (consumptionAfter1404 * 2000, 0);
            }            
            double partialOlgoo = IsDomesticCategory(customerInfo.UsageId) ?
                (double)finalDomesticCount * olgoo / 30 * nerkhDto.Duration :
                (double)customerInfo.ContractualCapacity / 30 * nerkhDto.Duration;

            double allowedConsumption = consumptionAfter1404 > partialOlgoo ? partialOlgoo : consumptionAfter1404;
            double disAllowedConsumption = consumptionAfter1404 - allowedConsumption;

            return (allowedConsumption * 2000, disAllowedConsumption * 4000);
        }

        /// <summary>
        /// محاسبه مبلغ کارمزد دفع یا مبلغ آبونمان فاضلاب
        /// </summary>
        /// <param name="nerkh"></param>
        /// <param name="customerInfo"></param>
        /// <param name="abBahaItemAmount"></param>
        /// <param name="currentDateJalali"></param>
        /// <returns></returns>
        /// <exception cref="TariffDateException"></exception>
        public double CalculateFazelab(string date1, string date2, int durationAll, CustomerInfoOutputDto customerInfo, double abBahaItemAmount, string currentDateJalali, bool isAbonman)
        {
            double sewageAmount = 0;
            //محاسبه کارمزد دفع در کاربری های گروه خانگی ضریب 0.7
            double multiplier = !isAbonman && IsDomesticCategory(customerInfo.UsageId) ? 0.7 : 1;
            int _withoutSewage = 0, _firstCalculation = 1, _normal = 2;

            if (IsConstruction(customerInfo.BranchType))
            {
                return 0;
            }
            
            if (customerInfo.SewageCalcState == _withoutSewage)
            {
                return 0;
            }
            // ثبت نصب بعد از تاریخ قرائت لحاظ شده
            if(string.Compare(customerInfo.SewageRegisterDate,"1330/0/01")>=0 &&
               string.Compare(customerInfo.SewageRegisterDate,date2)>0)
            {
                return 0;
            }
            else if (
                customerInfo.SewageCalcState == _firstCalculation &&
                string.Compare(date2, customerInfo.SewageInstallationDateJalali) > 0 &&
                customerInfo.SewageInstallationDateJalali.Trim().Length == 10)
            {
                CalcDistanceResultDto calcDistance = CalcDistance(customerInfo.SewageInstallationDateJalali, date2, true, customerInfo);
                int duration = 0;
                if (calcDistance.HasError)
                {
                    throw new TariffDateException(customerInfo.BillId + " - " + ExceptionLiterals.Incalculable);
                }
                duration = calcDistance.Distance;

                sewageAmount = (abBahaItemAmount / durationAll) * duration * multiplier;
                //Update SewageStateToNormal in DB
                return sewageAmount;               
            }
            else if (//تاریخ قرائت قبل از تاریخ نصب
               customerInfo.SewageCalcState == _normal &&
               string.Compare(date2, customerInfo.SewageInstallationDateJalali) < 0)
            {
                return 0;
            }
            else if (//نرمال اما تاریخ نصب قبل از تاریخ قرائت و بعد از ابتدای دوره مصرف، پس بخشی از آن باید حساب شود
                customerInfo.SewageCalcState == _normal &&
                string.Compare(date2, customerInfo.SewageInstallationDateJalali) > 0 &&
                string.Compare(date1, customerInfo.SewageInstallationDateJalali) <= 0 &&
                string.Compare(customerInfo.SewageInstallationDateJalali, "1330/01/01") > 0 &&
                customerInfo.SewageInstallationDateJalali.Trim().Length == 10)
            {
                CalcDistanceResultDto calcDistance = CalcDistance(customerInfo.SewageInstallationDateJalali, date2, true, customerInfo);
                int duration = 0;
                if (calcDistance.HasError)
                {
                    throw new TariffDateException(customerInfo.BillId + " - " + ExceptionLiterals.Incalculable);
                }
                duration = calcDistance.Distance;
                sewageAmount = (abBahaItemAmount / durationAll) * duration * multiplier;
                return sewageAmount;
            }
            else if (customerInfo.SewageCalcState == _normal || currentDateJalali == customerInfo.SewageInstallationDateJalali)
            {
                sewageAmount = abBahaItemAmount * multiplier;
            }           

            return sewageAmount;
        }
        
        public double CalculateAbonmanAb(CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, string currentDateJalali)
        {
            string date1400_01_01 = "1400/01/01";
            string date1403_12_01 = "1403/12/01";
            string date1403_12_30 = "1403/12/30";
            string date1404_02_14 = "1404/02/14";
            string date1404_02_31 = "1404/02/31";
            string date1404_12_29 = "1404/12/29";

            if (!IsConstruction(customerInfo.BranchType) && IsTankerSale(customerInfo.UsageId))
                return 0;

            double abonAbAmount = 0;//, abonAbDiscount = 0;
            double zabon_1 = 0, zabon_2 = 0, zabon_3 = 0, zabon_4 = 0;

            zabon_1 = PartTime(date1400_01_01, date1403_12_01, meterInfo.PreviousDateJalali, currentDateJalali,new { BillId = customerInfo.BillId,ZoneId = customerInfo.ZoneId,UsageId = customerInfo.UsageId});
            zabon_2 = PartTime(date1403_12_01, date1403_12_30, meterInfo.PreviousDateJalali, currentDateJalali, new { BillId = customerInfo.BillId, ZoneId = customerInfo.ZoneId, UsageId = customerInfo.UsageId });

            if (IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId))
                zabon_3 = PartTime(date1403_12_30, date1404_02_14, meterInfo.PreviousDateJalali, currentDateJalali, new { BillId = customerInfo.BillId, ZoneId = customerInfo.ZoneId, UsageId = customerInfo.UsageId });
            else
                zabon_3 = PartTime(date1403_12_30, date1404_02_31, meterInfo.PreviousDateJalali, currentDateJalali, new { BillId = customerInfo.BillId, ZoneId = customerInfo.ZoneId, UsageId = customerInfo.UsageId });

            if (IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId))
                zabon_4 = PartTime(date1404_02_14, date1404_12_29, meterInfo.PreviousDateJalali, currentDateJalali, new { BillId = customerInfo.BillId, ZoneId = customerInfo.ZoneId, UsageId = customerInfo.UsageId });
            else
                zabon_4 = PartTime(date1404_02_31, date1404_12_29, meterInfo.PreviousDateJalali, currentDateJalali, new { BillId = customerInfo.BillId, ZoneId = customerInfo.ZoneId, UsageId = customerInfo.UsageId });

            zabon_1 = Math.Max(zabon_1, 0);
            zabon_2 = Math.Max(zabon_2, 0);
            zabon_3 = Math.Max(zabon_3, 0);
            zabon_4 = Math.Max(zabon_4, 0);

            #region discount
            //if (!IsConstruction(customerInfo.BranchType) &&
            //    ((IsReligiousWithCharity(customerInfo.UsageId)) ||
            //         HasDiscountBranch(customerInfo.BranchType) && IsDomesticWithoutUnspecified(customerInfo.UsageId)))
            //{
            //    double sumZabon = zabon_1 + zabon_2 + zabon_3 + zabon_4;


            //if (customerInfo.DomesticUnit >= 1)
            //{
            //    if (sumZabon > 0)
            //    {
            //        abonAbDiscount = (((10000.0 / 30) * zabon_1) + ((35000.0 / 30) * zabon_2) + ((45500.0 / 30) * zabon_3) + ((58500.0 / 30) * zabon_4) * customerInfo.DomesticUnit);
            //    }
            //}
            //else
            //{
            //    if (sumZabon > 0)
            //    {
            //        abonAbDiscount = (((10000.0 / 30) * zabon_1) + ((35000.0 / 30) * zabon_2) + ((45500.0 / 30) * zabon_3) + ((58500.0 / 30) * zabon_4));
            //    }
            //}

            //if (IsReligiousWithCharity(customerInfo.UsageId) && !IsConstruction(customerInfo.BranchType))
            //{
            //    if (abBahaAmount <= 0 && abBahaDiscount != 0)
            //    {
            //        //nothing
            //    }
            //    else
            //    {
            //        abonAbDiscount = 0;
            //    }
            //}               
            //}
            #endregion

            int sumUnit = customerInfo.OtherUnit + customerInfo.DomesticUnit + customerInfo.CommertialUnit;

            if (IsVillageCollectorMeter(customerInfo.UsageId))
                sumUnit = 1;

            if (sumUnit <= 0)
                sumUnit = 1;

            abonAbAmount = (((10000.0 / monthDays) * zabon_1) + ((35000.0 / monthDays) * zabon_2) + ((45500.0 / monthDays) * zabon_3) + ((58500.0 / monthDays) * zabon_4)) * sumUnit;

            if (abonAbAmount < 0)
                abonAbAmount = 0;

            if (IsConstruction(customerInfo.BranchType) || IsUsageConstructor(customerInfo.UsageId))
                abonAbAmount *= 2;

            return abonAbAmount;
        }       

        public double CalculateAbonmanAbDiscount(int usageId,double abonmanAmount, double bahaDiscountAmount, bool isSpecial)
        {
            if(IsSpecialEducation(usageId, isSpecial))
            {
                return 0;
            }
            return bahaDiscountAmount > 0 && !IsReligiousWithCharity(usageId) ? abonmanAmount : 0;
        }
        public double CalculateMaliatDiscount(double abBahaDiscount, double fazelabDiscount, double abonAbDiscount,
            double abonFazelabDiscount, double boodjeDiscount, double hotSeasonDiscount)
        {
            return 0.1 * (abBahaDiscount + fazelabDiscount + abonAbDiscount + abonFazelabDiscount + boodjeDiscount+ hotSeasonDiscount);
        }

        private double CalculateJavaniJamiat(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, double abBahaAmount, double monthlyConsumption, int olgoo)
        {
            //L 2608
            if (IsUsageConstructor(customerInfo.UsageId))
                return 0;
            if (IsConstruction(customerInfo.BranchType))
                return 0;
            if (abBahaAmount == 0)
                return 0;
            if (customerInfo.ZoneId == 151511)
                return 0;

            int domesticUnit = customerInfo.DomesticUnit;
            double baseAmount = 1000;
            double olgooOrCapacity = IsDomestic(customerInfo.UsageId) ? olgoo : customerInfo.ContractualCapacity;

            if (IsGardenAndResidence(customerInfo.UsageId))
            {
                domesticUnit = customerInfo.DomesticUnit + customerInfo.OtherUnit;
                domesticUnit = domesticUnit == 0 ? 1 : domesticUnit;
            }

            if (IsVillage(customerInfo.ZoneId))
            {
                var (hasVillageCode, villageCode) = HasVillageCode(customerInfo.VillageId);
                if (!hasVillageCode)
                {
                    return 0;
                }

                if (villageCode > 0 && 
                    monthlyConsumption > olgoo &&
                    domesticUnit > 1 && 
                    RuralButIsMetro(customerInfo.ZoneId, villageCode))
                {
                    return baseAmount * nerkh.PartialConsumption;
                }
                else
                {
                    return 0;
                }
            }
            //L 2642
            if (monthlyConsumption > olgoo &&
                domesticUnit >= 1 &&
                (IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId)))
            {
                return baseAmount * nerkh.PartialConsumption;
            }
            if (!IsDomesticWithoutUnspecified(customerInfo.UsageId) && !IsGardenAndResidence(customerInfo.UsageId))
            {
                if (monthlyConsumption > customerInfo.ContractualCapacity)
                {
                    return baseAmount * nerkh.PartialConsumption;
                }
            }
            return 0;
        }
        private (bool, int) HasVillageCode(string villageId)
        {
            if (string.IsNullOrWhiteSpace(villageId) || villageId.Length < 5)
            {
                return (false, 0);
            }
            bool canParse = int.TryParse(villageId.Substring(0, 4), out int villageCode);
            return (canParse, villageCode);
        }
        private double CalculateBoodjeDiscount(CustomerInfoOutputDto customerInfo, double abBahaDiscount, (double, double) boodjeAmounts, NerkhGetDto nerkh)
        {
            if (abBahaDiscount <= 0)
            {
                return 0;
            }
            if (boodjeAmounts.Item1 == 0)
            {
                return 0;
            }
            if (IsHandoverDiscount(customerInfo.BranchType) &&
                IsDomesticCategory(customerInfo.UsageId))
            {
                return boodjeAmounts.Item1;
            }
            //اگه مدرسه با شرایط تعریف شده باشه هم بالای ظرفیت هم زیر ظرفیت تخفیف داده میشه ??
            int virtualCapacity = GetVirtualCapacity(customerInfo, nerkh.Duration);
            if (virtualCapacity > 0)
            {
                double partialOlgoo = (double)customerInfo.ContractualCapacity / monthDays * nerkh.Duration;
                return partialOlgoo <= virtualCapacity ? (boodjeAmounts.Item1+boodjeAmounts.Item2) : 0;
            }
            return boodjeAmounts.Item1;
        }

        private double CalculateAvarez(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, double monthlyConsumption)
        {
            if (IsMoreThan1404_01_01(nerkh.Date2) && IsIndustrial(customerInfo.UsageId) && IsSpecialIndustrial(customerInfo.BranchType))
            {
                return monthlyConsumption <= 25000 ? nerkh.PartialConsumption * 2000 : nerkh.PartialConsumption * 20000;
            }
            return 0;
        }
        private double CalculateFazelabDiscount(double abBahaDiscount, double fazelabAmount, CustomerInfoOutputDto customerInfo, NerkhGetDto nerkh)
        {
            if (abBahaDiscount <= 0)
            {
                return 0;
            }
            if (fazelabAmount <= 0)
            {
                return 0;
            }
            double fazelabDiscount = abBahaDiscount * 0.7;
            int virtualCapacity = GetVirtualCapacity(customerInfo, nerkh.Duration);
            if (virtualCapacity > 0)
            {
                double partialOlgoo = (double)customerInfo.ContractualCapacity / monthDays * nerkh.Duration;
                return partialOlgoo <= virtualCapacity ? fazelabDiscount : 0;
            }
            return fazelabAmount;
        }

        private double CalculateFasleGarmDiscount(NerkhGetDto nerkh, double amountDiscount, (int,double)hotSeasonInfo, CustomerInfoOutputDto customerInfo)
        {//hotSeasonInfo.Item1: duration  , hotSeasonInfo.Item2:amount
            if (amountDiscount == 0)
            {
                return 0;
            }
            if(hotSeasonInfo.Item1==0 || hotSeasonInfo.Item2==0)
            {
                return 0;
            }
            int virtualCapacity = GetVirtualCapacity(customerInfo, nerkh.Duration);
            double timePercentage = (double)hotSeasonInfo.Item1 / (double)nerkh.Duration;
            double fasleGarmAmount = amountDiscount * timePercentage * 0.2;
            if (virtualCapacity > 0)
            {
                double partialOlgoo = (double)customerInfo.ContractualCapacity / monthDays * nerkh.Duration;
                return partialOlgoo <= virtualCapacity ? fasleGarmAmount : 0;
            }          
            return fasleGarmAmount;
        }

        private long CalculateItemDiscount(CustomerInfoOutputDto customerInfo, NerkhGetDto nerkh, int olgoo, long amount, bool isFull, decimal multiplier, int finalDomesticUnit)
        {
            if (amount == 0)
            {
                return 0;
            }            
            double partialOlgoo = IsDomestic(customerInfo.UsageId) ?
               (double)finalDomesticUnit * olgoo / monthDays * nerkh.Duration :
               (double)customerInfo.ContractualCapacity / monthDays * nerkh.Duration;

            if (IsHandoverDiscount(customerInfo.BranchType) &&
                IsDomesticWithoutUnspecified(customerInfo.UsageId))
            {
                if (isFull)
                {
                    return amount;
                }
                if(nerkh.PartialConsumption <= olgoo)// در صورتی که مصرف زیر الگو بود کامل معاف میشود
                {
                    return amount;
                }
                else//در صورتی که بالای الگو بود بخش زیر الگو معاف و بالای الگو اخذ شود
                {
                    //long partialAmount = (long)(partialOlgoo / nerkh.PartialConsumption * amount);
                    //return partialAmount;
                    return (long)(90000 * 0.01 * partialOlgoo * olgoo * (double)multiplier);
                }
            }
            if (IsReligiousWithCharity(customerInfo.UsageId))
            {
                //در صورتی که بالای الگو بود بخش زیر الگو معاف و بالای الگو اخذ شود
                //C*0.1
                return (long)(90000 * 0.1 * (partialOlgoo) * (double)multiplier);
            }
            int virtualCapacity = GetVirtualCapacity(customerInfo, nerkh.Duration);
            if (virtualCapacity>0)
            {
                return partialOlgoo <= virtualCapacity ? amount : 0;
            }
            return 0;
        }

        public double CalcMaliat(double abBahaAmount, double abonmanAbBahaAmount, double hotseasonAbBahaAmount, double fazelabAmount, double abonmanFazelabAmount, double hotseasonFazelabAmount, double boodjeAmount)
        {
            double sumAmount = abBahaAmount + abonmanAbBahaAmount + hotseasonAbBahaAmount + fazelabAmount + abonmanFazelabAmount + hotseasonFazelabAmount + boodjeAmount;
            return sumAmount * 0.10;
        }

        private int GetVirtualCapacity(CustomerInfoOutputDto customerInfo, int duration)
        {
            if(IsSpecialEducation(customerInfo.UsageId, customerInfo.IsSpecial))
            {
                double partialCapacity = (double)customerInfo.ContractualCapacity / monthDays * duration;
                
                return Convert.ToInt32(Math.Round(partialCapacity * 2,1));
            }
            return 0;
        }
    }
}
