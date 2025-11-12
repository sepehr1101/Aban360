using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using static Aban360.Common.Timing.CalculationDistanceDate;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.VirtualCapacityCalculator;

namespace Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators
{
    internal interface IBudgetCalculator
    {
        (double, double) Calculate(NerkhGetDto nerkhDto, CustomerInfoOutputDto customerInfo, string currentDateJalali, double monthlyConsumption, double olgoo, ConsumptionInfo consumptionInfo);
        double CalculateDiscount(CustomerInfoOutputDto customerInfo, double abBahaDiscount, (double, double) boodjeAmounts, NerkhGetDto nerkh);
    }

    internal sealed class BudgetCalculator : IBudgetCalculator
    {
        public (double, double) Calculate(NerkhGetDto nerkhDto, CustomerInfoOutputDto customerInfo, string currentDateJalali, double monthlyConsumption, double olgoo, ConsumptionInfo consumptionInfo)
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
            if (IsUsageConstructor(customerInfo.UsageId))
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
        public double CalculateDiscount(CustomerInfoOutputDto customerInfo, double abBahaDiscount, (double, double) boodjeAmounts, NerkhGetDto nerkh)
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

            return virstualDiscount > 0 ? virstualDiscount : boodjeAmounts.Item1;
        }
    }
}
