using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using DNTPersianUtils.Core;
using org.matheval;

namespace Aban360.CalculationPool.Application.Features.Base
{
    internal abstract class BaseOldTariffEngine : BaseExpressionCalculator
    {
        /// <summary>
        /// تنها تابع با دسترسی پابلیک بابت محاسبه تک رکورد جدول نرخ
        /// </summary>
        /// <returns>مقدار خروجی بعد از اتمام نوشتن کد، اصلاح شود</returns>

        public BaseOldTariffEngineOutputDto CalculateWaterBill(NerkhGetDto nerkh, AbAzadGetDto abAzad, ZaribGetDto zarib, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, double dailyAverage, string currentDateJalali, int consumption, int duration)
        {
            DateOnly previousDate = ConvertJalaliToGregorian(meterInfo.PreviousDateJalali);
            DateOnly currentDate = ConvertJalaliToGregorian(currentDateJalali);
            nerkh.DailyAverageConsumption = dailyAverage;
            (nerkh, nerkh.Duration, nerkh.PartialConsumption) = CalcPartial(nerkh, previousDate, currentDate, dailyAverage, consumption,duration);

            int olgoo = GetOlgoo(nerkh.Date2, nerkh.Olgo);
            bool isVillageCalculation = false;
            double monthlyConsumption = nerkh.DailyAverageConsumption * 30;
            decimal multiplierAbBaha = Multiplier(zarib, olgoo, IsDomestic(customerInfo.UsageId), isVillageCalculation, monthlyConsumption);

            CalculateAbBahaOutputDto abBahaResult = _CalculateAbBaha(nerkh, customerInfo, meterInfo, zarib, abAzad, currentDateJalali, isVillageCalculation, monthlyConsumption, olgoo, multiplierAbBaha);
            (double, double) boodje = CalculateBoodje(nerkh, customerInfo, currentDateJalali, monthlyConsumption, olgoo, consumption, duration);
            double fazelab = CalculateFazelab(nerkh, customerInfo, abBahaResult.AbBahaAmount, currentDateJalali);
            double hotSeasonAbBaha = CalcHotSeasonAbBaha(nerkh, abBahaResult.AbBahaAmount, customerInfo, dailyAverage);
            double hotSeasonFazelb = CalcHotSeasonFaselab(nerkh, customerInfo, fazelab, dailyAverage);
            double avarez = CalculateAvarez(nerkh, customerInfo, monthlyConsumption);
            double javani = CalculateJavaniJamiat(nerkh, customerInfo, abBahaResult.AbBahaAmount, monthlyConsumption, olgoo);

            double abBahaDiscount = CalculateAbBahaDiscount(nerkh, customerInfo, meterInfo, abBahaResult, olgoo, multiplierAbBaha, monthlyConsumption, currentDateJalali);
            double hotSeasonDiscount = CalcHotSeasonDiscount(nerkh, customerInfo, meterInfo, abBahaResult, multiplierAbBaha, hotSeasonAbBaha);
            double fazelabDiscount = CalculateFazelabDiscount(nerkh, customerInfo, abBahaResult, abBahaDiscount, fazelab, 123, olgoo, monthlyConsumption);

            return new BaseOldTariffEngineOutputDto(abBahaResult, fazelab, hotSeasonAbBaha, hotSeasonFazelb, boodje.Item1, boodje.Item2, abBahaDiscount, hotSeasonDiscount, fazelabDiscount, 0, avarez, javani);
        }

        /// <summary>
        /// محاسبه فرمول
        /// </summary>
        /// <param name="formula">متن فرمول که در آن X متوسط مصرف ماهانه است</param>
        /// <param name="monthlyAverageConsumption">متوسط مصرف ماهانه</param>
        /// <returns></returns>
        private long CalcFormulaByRate(string formula, double monthlyAverageConsumption)
        {
            object parameters = new { X = monthlyAverageConsumption };
            Expression expression = GetExpression(formula, parameters);
            long value = expression.Eval<long>();
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
            return CheckConditions(usageId, [1, 3, 25, 34]);
        }
        private bool IsDomesticWithoutUnspecified(int usageId)
        {
            return CheckConditions(usageId, [1, 3]);
        }
        private bool IsNotReligious(int usageId)
        {
            return CheckConditions(usageId, [10, 12, 13, 32, 29]);
        }
        private bool IsReligious(int usageId)
        {
            return CheckConditions(usageId, [10, 12, 13, 32, 29]);
        }
        private bool IsIndustrial(int usageId)
        {
            return CheckConditions(usageId, [4]);
        }
        private bool IsCharityAndSchool(int usageId)
        {
            return CheckConditions(usageId, [8, 7, 12, 13, 29, 30, 32]);
        }
        private bool IsHandoverDiscount(int usageId)
        {
            return CheckConditions(usageId, [3, 6, 7]);
        }
        private bool IsSchool(int usageId)
        {
            return CheckConditions(usageId, [8, 7]);
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
        private bool IsBetween(string number, string start, string end)
        {
            return number.CompareTo(start) >= 0 && number.CompareTo(end) <= 0;
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

        private bool IsDolatabadOrHabibabadWithConditionEshtrak(int zoneId, ulong readingNumber)
        {
            return
                (zoneId == 134013 && IsBetween(readingNumber, 57000000, 57999999)) ||
                (zoneId == 134016 && IsBetween(readingNumber, 57000000, 57999999)) ||
                 MetroButIsRural(zoneId, readingNumber, 4);
        }
        private bool MetroButIsRural(int zoneId, ulong readingNumber, int thresholdSkip)
        {
            int shortReadingNumber = (int.Parse)(readingNumber.ToString().Substring(0, thresholdSkip));
            if (zoneId == 132220 &&
                (IsBetween(shortReadingNumber, 1610, 1628) ||
                IsBetween(shortReadingNumber, 1633, 1648) ||
                IsBetween(shortReadingNumber, 1651, 1661) ||
                IsBetween(shortReadingNumber, 6042, 6052) ||
                IsBetween(shortReadingNumber, 6060, 6072))
                )
                return true;

            if (zoneId == 132211 &&
                 (IsBetween(shortReadingNumber, 1103, 1108) ||
                 IsBetween(shortReadingNumber, 1109, 1113) ||
                 IsBetween(shortReadingNumber, 1143, 1165) ||
                 IsBetween(shortReadingNumber, 1161, 1184) ||
                 IsBetween(shortReadingNumber, 1403, 1499) ||
                 IsBetween(shortReadingNumber, 1450, 1472) ||
                 IsBetween(shortReadingNumber, 1574, 1599))
               )
                return true;

            return false;
        }
        private decimal Multiplier(ZaribGetDto zarib, int olgo, bool isDomestic, bool isVillage, double monthlyConsumption)
        {
            double zbSelection = 1;

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
            if (duration <= 0 ||
              monthlyConsumption == 0 ||
              string.IsNullOrWhiteSpace(vaj))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// محاسبه آب بها 
        /// </summary>
        /// <returns>عدد محاسبه شده‌ی آب‌بها</returns>
        private CalculateAbBahaOutputDto _CalculateAbBaha(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, ZaribGetDto zarib, AbAzadGetDto abAzad8And39, string currentDateJalali, bool isVillageCalculation, double monthlyConsumption, int _olgoo, decimal multiplierAbBaha)
        {
            var abAzadTest = abAzad8And39;

            double abBahaAmount = 0, oldAbBahaAmount = 0, abBahaFromExpression = 0;
            double duration = nerkh.Duration;
            abBahaFromExpression = CalcFormulaByRate(nerkh.Vaj, monthlyConsumption);
            (double, double) abBahaValues = (0, 0);

            if (CheckZero(duration, monthlyConsumption, nerkh.Vaj))
                return new CalculateAbBahaOutputDto(0, (0, 0));

            if ((IsDomestic(customerInfo.UsageId) || IsGardenOrDweltyAfter1400_12_24(customerInfo.UsageId, nerkh.Date1)) &&
                IsNotReligious(customerInfo.UsageId))
            {
                abBahaFromExpression = CalcFormulaByRate(nerkh.Vaj, monthlyConsumption);
                abBahaAmount = abBahaFromExpression * nerkh.PartialConsumption;

                if (!string.IsNullOrWhiteSpace(nerkh.OVaj) &&
                    nerkh.OVaj.Trim() != "0" &&
                    IsLessThan1403_09_13(nerkh.Date2))
                {
                    long oldAbBahaFromExpression = CalcFormulaByRate(nerkh.OVaj, monthlyConsumption);
                    oldAbBahaAmount = nerkh.PartialConsumption * oldAbBahaFromExpression * 1.15;
                }

                if (IsLessThan1403_09_13(nerkh.Date2) &&
                    monthlyConsumption <= _olgoo &&
                    abBahaAmount > oldAbBahaAmount &&
                    IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
                    !IsConstruction(customerInfo.BranchType))
                {
                    abBahaAmount = oldAbBahaAmount;
                }
            }
            else
            {
                // foxpro:1139
                if ((customerInfo.ContractualCapacity > 0 && !IsConstruction(customerInfo.BranchType)) ||
                    IsReligious(customerInfo.UsageId))
                {
                    double contractualCapacityInDuration = ((double)customerInfo.ContractualCapacity / 30) * duration;

                    if (nerkh.PartialConsumption > contractualCapacityInDuration ||
                        IsCharityAndSchool(customerInfo.UsageId))
                    {
                        double disallowedPartialConsumption = nerkh.PartialConsumption - contractualCapacityInDuration;
                        double allowedPartialConsumption = contractualCapacityInDuration;

                        if (nerkh.PartialConsumption < contractualCapacityInDuration)
                        {
                            allowedPartialConsumption = nerkh.PartialConsumption;
                            disallowedPartialConsumption = 0;
                        }//L 1153
                        if (customerInfo.ContractualCapacity == 0 &&
                            IsReligiousWithCharity(customerInfo.UsageId))
                        {
                            disallowedPartialConsumption = 0;
                            allowedPartialConsumption = nerkh.PartialConsumption;
                        }

                        (long, long) _2Amount = (0, 0);
                        //(double, double) abBahaValues = (0, 0);
                        if (IsReligiousWithCharity(customerInfo.UsageId))
                        {
                            if (!IsConstruction(customerInfo.BranchType))//  foxpro:1178
                            {
                                _2Amount = Get2PartAmount(nerkh.Date2);
                            }
                            else
                            {
                                _2Amount = (450000, 450000);
                            }
                        }
                        else
                        {
                            _2Amount = BigCase(customerInfo.UsageId, nerkh.Date1, nerkh.Date2, customerInfo.IsSpecial, abAzad8And39.Azad, abBahaFromExpression);//Azad:39
                        }
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
                    abBahaAmount = nerkh.PartialConsumption * abBahaFromExpression;
                }
            }
            //L 1553
            isVillageCalculation = false;//L 1558

            if (IsVillage(customerInfo.ZoneId) &&
                IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
                !IsConstruction(customerInfo.BranchType))
            {
                int villageCode = int.Parse(customerInfo.VillageId.Trim().Substring(0, 4));
                if (RuralButIsMetro(customerInfo.ZoneId, customerInfo.ReadingNumber) ||
                    RuralButIsMetro(customerInfo.ZoneId, villageCode))
                {
                    //nothing !
                }
                else
                {//1578
                    if (abBahaAmount != 0)
                    {
                        if (IsLessThan1403_09_13(nerkh.Date2) || monthlyConsumption <= _olgoo)
                        {
                            (abBahaAmount, oldAbBahaAmount, isVillageCalculation) = MultiplyCalculation(abBahaAmount, oldAbBahaAmount, 0.5);
                        }
                        else
                        {
                            (abBahaAmount, oldAbBahaAmount, isVillageCalculation) = MultiplyCalculation(abBahaAmount, oldAbBahaAmount, 0.65);
                        }
                    }
                }
            }
            //L 1600 approximately
            if (IsDolatabadOrHabibabadWithConditionEshtrak(customerInfo.ZoneId, ulong.Parse(customerInfo.ReadingNumber)))
            {
                if (IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
                    !IsConstruction(customerInfo.BranchType))
                {
                    if (abBahaAmount != 0)//L 1604
                    {
                        if (IsLessThan1403_09_13(nerkh.Date2) || monthlyConsumption <= _olgoo)
                        {
                            (abBahaAmount, oldAbBahaAmount, isVillageCalculation) = MultiplyCalculation(abBahaAmount, oldAbBahaAmount, 0.5);
                        }
                        else
                        {
                            (abBahaAmount, oldAbBahaAmount, isVillageCalculation) = MultiplyCalculation(abBahaAmount, oldAbBahaAmount, 0.65);
                        }
                    }
                }
            }//foxpro:1620

            abBahaAmount = abBahaAmount * (double)multiplierAbBaha;
            oldAbBahaAmount = oldAbBahaAmount * (double)multiplierAbBaha;// foxpro:1755
            return new CalculateAbBahaOutputDto(abBahaAmount, abBahaValues);

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
            double partialConsumption = (double)consumption/(double)duration*durationPartial;
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
        private int PartTime(string date1, string date2, string previousDate, string currentDate)
        {
            int partMethod = 0;
            partMethod = IsBetween(previousDate, date1, date2) && IsBetween(currentDate, date1, date2) ? 
                (int.Parse)(CalculationDistanceDate.CalcDistance(previousDate, currentDate)) : partMethod;

            partMethod = previousDate.CompareTo(date1) <= 0 && IsBetween(currentDate, date1, date2) ? 
                (int.Parse)(CalculationDistanceDate.CalcDistance(date1, currentDate)) : partMethod;

            partMethod = currentDate.CompareTo(date2) >= 0 && IsBetween(previousDate, date1, date2) ? 
                (int.Parse)(CalculationDistanceDate.CalcDistance(previousDate, date2)) : partMethod;

            partMethod = previousDate.CompareTo(date1) <= 0 && currentDate.CompareTo(date2) >= 0 ? 
                (int.Parse)(CalculationDistanceDate.CalcDistance(date1, date2)) : partMethod;

            return partMethod;
        }
        private double CalcHotSeasonAbBaha(NerkhGetDto nerkh, double abBahaAmount, CustomerInfoOutputDto customerInfo, double dailyAverage)
        {
            if (IsDomestic(customerInfo.UsageId) && dailyAverage<25)
            {
                return 0;
            }
            string date_02_31 = "/02/31";
            string date_06_31 = "/06/31";
            string hotSeasonStart = nerkh.Date2.Substring(0, 4) + date_02_31;
            string hotSeasonEnd = nerkh.Date2.Substring(0, 4) + date_06_31;

            int hotSeasonDuration = PartTime(hotSeasonStart, hotSeasonEnd, nerkh.Date1, nerkh.Date2);
            return hotSeasonDuration > 0 && PartTime(hotSeasonStart, hotSeasonEnd, nerkh.Date1, nerkh.Date2) > 0 ? (int)((hotSeasonDuration * abBahaAmount / nerkh.Duration) * 0.2) : 0;

        }

        private double CalcHotSeasonFaselab(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, double fazelabAmount, double dailyAverage)
        {
            if (IsDomestic(customerInfo.UsageId) && dailyAverage < 25)
            {
                return 0;
            }
            string date_02_31 = "/02/31";
            string date_06_31 = "/06/31";
            string hotSeasonStart = nerkh.Date2.Substring(0, 4) + date_02_31;
            string hotSeasonEnd = nerkh.Date2.Substring(0, 4) + date_06_31;
            int hotSeasonDuration = 0;

            if (customerInfo.SewageCalcState == 0)
                return 0;
            else if (customerInfo.SewageCalcState == 1)
            {
                hotSeasonDuration = PartTime(hotSeasonStart, hotSeasonEnd, customerInfo.SewageInstallationDateJalali, nerkh.Date2);
                return hotSeasonDuration > 0 && PartTime(hotSeasonStart, hotSeasonEnd, nerkh.Date1, nerkh.Date2) > 0 ? (int)((hotSeasonDuration * fazelabAmount / nerkh.Duration) * 0.2) : 0;
            }

            hotSeasonDuration = PartTime(hotSeasonStart, hotSeasonEnd, nerkh.Date1, nerkh.Date2);
            return hotSeasonDuration > 0 && PartTime(hotSeasonStart, hotSeasonEnd, nerkh.Date1, nerkh.Date2) > 0 ? (int)((hotSeasonDuration * fazelabAmount / nerkh.Duration) * 0.2) : 0;

        }

        private double CalcHotSeasonDiscount(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, CalculateAbBahaOutputDto abBahaValues, decimal MultiplierAbBaha, double hotSeasonAmount)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            double abBahaMultiplied = 0;

            //1942
            if ((IsReligiousWithCharity(customerInfo.UsageId)) && !IsConstruction(customerInfo.BranchType))
            {
                abBahaMultiplied = (abBahaValues.AbBahaValues.Item1 * (double)MultiplierAbBaha);
                abBahaValues.AbBahaAmount = abBahaValues.AbBahaValues.Item1 - abBahaMultiplied;
                if (hotSeasonAmount != 0)
                {
                }
            }//line->1975
            return 0;
        }
        private double CalculateAbBahaDiscount(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, CalculateAbBahaOutputDto abBahaValues, int olgoo, decimal multiplierAbBaha, double monthlyConsumption, string currentDateJalali)
        {
            double fazelabAmount = abBahaValues.AbBahaAmount;
            bool isVillage = IsVillage(customerInfo.ZoneId);
            double partialOlgoo = olgoo / 30 * nerkh.Duration;
            string date1401_12_27 = "1401/12/27";
            int divider = isVillage ? 2 : 1;
            double abBahaDiscount = 0;

            double consumptionDiscount = nerkh.PartialConsumption > partialOlgoo ? partialOlgoo : nerkh.PartialConsumption;
            if (MeetCondition(nerkh, customerInfo, date1401_12_27))
            {
                return IsLessThan1403_09_13(nerkh.Date2) ?
                    (int)(consumptionDiscount * ((((3706 * partialOlgoo) - 13845) / partialOlgoo) * 1.15) * (double)multiplierAbBaha) / divider :
                    (int)(consumptionDiscount * ((((70000 * 0.01 * partialOlgoo)) * partialOlgoo) / partialOlgoo) * (double)multiplierAbBaha) / divider;

            }//L 1883

            return abBahaDiscount;


            bool MeetCondition(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, string date1401_12_27)
            {
                return IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
                                !IsConstruction(customerInfo.BranchType) &&
                                IsHandoverDiscount(customerInfo.BranchType) &&
                                nerkh.Date1.CompareTo(date1401_12_27) >= 0;
            }
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

        private (long, long) BigCase(int usageId, string nerkhDate1, string nerkhDate2, bool isSpecial, long abAzad, double abBahaFromExpression)
        {
            string date1399_01_31 = "1399/01/31";
            string date1400_01_31 = "1400/01/31";
            string date1400_12_24 = "1400/12/24";
            string date1401_12_27 = "1401/12/27";
            string date1402_04_23 = "1402/04/23";
            string date1403_06_25 = "1403/06/25";
            string date1403_09_13 = "1403/09/13";
            string date1404_02_31 = "1404/02/31";

            //start line 1228
            //1                                                  
            if ((IsEducation(usageId) &&
            nerkhDate2.CompareTo(date1399_01_31) > 0 &&//TMP_NERKH.Date2 > '1399/01/31'
            nerkhDate2.CompareTo(date1400_01_31) <= 0))//TMP_NERKH.Date2 <= '1400/01/31'
            {
                if (usageId == 9 && isSpecial)
                {
                    return (10953, 45000);
                }
                else if (usageId == 9 && !isSpecial)
                {
                    return (9525, 45000);
                }
                else if (usageId == 41 && isSpecial)
                {
                    return (10953, 45000);
                }
                else if (usageId == 41 && !isSpecial)
                {
                    return (3529, 45000);
                }
                else if (usageId == 8 && isSpecial)
                {
                    return (10953, 45000);
                }
                else if (usageId == 8 && !isSpecial)
                {
                    return (3529, 45000);
                }
                else if (usageId == 7 && isSpecial)
                {
                    return (10953, 45000);
                }
                else if (usageId == 7 && !isSpecial)
                {
                    return (3529, 45000);
                }
            }
            //2
            else if (IsEducation(usageId) &&
                     nerkhDate2.CompareTo(date1400_01_31) > 0 &&//TMP_NERKH.Date2 > '1400/01/31'
                     nerkhDate2.CompareTo(date1400_12_24) <= 0)//TMP_NERKH.Date2 <= '1400/12/24'
            {
                if (usageId == 9 && isSpecial)
                {
                    return (11720, 133255);
                }
                else if (usageId == 9 && !isSpecial)
                {
                    return (11720, 133255);
                }
                else if (usageId == 41 && isSpecial)
                {
                    return (3776, 133255);
                }
                else if (usageId == 41 && !isSpecial)
                {
                    return (3776, 133255);
                }
                else if (usageId == 8 && isSpecial)
                {
                    return (11720, 133255);
                }
                else if (usageId == 8 && !isSpecial)
                {
                    return (3776, 133255);
                }
                else if (usageId == 7 && isSpecial)
                {
                    return (3776, 133255);
                }
                else if (usageId == 7 && !isSpecial)
                {
                    return (3776, 133255);
                }
            }
            // 3
            else if (IsEducation(usageId) &&
                     nerkhDate2.CompareTo(date1400_12_24) > 0 &&
                     nerkhDate2.CompareTo(date1401_12_27) <= 0)
            {
                if (usageId == 9 && isSpecial)
                {
                    return (33622, 168110);
                }
                else if (usageId == 9 && !isSpecial)
                {
                    return (33622, 168110);
                }
                else if (usageId == 41 && isSpecial)
                {
                    return (3776, 168110);
                }
                else if (usageId == 41 && !isSpecial)
                {
                    return (3776, 168110);
                }
                else if (usageId == 8 && isSpecial)
                {
                    return (3776, 168110);
                }
                else if (usageId == 8 && !isSpecial)
                {
                    return (3776, 168110);
                }
                else if (usageId == 7 && isSpecial)
                {
                    return (3776, 168110);
                }
                else if (usageId == 7 && !isSpecial)
                {
                    return (3776, 168110);
                }
            }

            // 4
            else if (IsEducationOrBath(usageId) &&
                     nerkhDate2.CompareTo(date1401_12_27) > 0 &&
                     nerkhDate2.CompareTo(date1402_04_23) <= 0)
            {
                if (usageId == 9 && isSpecial)
                {
                    return (33622, 168110);
                }
                else if (usageId == 9 && !isSpecial)
                {
                    return (33622, 168110);
                }
                else if (usageId == 41 && isSpecial)
                {
                    return (4040, 168110);
                }
                else if (usageId == 41 && !isSpecial)
                {
                    return (3776, 168110);
                }
                else if (usageId == 8 && isSpecial)
                {
                    return (4040, 168110);
                }
                else if (usageId == 8 && !isSpecial)
                {
                    return (4040, 168110);
                }
                else if (usageId == 7 && isSpecial)
                {
                    return (4040, 168110);
                }
                else if (usageId == 7 && !isSpecial)
                {
                    return (4040, 168110);
                }
                else if (usageId == 11)
                {
                    return (8644, 8644);
                }
            }

            // 5
            else if (IsEducationOrBath(usageId) &&
                     nerkhDate2.CompareTo(date1402_04_23) > 0 &&
                     nerkhDate2.CompareTo(date1403_06_25) <= 0)
            {
                if (usageId == 9 && isSpecial)
                {
                    return (4323, 225000);
                }
                else if (usageId == 9 && !isSpecial)
                {
                    return (45000, 225000);
                }
                else if (usageId == 41 && isSpecial)
                {
                    return (4323, 225000);
                }
                else if (usageId == 41 && !isSpecial)
                {
                    return (4323, 225000);
                }
                else if (usageId == 8 && isSpecial)
                {
                    return (4323, 225000);
                }
                else if (usageId == 8 && !isSpecial)
                {
                    return (4323, 225000);
                }
                else if (usageId == 7 && isSpecial)
                {
                    return (4323, 225000);
                }
                else if (usageId == 7 && !isSpecial)
                {
                    return (4323, 225000);
                }
                else if (usageId == 11)
                {
                    return (8644, 8644);
                }
            }

            // 6
            else if (IsEducationOrBath(usageId) &&
                     nerkhDate2.CompareTo(date1403_06_25) > 0 &&
                     nerkhDate2.CompareTo(date1403_09_13) <= 0)
            {
                if (usageId == 9 && isSpecial)
                {
                    return (4323, 350000);
                }
                else if (usageId == 9 && !isSpecial)
                {
                    return (45000, 350000);
                }
                else if (usageId == 41 && isSpecial)
                {
                    return (4323, 350000);
                }
                else if (usageId == 41 && !isSpecial)
                {
                    return (4323, 350000);
                }
                else if (usageId == 8 && isSpecial)
                {
                    return (4323, 350000);
                }
                else if (usageId == 8 && !isSpecial)
                {
                    return (4323, 350000);
                }
                else if (usageId == 7 && isSpecial)
                {
                    return (4323, 350000);
                }
                else if (usageId == 7 && !isSpecial)
                {
                    return (4323, 350000);
                }
                else if (usageId == 11)
                {
                    return (8644, 8644);
                }
            }

            // 7
            else if (IsEducationOrBath(usageId) &&
                     nerkhDate2.CompareTo(date1403_09_13) > 0 &&
                     nerkhDate2.CompareTo(date1404_02_31) <= 0)
            {
                if (usageId == 9 && isSpecial)
                {
                    return (7000, 350000);
                }
                else if (usageId == 9 && !isSpecial)
                {
                    return (45000, 350000);
                }
                else if (usageId == 41 && isSpecial)
                {
                    return (7000, 350000);
                }
                else if (usageId == 41 && !isSpecial)
                {
                    return (7000, 350000);
                }
                else if (usageId == 8 && isSpecial)
                {
                    return (7000, 350000);
                }
                else if (usageId == 8 && !isSpecial)
                {
                    return (7000, 350000);
                }
                else if (usageId == 7 && isSpecial)
                {
                    return (7000, 350000);
                }
                else if (usageId == 11)
                {
                    return (7000, 350000);
                }
            }

            // 8
            else if (IsEducationOrBath(usageId) &&
                     nerkhDate2.CompareTo(date1404_02_31) > 0)
            {
                if (usageId == 9 && isSpecial)
                {
                    return (9000, 450000);
                }
                else if (usageId == 9 && !isSpecial)
                {
                    return (45000, 450000);
                }
                else if (usageId == 41 && isSpecial)
                {
                    return (9000, 450000);
                }
                else if (usageId == 41 && !isSpecial)
                {
                    return (9000, 450000);
                }
                else if (usageId == 8 && isSpecial)
                {
                    return (9000, 450000);
                }
                else if (usageId == 8 && !isSpecial)
                {
                    return (9000, 450000);
                }
                else if (usageId == 7 && isSpecial)
                {
                    return (9000, 450000);
                }
                else if (usageId == 11)
                {
                    return (9000, 450000);
                }
            }

            else
            {
                //long nerkh_azad = CalculateAzad(nerkhDate1, nerkhDate2, 39);//&& ab azad sakht va saz  && ab azad omomi kargahi** dar  tarikh 1398 / 01 / 31
                return ((long)abBahaFromExpression, abAzad);
            }
            return (0, 0);
            //end line 1532
        }

        private static int CalculateAzad(string date1, string date2, int kar)
        {
            return 150000;
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
     
        private (double, double) CalculateBoodje(NerkhGetDto nerkhDto, CustomerInfoOutputDto customerInfo, string currentDateJalali, double monthlyConsumption, double olgoo, int consumption, int duration)
        {
            double consumptionAfter1404 = 0;
            string _1404_01_01= "1404/01/01";
            if (nerkhDto.Date2.CompareTo(_1404_01_01) < 0)
            {
                return (0, 0);
            }

            if (nerkhDto.Date1.CompareTo(_1404_01_01) < 0 && nerkhDto.Date2.CompareTo(_1404_01_01) >=0)
            {
                int durationAfter1404 = int.Parse(CalculationDistanceDate.CalcDistance(_1404_01_01, nerkhDto.Date2));
                consumptionAfter1404 = ((double)consumption / duration) * (double)durationAfter1404;
            }
            else
            {
                consumptionAfter1404 = nerkhDto.PartialConsumption;
            }
            int domesticCount = (customerInfo.DomesticUnit-customerInfo.EmptyUnit) <= 0 ? 1 : customerInfo.DomesticUnit-customerInfo.EmptyUnit;
            double partialOlgoo = IsDomesticWithoutUnspecified(customerInfo.UsageId) ?
                (double)domesticCount * olgoo / 30 * nerkhDto.Duration :
                (double)customerInfo.ContractualCapacity / 30 * nerkhDto.Duration;

            double allowedConsumption = consumptionAfter1404 > partialOlgoo ? partialOlgoo : consumptionAfter1404;
            double disAllowedConsumption = consumptionAfter1404 - allowedConsumption;

            return (allowedConsumption * 2000, disAllowedConsumption * 4000);
        }
    
        private double CalculateFazelab(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, double abBahaAmount, string currentDateJalali)
        {
            double sewageAmount = 0;
            double multiplier = IsDomesticWithoutUnspecified(customerInfo.UsageId) ? 0.7 : 1;
            int _withoutSewage = 0, _firstCalculation = 1, _normal = 2;

            //has foreach
            if (customerInfo.SewageCalcState == _withoutSewage)
            {
                return 0;
            }
            else if (customerInfo.SewageCalcState == _firstCalculation && string.Compare(currentDateJalali, customerInfo.SewageInstallationDateJalali) > 0)
            {
                // int mod_as_nasb = PartTime(nerkh.Date1, nerkh.Date2, customerInfo.SewageInstallationDateJalali, currentDateJalali);
                // sewageAmount = (sewageAmount / nerkh.Duration) * mod_as_nasb;
                int duration = int.Parse(CalculationDistanceDate.CalcDistance(customerInfo.SewageInstallationDateJalali, currentDateJalali));
                sewageAmount = (abBahaAmount / nerkh.Duration) * duration * multiplier;

                //Update SewageStateToNormal in DB
            }
            else if (customerInfo.SewageCalcState == _normal || currentDateJalali == customerInfo.SewageInstallationDateJalali)
            {
                sewageAmount = abBahaAmount * multiplier;
            }

            return sewageAmount;
        }
        private double CalculateFazelabDiscount(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, CalculateAbBahaOutputDto abBahaValues, double abBahaDiscount, double fazelabAmount, double ab_Fas, int olgoo, double monthlyConsumption)
        {
            double fazelbDiscount = 0;
            //line->1916
            if (IsDomesticWithoutUnspecified(customerInfo.UsageId))
            {
                if (abBahaDiscount != 0 && fazelabAmount != 0)
                {
                    if (monthlyConsumption > olgoo)
                    {
                        fazelbDiscount += (int)(abBahaDiscount * 0.7);
                        fazelabAmount -= (int)(abBahaDiscount * 0.7);
                    }
                    else
                    {
                        fazelbDiscount += (int)(ab_Fas * 0.7);
                        fazelabAmount = 0;
                    }
                }
            }

            return fazelbDiscount;
            #region FazelabDiscount
            //TMP_NERKH.date2 > "1395/02/31" 
            //if (nerkh.Date2.CompareTo("1395/02/31") > 0 && zaribFaslAmountTemp != 0)
            //{
            //    if (V_FASBAHA1 != 0)
            //    {
            //        if ((IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId)) && IsNotConstruction(customerInfo.BranchType))
            //            V_FASBAHA1 = V_FASBAHA1 + (zaribFaslAmountTemp * 0.7);
            //        else
            //            V_FASBAHA1 = V_FASBAHA1 + (zaribFaslAmountTemp * 1);
            //    }

            //}//line -> 2009

            //if (IsReligiousWithCharity(customerInfo.UsageId))
            //{
            //    if (abBahaDiscountTemp != 0 && V_FASBAHA1 != 0)
            //    {
            //        fazelbDiscount += abBahaDiscountTemp;
            //        V_FASBAHA1 -= abBahaDiscountTemp;

            //        if (V_FASBAHA1 < 0)
            //        {
            //            V_FASBAHA1 = 0;
            //        }
            //    }
            //}

            //if (fazelbDiscount != 0)
            //{
            //    if ((IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId)) && IsNotConstruction(customerInfo.BranchType))
            //    {
            //        fazelbDiscount = fazelbDiscount;
            //        V_FASBAHA1 = V_FASBAHA1;
            //    }
            //    else
            //    {
            //        fazelbDiscount = fazelbDiscount + (VZFASL_olgo * 1);
            //        V_FASBAHA1 = V_FASBAHA1 - (VZFASL_olgo * 1);
            //    }
            //    if (V_FASBAHA1 < 0)
            //        V_FASBAHA1 = 0;
            //}//line-> 2050
            //throw new NotImplementedException();
            #endregion
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

            zabon_1 = PartTime(date1400_01_01, date1403_12_01, meterInfo.PreviousDateJalali, currentDateJalali);
            zabon_2 = PartTime(date1403_12_01, date1403_12_30, meterInfo.PreviousDateJalali, currentDateJalali);

            if (IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId))
                zabon_3 = PartTime(date1403_12_30, date1404_02_14, meterInfo.PreviousDateJalali, currentDateJalali);
            else
                zabon_3 = PartTime(date1403_12_30, date1404_02_31, meterInfo.PreviousDateJalali, currentDateJalali);

            if (IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId))
                zabon_4 = PartTime(date1404_02_14, date1404_12_29, meterInfo.PreviousDateJalali, currentDateJalali);
            else
                zabon_4 = PartTime(date1404_02_31, date1404_12_29, meterInfo.PreviousDateJalali, currentDateJalali);

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

            abonAbAmount = (((10000.0 / 30) * zabon_1) + ((35000.0 / 30) * zabon_2) + ((45500.0 / 30) * zabon_3) + ((58500.0 / 30) * zabon_4)) * sumUnit;

            if (abonAbAmount < 0)
                abonAbAmount = 0;

            if (IsConstruction(customerInfo.BranchType) || IsUsageConstructor(customerInfo.UsageId))
                abonAbAmount *= 2;

            return abonAbAmount;
        }
        private long CalculateAbonmanAbDiscount()
        {
            throw new NotImplementedException();
        }

        public double CalculateAbonmanFazelab(int totalDuration, CustomerInfoOutputDto customerInfo, string currentDateJalali, double abonmanAbBaha)
        {
            if (IsTankerSaleAndHousehold(customerInfo.UsageId) || customerInfo.SewageCalcState == 0)
            {
                return 0;
            }
            else if (customerInfo.SewageCalcState == 1)
            {
                int duration = (int.Parse)(CalculationDistanceDate.CalcDistance(customerInfo.SewageInstallationDateJalali, currentDateJalali));
                return (abonmanAbBaha / totalDuration) * duration;
            }

            return abonmanAbBaha;
        }
        private long CalculateAbonmanFazelabDiscount()
        {
            throw new NotImplementedException();
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
                int villageCode = int.Parse(customerInfo.VillageId.ToString().Substring(0, 4));
                if (monthlyConsumption > olgoo && domesticUnit > 1 && RuralButIsMetro(customerInfo.ZoneId, villageCode))
                {
                    return baseAmount * nerkh.PartialConsumption;
                }
                else
                {
                    return 0;
                }
            }
            //L 2642
            if (monthlyConsumption > olgoo && domesticUnit >= 1 && (IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId)))
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

        private long CalculateJavaniJamiatDiscount()
        {
            throw new NotImplementedException();
        }

        private double CalculateAvarez(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, double monthlyConsumption)
        {
            if (IsMoreThan1404_01_01(nerkh.Date2) && IsIndustrial(customerInfo.UsageId) && IsSpecialIndustrial(customerInfo.BranchType))
            {
                return monthlyConsumption <= 25000 ? nerkh.PartialConsumption * 2000 : nerkh.PartialConsumption * 20000;
            }
            return 0;
        }
        private long CalculateAvarezDiscount()
        {
            throw new NotImplementedException();
        }

        private long CalculateFasleGarm()
        {
            throw new NotImplementedException();
        }
        private long CalculateFasleGarmDiscount()
        {
            throw new NotImplementedException();
        }

        public double CalcMaliat(double abBahaAmount, double abonmanAbBahaAmount, double hotseasonAbBahaAmount, double fazelabAmount, double abonmanFazelabAmount, double hotseasonFazelabAmount, double boodjeAmount)
        {
            double sumAmount = abBahaAmount + abonmanAbBahaAmount + hotseasonAbBahaAmount + fazelabAmount + abonmanFazelabAmount + hotseasonFazelabAmount + boodjeAmount;
            return sumAmount * 0.10;
        }
    }
}
