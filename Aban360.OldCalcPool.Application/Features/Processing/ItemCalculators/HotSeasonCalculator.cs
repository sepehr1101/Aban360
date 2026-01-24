using Aban360.OldCalcPool.Application.Features.Processing.Helpers;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffDateOperations;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.VirtualCapacityCalculator;

namespace Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators
{
    internal interface IHotSeasonCalculator
    {
        TariffItemResult CalcFazelab(CustomerInfoOutputDto customerInfo, double fazelabAmount, double monthlyConsumption, TariffItemResult fazelabCalcResult, ConsumptionPartialInfo consumptionPartialInfo, bool isVillageCalculation, double villageMultiplier);
        TariffItemResult CalculateAb(double abBahaAmount, CustomerInfoOutputDto customerInfo, double monthlyConsumption, TariffItemResult calcResult, ConsumptionPartialInfo consumptionPartialInfo, bool isVillageCalculation, double villageMultiplier);
        TariffItemResult CalculateDiscount(double amountDiscount, TariffItemResult hotSeasonInfo, CustomerInfoOutputDto customerInfo, TariffItemResult calcResult, ConsumptionPartialInfo consumptionPartialInfo);
    }

    internal sealed class HotSeasonCalculator : IHotSeasonCalculator
    {
        //const string date_1404_02_31 = "1404/02/31";
        const string date_02_31 = "/02/31";
        const string date_06_31 = "/06/31";
        const double _hotSeasonRate = 0.2;
        const int _firstSewageCalculation = 1;
        public TariffItemResult CalculateAb(double abBahaAmount, CustomerInfoOutputDto customerInfo, double monthlyConsumption, TariffItemResult calcResult, ConsumptionPartialInfo consumptionPartialInfo, bool isVillageCalculation, double villageMultiplier)
        {           
            if (IsDomesticBelow25MeterConsumption(customerInfo, monthlyConsumption))
            {
                return new TariffItemResult();
            }
            return GetDurationAndAmount(consumptionPartialInfo.StartDateJalali, consumptionPartialInfo.EndDateJalali, consumptionPartialInfo.Duration, customerInfo, abBahaAmount, calcResult, consumptionPartialInfo, isVillageCalculation, villageMultiplier);
        }

        public TariffItemResult CalcFazelab(CustomerInfoOutputDto customerInfo, double fazelabAmount, double monthlyConsumption, TariffItemResult calcResult, ConsumptionPartialInfo consumptionPartialInfo, bool isVillageCalculation, double villageMultiplier)
        {
            if (IsDomesticBelow25MeterConsumption(customerInfo, monthlyConsumption))
            {
                return new TariffItemResult();
            }
            if (customerInfo.SewageCalcState == 0)
            {
                return new TariffItemResult();
            }
            if (IsConstruction(customerInfo.BranchType))
            {
                return new TariffItemResult();
            }
            if (IsUsageConstructor(customerInfo.UsageId))
            {
                return new TariffItemResult();
            }
            if (IsTankerSale(customerInfo.UsageId))
            {
                return new TariffItemResult();
            }

            string hotSeasonStart = GetHotSeasonStart(consumptionPartialInfo.StartDateJalali);
            string hotSeasonEnd = GetHotSeasonEnd(consumptionPartialInfo.StartDateJalali);
            int hotSeasonDuration = 0;
            double amount = 0;

            //double fazelabMultiplier = GetMultiplier(customerInfo.UsageId);
            if (customerInfo.SewageCalcState == _firstSewageCalculation)
            {               
                return GetDurationAndAmount(consumptionPartialInfo.StartDateJalali, customerInfo.SewageInstallationDateJalali, consumptionPartialInfo.Duration, customerInfo, fazelabAmount, calcResult, consumptionPartialInfo, isVillageCalculation, villageMultiplier, aboveZero:false);               
            }
            return GetDurationAndAmount(consumptionPartialInfo.StartDateJalali, consumptionPartialInfo.EndDateJalali, consumptionPartialInfo.Duration, customerInfo, fazelabAmount, calcResult, consumptionPartialInfo, isVillageCalculation, villageMultiplier, aboveZero: true );         
        }

        public TariffItemResult CalculateDiscount(double amountDiscount, TariffItemResult hotSeasonInfo, CustomerInfoOutputDto customerInfo, TariffItemResult calcResult, ConsumptionPartialInfo consumptionPartialInfo)
        {
            if (amountDiscount == 0)
            {
                return new TariffItemResult();
            }
            if (hotSeasonInfo.TmpDuration == 0 || hotSeasonInfo.Allowed == 0)
            {
                return new TariffItemResult();
            }
            if (IsConstruction(customerInfo.BranchType))
            {
                return new TariffItemResult();
            }
            if (IsUsageConstructor(customerInfo.UsageId))
            {
                return new TariffItemResult();
            }
            if (IsUnderSocialService(customerInfo.BranchType) &&
                IsDomesticWithoutUnspecified(customerInfo.UsageId))
            {
                return new TariffItemResult();
            }

            if (IsMullah(customerInfo.BranchType))
            {
                return new TariffItemResult();
            }
            if (calcResult.Summation - amountDiscount < 2)
            {
                return new TariffItemResult(hotSeasonInfo.Summation);
            }
            if (IsReligiousWithCharity(customerInfo.UsageId))
            {
                return new TariffItemResult(hotSeasonInfo.Allowed);
            }
            if (IsQuranAfter1404_01_01(customerInfo.UsageId, consumptionPartialInfo.StartDateJalali))
            {
                return new TariffItemResult(hotSeasonInfo.Allowed);
            }
            /* if (date_1404_02_31.MoreOrEq(consumptionPartialInfo.EndDateJalali) && IsSchool(customerInfo.UsageId))
             {
                 return new TariffItemResult();
             }*/
            double fasleGarmAmount = hotSeasonInfo.Disallowed;
            double virtualDiscount = CalculateDiscountByVirtualCapacity(customerInfo, consumptionPartialInfo.Consumption, consumptionPartialInfo.Duration, fasleGarmAmount, consumptionPartialInfo);
            double finalDiscount = virtualDiscount > 0 ? virtualDiscount : fasleGarmAmount;
            return new TariffItemResult(finalDiscount);
        }

        #region private methods
        private TariffItemResult GetDurationAndAmount(string date1, string date2, int duration, CustomerInfoOutputDto customerInfo, double baseAmount, TariffItemResult fazelabCalcResult, ConsumptionPartialInfo consumptionPartialInfo, bool isVillageCalculation, double villageMultiplier, bool aboveZero = true, double fazelabMultiplier = 1)
        {
            string hotSeasonStart = GetHotSeasonStart(date2);
            string hotSeasonEnd = GetHotSeasonEnd(date2);
            int hotSeasonDuration = PartTime(hotSeasonStart, hotSeasonEnd, date1, date2, new { customerInfo.BillId, customerInfo.ZoneId, customerInfo.UsageId });
            double amount1 = hotSeasonDuration > 0 ? (long)((hotSeasonDuration * (fazelabCalcResult.Allowed > 0 && aboveZero ? fazelabCalcResult.Allowed : baseAmount) / duration) * _hotSeasonRate) : 0;
            //calcResult.Disallowed> 0 بابت یک باگ اضافه شده که بعدا باید اصولی تر رفع شود: در صورتی که مبلغ زیر الگو یا ظرفیت صفر باشد اما مبلغ بالای ظرفیت عدد داشته باشد در مبلغ1 یکبار محاسبه فصل گرم اتفاق می افتد
            double amount2 = hotSeasonDuration > 0 ? (long)((hotSeasonDuration * (fazelabCalcResult.Allowed > 0 && fazelabCalcResult.Disallowed > 0 && aboveZero ? fazelabCalcResult.Disallowed : 0) / duration) * _hotSeasonRate) : 0;

            if (IsUnderSocialService(customerInfo.BranchType) &&
               IsDomesticWithoutUnspecified(customerInfo.UsageId))
            {
                if ("1403/12/30".MoreOrEq(consumptionPartialInfo.EndDateJalali))
                {
                    return new TariffItemResult(0, amount2 * fazelabMultiplier, hotSeasonDuration);
                }

                double allowedDiscount = isVillageCalculation?(fazelabCalcResult.Allowed / 0.65) * 0.5: fazelabCalcResult.Allowed;
                double remained = fazelabCalcResult.Summation - allowedDiscount;
                double hotseasonDiscount = hotSeasonDuration > 0 ? (hotSeasonDuration * remained / duration) * _hotSeasonRate : 0;

                return new TariffItemResult(0, hotseasonDiscount * fazelabMultiplier, hotSeasonDuration);
            }
            if (IsMullah(customerInfo.BranchType) && IsVillage(customerInfo.ZoneId))
            {
                // بخش زیر الگو صفر محاسبه شده بود که طبق نظر سرکار خانم حبیبی نژاد در 28 دی 1404 تغییر کرد
                if ("1403/12/30".MoreOrEq(consumptionPartialInfo.EndDateJalali))
                {
                    return new TariffItemResult(0, amount2 * fazelabMultiplier, hotSeasonDuration);
                }
                //else 1404 or more
                double allowedDiscount = (fazelabCalcResult.Allowed / 0.65) * 0.5;
                double remained = fazelabCalcResult.Summation - allowedDiscount;
                double hotseasonMullah= hotSeasonDuration > 0 ? (hotSeasonDuration * remained / duration) * _hotSeasonRate : 0;
                return new TariffItemResult(0, hotseasonMullah * fazelabMultiplier, hotSeasonDuration);
            }
            return new TariffItemResult(amount1 * fazelabMultiplier, amount2 * fazelabMultiplier, hotSeasonDuration);
        }
        private bool IsDomesticBelow25MeterConsumption(CustomerInfoOutputDto customerInfo, double monthlyConsumption)
        {
            return IsDomesticCategory(customerInfo.UsageId) && 
                   monthlyConsumption <= 25 &&
                   !IsConstruction(customerInfo.BranchType);
        }

        private string GetHotSeasonStart(string date1)
        {
            return date1.Substring(0, 4) + date_02_31;
        }
        private string GetHotSeasonEnd(string date2)
        {
            return date2.Substring(0, 4) + date_06_31;
        }
        #endregion
    }
}