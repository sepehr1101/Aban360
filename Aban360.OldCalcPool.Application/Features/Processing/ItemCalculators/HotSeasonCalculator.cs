using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffDateOperations;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.VirtualCapacityCalculator;

namespace Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators
{
    internal interface IHotSeasonCalculator
    {
        TariffItemResult CalcFazelab(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, double fazelabAmount, double monthlyConsumption, CalculateAbBahaOutputDto calcResult);
        TariffItemResult CalculateAb(NerkhGetDto nerkh, double abBahaAmount, CustomerInfoOutputDto customerInfo, double monthlyConsumption, CalculateAbBahaOutputDto calcResult);
        double CalculateDiscount(NerkhGetDto nerkh, double amountDiscount, TariffItemResult hotSeasonInfo, CustomerInfoOutputDto customerInfo, CalculateAbBahaOutputDto calcResult);
    }

    internal sealed class HotSeasonCalculator : IHotSeasonCalculator
    {
        const string date_02_31 = "/02/31";
        const string date_06_31 = "/06/31";
        const double _hotSeasonRate = 0.2;
        const int _firstSewageCalculation = 1;
        public TariffItemResult CalculateAb(NerkhGetDto nerkh, double abBahaAmount, CustomerInfoOutputDto customerInfo, double monthlyConsumption, CalculateAbBahaOutputDto calcResult)
        {           
            if (IsDomesticBelow25MeterConsumption(customerInfo, monthlyConsumption) &&
                !IsConstruction(customerInfo.BranchType))
            {
                return new TariffItemResult();
            }
            return GetDurationAndAmount(nerkh.Date1, nerkh.Date2, nerkh.Duration, customerInfo, abBahaAmount, calcResult);
        }

        public TariffItemResult CalcFazelab(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, double fazelabAmount, double monthlyConsumption, CalculateAbBahaOutputDto calcResult)
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


            if (customerInfo.SewageCalcState == _firstSewageCalculation)
            {
                calcResult.AbBaha1 = 0;
                calcResult.AbBaha2 = 0;
                return GetDurationAndAmount(nerkh.Date1, customerInfo.SewageInstallationDateJalali, nerkh.Duration, customerInfo, fazelabAmount, calcResult);               
            }
            return GetDurationAndAmount(nerkh.Date1, nerkh.Date2, nerkh.Duration, customerInfo, fazelabAmount, calcResult);           
        }

        public double CalculateDiscount(NerkhGetDto nerkh, double amountDiscount, TariffItemResult hotSeasonInfo, CustomerInfoOutputDto customerInfo, CalculateAbBahaOutputDto calcResult)
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
            if(calcResult.AbBahaAmount-amountDiscount<2)
            {
                return hotSeasonInfo.Allowed + hotSeasonInfo.Disallowed;
            }

            double fasleGarmAmount =  hotSeasonInfo.Disallowed; //* timePercentage * 0.2;
            double virtualDiscount = CalculateDiscountByVirtualCapacity(customerInfo, nerkh.PartialConsumption, nerkh.Duration, fasleGarmAmount);
            return virtualDiscount > 0 ? virtualDiscount : fasleGarmAmount;
        }

        private TariffItemResult GetDurationAndAmount(string date1, string date2, int duration, CustomerInfoOutputDto customerInfo, double baseAmount, CalculateAbBahaOutputDto calcResult)
        {
            string hotSeasonStart = GetHotSeasonStart(date2);
            string hotSeasonEnd = GetHotSeasonEnd(date2);
            int hotSeasonDuration = PartTime(hotSeasonStart, hotSeasonEnd, date1, date2, new { customerInfo.BillId, customerInfo.ZoneId, customerInfo.UsageId });
            double amount1 = hotSeasonDuration > 0 ? (int)((hotSeasonDuration * (calcResult.AbBaha1>0  ? calcResult.AbBaha1: baseAmount) / duration) * _hotSeasonRate) : 0;
            double amount2= hotSeasonDuration > 0 ? (int) ((hotSeasonDuration * (calcResult.AbBaha2> 0 ? calcResult.AbBaha2 : 0) / duration) * _hotSeasonRate) : 0;
            return new TariffItemResult(amount1, amount2, hotSeasonDuration);
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
    }
}
