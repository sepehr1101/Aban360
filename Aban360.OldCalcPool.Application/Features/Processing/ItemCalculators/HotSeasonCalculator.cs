using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffDateOperations;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.VirtualCapacityCalculator;

namespace Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators
{
    internal interface IHotSeasonCalculator
    {
        (int, double) CalcFazelab(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, double fazelabAmount, double monthlyConsumption);
        (int, double) CalculateAb(NerkhGetDto nerkh, double abBahaAmount, CustomerInfoOutputDto customerInfo, double monthlyConsumption);
        double CalculateDiscount(NerkhGetDto nerkh, double amountDiscount, (int, double) hotSeasonInfo, CustomerInfoOutputDto customerInfo);
    }

    internal sealed class HotSeasonCalculator : IHotSeasonCalculator
    {
        const string date_02_31 = "/02/31";
        const string date_06_31 = "/06/31";
        const double _hotSeasonRate = 0.2;
        const int _firstSewageCalculation = 1;
        public (int, double) CalculateAb(NerkhGetDto nerkh, double abBahaAmount, CustomerInfoOutputDto customerInfo, double monthlyConsumption)
        {
            if (IsDomesticBelow25MeterConsumption(customerInfo, monthlyConsumption) &&
                !IsConstruction(customerInfo.BranchType))
            {
                return (0, 0);
            }
            return GetDurationAndAmount(nerkh.Date1, customerInfo.SewageInstallationDateJalali, nerkh.Duration, customerInfo, abBahaAmount);

            //int hotSeasonDuration = PartTime(hotSeasonStart, hotSeasonEnd, nerkh.Date1, nerkh.Date2, new { customerInfo.BillId, customerInfo.ZoneId, customerInfo.UsageId });
            //double hotSeasonAmount = hotSeasonDuration > 0 ? (int)((hotSeasonDuration * abBahaAmount / nerkh.Duration) * _hotSeasonRate) : 0;
            //return (hotSeasonDuration, hotSeasonAmount);
        }

        public (int, double) CalcFazelab(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, double fazelabAmount, double monthlyConsumption)
        {
            if (IsDomesticBelow25MeterConsumption(customerInfo, monthlyConsumption))
            {
                return (0, 0);
            }
            if (customerInfo.SewageCalcState == 0)
            {
                return (0, 0);
            }

            string hotSeasonStart = GetHotSeasonStart(nerkh.Date2);
            string hotSeasonEnd = GetHotSeasonEnd(nerkh.Date2);
            int hotSeasonDuration = 0;
            double amount = 0;


            if (customerInfo.SewageCalcState == _firstSewageCalculation)
            {
                return GetDurationAndAmount(nerkh.Date1, customerInfo.SewageInstallationDateJalali, nerkh.Duration, customerInfo, fazelabAmount);
                //hotSeasonDuration = PartTime(hotSeasonStart, hotSeasonEnd, customerInfo.SewageInstallationDateJalali, nerkh.Date2, new { customerInfo.BillId, customerInfo.ZoneId, customerInfo.UsageId });
                //amount = hotSeasonDuration > 0 ? (int)((hotSeasonDuration * fazelabAmount / nerkh.Duration) * 0.2) : 0;
                //return (hotSeasonDuration, amount);
            }
            return GetDurationAndAmount(nerkh.Date1, nerkh.Date2, nerkh.Duration, customerInfo, fazelabAmount);
            //hotSeasonDuration = PartTime(hotSeasonStart, hotSeasonEnd, nerkh.Date1, nerkh.Date2, new { customerInfo.BillId, customerInfo.ZoneId, customerInfo.UsageId });
            //amount = hotSeasonDuration > 0 ? (int)((hotSeasonDuration * fazelabAmount / nerkh.Duration) * 0.2) : 0;
            //return (hotSeasonDuration, amount);
        }

        public double CalculateDiscount(NerkhGetDto nerkh, double amountDiscount, (int, double) hotSeasonInfo, CustomerInfoOutputDto customerInfo)
        {
            if (amountDiscount == 0)
            {
                return 0;
            }
            if (hotSeasonInfo.Item1 == 0 || hotSeasonInfo.Item2 == 0)
            {
                return 0;
            }

            double timePercentage = (double)hotSeasonInfo.Item1 / (double)nerkh.Duration;
            double fasleGarmAmount = amountDiscount * timePercentage * 0.2;
            double virtualDiscount = CalculateDiscountByVirtualCapacity(customerInfo, nerkh.PartialConsumption, nerkh.Duration, fasleGarmAmount);
            return virtualDiscount > 0 ? virtualDiscount : fasleGarmAmount;
        }

        private (int, double) GetDurationAndAmount(string date1, string date2, int duration, CustomerInfoOutputDto customerInfo, double baseAmount)
        {
            string hotSeasonStart = GetHotSeasonStart(date2);
            string hotSeasonEnd = GetHotSeasonEnd(date2);
            int hotSeasonDuration = PartTime(hotSeasonStart, hotSeasonEnd, date1, date2, new { customerInfo.BillId, customerInfo.ZoneId, customerInfo.UsageId });
            double amount = hotSeasonDuration > 0 ? (int)((hotSeasonDuration * baseAmount / duration) * _hotSeasonRate) : 0;
            return (hotSeasonDuration, amount);
        }
        private bool IsDomesticBelow25MeterConsumption(CustomerInfoOutputDto customerInfo, double monthlyConsumption)
        {
            return IsDomesticCategory(customerInfo.UsageId) && monthlyConsumption <= 25;
        }

        private string GetHotSeasonStart(string date2)
        {
            return date2.Substring(0, 4) + date_02_31;
        }
        private string GetHotSeasonEnd(string date2)
        {
            return date2.Substring(0, 4) + date_06_31;
        }
    }
}
