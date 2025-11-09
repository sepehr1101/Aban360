using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using System.Runtime.InteropServices;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffStringChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffDateOperations;
using static Aban360.Common.Timing.CalculationDistanceDate;
using Aban360.OldCalcPool.Application.Features.Processing.Helpers;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Base
{
    internal abstract class BaseOldTariffEngine : BaseExpressionCalculator
    {
        int monthDays = 30;
        private readonly IAbBahaCalculator _abBahaCalculator;

        internal BaseOldTariffEngine(IAbBahaCalculator abBahaCalculator)
        {
            _abBahaCalculator = abBahaCalculator;
            _abBahaCalculator.NotNull();
        }

        /// <summary>
        ///  تابع اصلی با دسترسی اینترنال بابت محاسبه تک رکورد جدول نرخ
        /// </summary>
        /// <returns>مقدار خروجی بعد از اتمام نوشتن کد، اصلاح شود</returns>
        internal BaseOldTariffEngineOutputDto CalculateWaterBill(NerkhGetDto nerkh, AbAzadFormulaDto abAzad, ZaribGetDto zarib, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, string currentDateJalali, ConsumptionInfo consumptionInfo, int _olgoo, [Optional] int? c, [Optional] IEnumerable<int>? tagIds)
        {
            DateOnly previousDate = ConvertJalaliToGregorian(meterInfo.PreviousDateJalali);
            DateOnly currentDate = ConvertJalaliToGregorian(currentDateJalali);
            nerkh.DailyAverageConsumption = consumptionInfo.DailyAverageConsumption;
            (nerkh, nerkh.Duration, nerkh.PartialConsumption) = CalcPartial(nerkh, previousDate, currentDate, consumptionInfo);

            int olgoo = GetOlgoo(nerkh.Date2, _olgoo);
            bool isVillageCalculation = IsVillage(customerInfo.ZoneId);
            double monthlyConsumption = nerkh.DailyAverageConsumption * monthDays;
            decimal multiplierAbBaha = GetMultiplier(zarib, olgoo, IsDomesticCategory(customerInfo.UsageId), isVillageCalculation, monthlyConsumption, customerInfo.BranchType);

            CalculateAbBahaOutputDto abBahaResult = _abBahaCalculator.CalculateAbBaha(nerkh, customerInfo, meterInfo, zarib, abAzad, currentDateJalali, isVillageCalculation, monthlyConsumption, olgoo, multiplierAbBaha, c, tagIds); //CalculateAbBaha(nerkh, customerInfo, meterInfo, zarib, abAzad, currentDateJalali, isVillageCalculation, monthlyConsumption, olgoo, multiplierAbBaha, c, tagIds);
            (double, double) boodje = CalculateBoodje(nerkh, customerInfo, currentDateJalali, monthlyConsumption, olgoo,consumptionInfo);
            double fazelab = CalculateFazelab(nerkh.Date1, nerkh.Date2, nerkh.Duration, customerInfo, abBahaResult.AbBahaAmount, currentDateJalali, false);
            (int, double) hotSeasonAbBaha = CalcHotSeasonAbBaha(nerkh, abBahaResult.AbBahaAmount, customerInfo, monthlyConsumption);
            (int, double) hotSeasonFazelab = CalcHotSeasonFaselab(nerkh, customerInfo, fazelab, monthlyConsumption);
            double avarez = CalculateAvarez(nerkh, customerInfo, monthlyConsumption);
            double javani = CalculateJavaniJamiat(nerkh, customerInfo, abBahaResult.AbBahaAmount, monthlyConsumption, olgoo);
                        
            //Discounts
            double abBahaDiscount = CalculateItemDiscount(customerInfo, nerkh, olgoo, (long)abBahaResult.AbBahaAmount, false, multiplierAbBaha, consumptionInfo.FinalDomesticUnit);
            double fazelabDiscount = CalculateFazelabDiscount(abBahaDiscount, fazelab, customerInfo, nerkh);
            double hotSeasonAbDiscount = CalculateFasleGarmDiscount(nerkh, abBahaDiscount, hotSeasonAbBaha, customerInfo);
            double hotSeasonFazelabDiscount = CalculateFasleGarmDiscount(nerkh, fazelabDiscount, hotSeasonFazelab, customerInfo);
            double avarezDiscount = 0;
            double javaniDiscount = 0;
            double boodjeDiscount = CalculateBoodjeDiscount(customerInfo,abBahaDiscount,boodje, nerkh);
            return new BaseOldTariffEngineOutputDto(abBahaResult, fazelab, hotSeasonAbBaha.Item2, hotSeasonFazelab.Item2, boodje.Item1, boodje.Item2, abBahaDiscount, hotSeasonAbDiscount, fazelabDiscount, 0, avarez, javani,
                0, 0, avarezDiscount, javaniDiscount, boodjeDiscount);
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
        internal double CalculateFazelab(string date1, string date2, int durationAll, CustomerInfoOutputDto customerInfo, double abBahaItemAmount, string currentDateJalali, bool isAbonman)
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
            if (string.Compare(customerInfo.SewageRegisterDate, "1330/0/01") >= 0 &&
               string.Compare(customerInfo.SewageRegisterDate, date2) > 0)
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

        internal double CalculateAbonmanAb(CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, string currentDateJalali)
        {
            string date1400_01_01 = "1400/01/01";
            string date1403_12_01 = "1403/12/01";
            string date1403_12_30 = "1403/12/30";
            string date1404_02_14 = "1404/02/14";
            string date1404_02_31 = "1404/02/31";
            string date1404_12_29 = "1404/12/29";

            if (!IsConstruction(customerInfo.BranchType) && IsTankerSale(customerInfo.UsageId))
            {
                return 0;
            }

            double abonAbAmount = 0;//, abonAbDiscount = 0;
            double zabon_1 = 0, zabon_2 = 0, zabon_3 = 0, zabon_4 = 0;

            zabon_1 = PartTime(date1400_01_01, date1403_12_01, meterInfo.PreviousDateJalali, currentDateJalali, new { BillId = customerInfo.BillId, ZoneId = customerInfo.ZoneId, UsageId = customerInfo.UsageId });
            zabon_2 = PartTime(date1403_12_01, date1403_12_30, meterInfo.PreviousDateJalali, currentDateJalali, new { BillId = customerInfo.BillId, ZoneId = customerInfo.ZoneId, UsageId = customerInfo.UsageId });

            if (IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId))
            {
                zabon_3 = PartTime(date1403_12_30, date1404_02_14, meterInfo.PreviousDateJalali, currentDateJalali, new { BillId = customerInfo.BillId, ZoneId = customerInfo.ZoneId, UsageId = customerInfo.UsageId });
            }
            else
            {
                zabon_3 = PartTime(date1403_12_30, date1404_02_31, meterInfo.PreviousDateJalali, currentDateJalali, new { BillId = customerInfo.BillId, ZoneId = customerInfo.ZoneId, UsageId = customerInfo.UsageId });
            }

            if (IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId))
            {
                zabon_4 = PartTime(date1404_02_14, date1404_12_29, meterInfo.PreviousDateJalali, currentDateJalali, new { BillId = customerInfo.BillId, ZoneId = customerInfo.ZoneId, UsageId = customerInfo.UsageId });
            }
            else
            {
                zabon_4 = PartTime(date1404_02_31, date1404_12_29, meterInfo.PreviousDateJalali, currentDateJalali, new { BillId = customerInfo.BillId, ZoneId = customerInfo.ZoneId, UsageId = customerInfo.UsageId });
            }

            zabon_1 = Math.Max(zabon_1, 0);
            zabon_2 = Math.Max(zabon_2, 0);
            zabon_3 = Math.Max(zabon_3, 0);
            zabon_4 = Math.Max(zabon_4, 0);

            int sumUnit = customerInfo.OtherUnit + customerInfo.DomesticUnit + customerInfo.CommertialUnit;

            if (IsVillageCollectorMeter(customerInfo.UsageId))
            {
                sumUnit = 1;
            }

            if (sumUnit <= 0)
            {
                sumUnit = 1;
            }

            abonAbAmount = (((10000.0 / monthDays) * zabon_1) + ((35000.0 / monthDays) * zabon_2) + ((45500.0 / monthDays) * zabon_3) + ((58500.0 / monthDays) * zabon_4)) * sumUnit;

            if (abonAbAmount < 0)
            {
                abonAbAmount = 0;
            }

            if (IsConstruction(customerInfo.BranchType) || IsUsageConstructor(customerInfo.UsageId))
            {
                abonAbAmount *= 2;
            }

            return abonAbAmount;
        }

        internal double CalculateAbonmanDiscount(int usageId, double abonmanAmount, double bahaDiscountAmount, bool isSpecial)
        {
            if (IsSpecialEducation(usageId, isSpecial))
            {
                return 0;
            }
            return bahaDiscountAmount > 0 && !IsReligiousWithCharity(usageId) ? abonmanAmount : 0;
        }

        private decimal GetMultiplier(ZaribGetDto zarib, int olgo, bool isDomestic, bool isVillage, double monthlyConsumption, int branchType)
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
        
        private int GetOlgoo(string nerkhDate2, int olgo)
        {
            string baseDate = "1403/12/30";
            return nerkhDate2.CompareTo(baseDate) <= 0 ? 14 : olgo;
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
               
        private (double, double) CalculateBoodje(NerkhGetDto nerkhDto, CustomerInfoOutputDto customerInfo, string currentDateJalali, double monthlyConsumption, double olgoo, ConsumptionInfo consumptionInfo)
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
                CalcDistanceResultDto calcDistance = CalcDistance(_1403_12_30, nerkhDto.Date2, true, customerInfo);
                int durationAfter1404 = 0;
                if (calcDistance.HasError)
                {
                    throw new TariffDateException(customerInfo.BillId + " - " + ExceptionLiterals.Incalculable);
                }
                durationAfter1404 = calcDistance.Distance;


                consumptionAfter1404 = ((double)consumptionInfo.Consumption / consumptionInfo.Duration) * (double)durationAfter1404;
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
                (double)consumptionInfo.FinalDomesticUnit * olgoo / 30 * nerkhDto.Duration :
                (double)customerInfo.ContractualCapacity / 30 * nerkhDto.Duration;

            double allowedConsumption = consumptionAfter1404 > partialOlgoo ? partialOlgoo : consumptionAfter1404;
            double disAllowedConsumption = consumptionAfter1404 - allowedConsumption;

            return (allowedConsumption * 2000, disAllowedConsumption * 4000);
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
                    TariffRuleChecker.RuralButIsMetro(customerInfo.ZoneId, villageCode))
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
            double virstualDiscount = CalculateDiscountByVirtualCapacity(customerInfo, nerkh.PartialConsumption, nerkh.Duration, boodjeAmounts.Item1 + boodjeAmounts.Item2);

            return virstualDiscount>0? virstualDiscount: boodjeAmounts.Item1;
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
            double virtualDiscount = CalculateDiscountByVirtualCapacity(customerInfo, nerkh.PartialConsumption, nerkh.Duration, fazelabDiscount);
            return virtualDiscount > 0 ? virtualDiscount : fazelabAmount;
        }

        private double CalculateFasleGarmDiscount(NerkhGetDto nerkh, double amountDiscount, (int,double)hotSeasonInfo, CustomerInfoOutputDto customerInfo)
        {
            if (amountDiscount == 0)
            {
                return 0;
            }
            if(hotSeasonInfo.Item1==0 || hotSeasonInfo.Item2==0)
            {
                return 0;
            }

            double timePercentage = (double)hotSeasonInfo.Item1 / (double)nerkh.Duration;
            double fasleGarmAmount = amountDiscount * timePercentage * 0.2;
            double virtualDiscount = CalculateDiscountByVirtualCapacity(customerInfo, nerkh.PartialConsumption, nerkh.Duration, fasleGarmAmount);
            return virtualDiscount > 0 ? virtualDiscount : fasleGarmAmount;
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
                if (nerkh.PartialConsumption <= olgoo)// در صورتی که مصرف زیر الگو بود کامل معاف میشود
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
            double virtualDiscount = CalculateDiscountByVirtualCapacity(customerInfo, nerkh.PartialConsumption, nerkh.Duration, amount);
            return virtualDiscount > 0 ? (long)virtualDiscount : 0;
        }        

        private int GetVirtualCapacity(CustomerInfoOutputDto customerInfo, int duration)
        {
            float multiplier = GetVirtualMultiplier(customerInfo.VirtualCategoryId);
            if(IsSpecialEducation(customerInfo.UsageId, customerInfo.IsSpecial) && multiplier>0)
            {
                double partialCapacity = (double)customerInfo.ContractualCapacity / monthDays * duration;                
                return Convert.ToInt32(Math.Round(partialCapacity * multiplier,1));
            }
            return 0;
        }
        private float GetVirtualMultiplier(int virtualCategoryId)
        {
            switch (virtualCategoryId)
            {
                case 0:
                    return 0.45f / 0.9f;
                case 1:
                    return 0.45f / 0.9f;
                case 2:
                    return 1.2f / 1.05f;
                case 3:
                    return 4.5f / 3.9f;
                case 4:
                    return 1.2f / 1.05f;
                case 5:
                    return 4.5f / 3.9f;
                case 6:
                    return 0.45f / 0.9f;
                case 7:
                    return 3.6f / 3.9f;
                case 8:
                    return 2.4f / 2.1f;
                default:
                    return 0;
            }
        }
        private double CalculateDiscountByVirtualCapacity(CustomerInfoOutputDto customerInfo, double partialConsumption, int duration, double amount)
        {
            int virtualCapacity = GetVirtualCapacity(customerInfo, duration);
            if (virtualCapacity > 0)
            {                
                return partialConsumption <= virtualCapacity ? amount : 0;
            }
            return 0;
        }
    }
}
