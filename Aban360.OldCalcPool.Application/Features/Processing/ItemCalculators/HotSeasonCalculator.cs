using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffDateOperations;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.VirtualCapacityCalculator;

namespace Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators
{
    internal interface IHotSeasonCalculator
    {
        TariffItemResult CalcFazelab(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, double fazelabAmount, double monthlyConsumption, TariffItemResult calcResult);
        TariffItemResult CalculateAb(NerkhGetDto nerkh, double abBahaAmount, CustomerInfoOutputDto customerInfo, double monthlyConsumption, TariffItemResult calcResult);
        double CalculateDiscount(NerkhGetDto nerkh, double amountDiscount, TariffItemResult hotSeasonInfo, CustomerInfoOutputDto customerInfo, TariffItemResult calcResult);
    }

    internal sealed class HotSeasonCalculator : IHotSeasonCalculator
    {
        const string date_02_31 = "/02/31";
        const string date_06_31 = "/06/31";
        const double _hotSeasonRate = 0.2;
        const int _firstSewageCalculation = 1;
        public TariffItemResult CalculateAb(NerkhGetDto nerkh, double abBahaAmount, CustomerInfoOutputDto customerInfo, double monthlyConsumption, TariffItemResult calcResult)
        {           
            if (IsDomesticBelow25MeterConsumption(customerInfo, monthlyConsumption) &&
                !IsConstruction(customerInfo.BranchType))
            {
                return new TariffItemResult();
            }
            return GetDurationAndAmount(nerkh.Date1, nerkh.Date2, nerkh.Duration, customerInfo, abBahaAmount, calcResult);
        }

        public TariffItemResult CalcFazelab(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, double fazelabAmount, double monthlyConsumption, TariffItemResult calcResult)
        {
            if (IsDomesticBelow25MeterConsumption(customerInfo, monthlyConsumption))
            {
                return new TariffItemResult();
            }
            if (customerInfo.SewageCalcState == 0)
            {
                return new TariffItemResult();
            }

            string hotSeasonStart = GetHotSeasonStart(nerkh.Date1);
            string hotSeasonEnd = GetHotSeasonEnd(nerkh.Date2);
            int hotSeasonDuration = 0;
            double amount = 0;

            double fazelabMultiplier= GetMultiplier(customerInfo.UsageId);
            if (customerInfo.SewageCalcState == _firstSewageCalculation)
            {               
                return GetDurationAndAmount(nerkh.Date1, customerInfo.SewageInstallationDateJalali, nerkh.Duration, customerInfo, fazelabAmount, calcResult, aboveZero:false, fazelabMultiplier);               
            }
            return GetDurationAndAmount(nerkh.Date1, nerkh.Date2, nerkh.Duration, customerInfo, fazelabAmount, calcResult, true, fazelabMultiplier);         
        }

        public double CalculateDiscount(NerkhGetDto nerkh, double amountDiscount, TariffItemResult hotSeasonInfo, CustomerInfoOutputDto customerInfo, TariffItemResult calcResult)
        {
            if (amountDiscount == 0)
            {
                return 0;
            }
            if (hotSeasonInfo.TmpDuration == 0 || hotSeasonInfo.Allowed == 0)
            {
                return 0;
            }
            if (IsConstruction(customerInfo.BranchType))
            {
                return 0;
            }
            if (calcResult.Summation - amountDiscount < 2)
            {
                return hotSeasonInfo.Summation;
            }
            if (IsReligiousWithCharity(customerInfo.UsageId))
            {
                return hotSeasonInfo.Allowed;
            }
            double fasleGarmAmount = hotSeasonInfo.Disallowed;
            double virtualDiscount = CalculateDiscountByVirtualCapacity(customerInfo, nerkh.PartialConsumption, nerkh.Duration, fasleGarmAmount);
            return virtualDiscount > 0 ? virtualDiscount : fasleGarmAmount;
        }

        private TariffItemResult GetDurationAndAmount(string date1, string date2, int duration, CustomerInfoOutputDto customerInfo, double baseAmount, TariffItemResult calcResult, bool aboveZero=true, double fazelabMultiplier=1)
        {
            string hotSeasonStart = GetHotSeasonStart(date2);
            string hotSeasonEnd = GetHotSeasonEnd(date2);
            int hotSeasonDuration = PartTime(hotSeasonStart, hotSeasonEnd, date1, date2, new { customerInfo.BillId, customerInfo.ZoneId, customerInfo.UsageId });
            double amount1 = hotSeasonDuration > 0 ? (int)((hotSeasonDuration * (calcResult.Allowed>0 && aboveZero ? calcResult.Allowed: baseAmount) / duration) * _hotSeasonRate) : 0;
            double amount2= hotSeasonDuration > 0 ? (int) ((hotSeasonDuration * (calcResult.Disallowed> 0 && aboveZero ? calcResult.Disallowed : 0) / duration) * _hotSeasonRate) : 0;
            return new TariffItemResult(amount1*fazelabMultiplier, amount2*fazelabMultiplier, hotSeasonDuration);
        }
        private bool IsDomesticBelow25MeterConsumption(CustomerInfoOutputDto customerInfo, double monthlyConsumption)
        {
            return IsDomesticCategory(customerInfo.UsageId) && monthlyConsumption <= 25;
        }

        private string GetHotSeasonStart(string date1)
        {
            return date1.Substring(0, 4) + date_02_31;
        }
        private string GetHotSeasonEnd(string date2)
        {
            return date2.Substring(0, 4) + date_06_31;
        }
        private double GetMultiplier(int usageId)
        {
            return IsDomesticCategory(usageId) ? 0.7 : 1;
        }
    }
}
