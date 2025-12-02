using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using static Aban360.Common.Timing.CalculationDistanceDate;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.VirtualCapacityCalculator;

namespace Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators
{
    internal interface IBudgetCalculator
    {
        TariffItemResult Calculate(ConsumptionPartialInfo consumptionPartialInfo, CustomerInfoOutputDto customerInfo, string currentDateJalali, double monthlyConsumption, double olgoo, ConsumptionInfo consumptionInfo);
        TariffItemResult CalculateDiscount(ConsumptionPartialInfo consumptionPartialInfo, CustomerInfoOutputDto customerInfo, double abBahaDiscount, TariffItemResult boodjeAmounts);
    }

    internal sealed class BudgetCalculator : IBudgetCalculator
    {        
        const string date1404_01_01 = "1404/01/01";
        const string date1403_12_30 = "1403/12/30";
        const int _allowedMultiplier = 2000;
        const int _disAllowedMultiplier = 4000;
        const int _monthDays = 30;
        public TariffItemResult Calculate(ConsumptionPartialInfo partialConsumptionInfo, CustomerInfoOutputDto customerInfo, string currentDateJalali, double monthlyConsumption, double olgoo, ConsumptionInfo consumptionInfo)
        {
             double consumptionAfter1404 = 0;
            if (partialConsumptionInfo.EndDateJalali.CompareTo(date1404_01_01) < 0)
            {
                return new TariffItemResult();
            }
            if (partialConsumptionInfo.StartDateJalali.CompareTo(date1404_01_01) < 0 && partialConsumptionInfo.EndDateJalali.CompareTo(date1404_01_01) >= 0)
            {
                CalcDistanceResultDto calcDistance = CalcDistance(date1403_12_30, partialConsumptionInfo.EndDateJalali, true, customerInfo);
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
                consumptionAfter1404 = partialConsumptionInfo.Consumption;
            }
            if (IsConstruction(customerInfo.BranchType))
            {
                return new TariffItemResult(consumptionAfter1404 * _allowedMultiplier, 0);
            }
            if (IsUsageConstructor(customerInfo.UsageId))
            {
                return new TariffItemResult(consumptionAfter1404 * _allowedMultiplier, 0);
            }

            double partialOlgoo = IsDomesticCategory(customerInfo.UsageId) ?
                (double)consumptionInfo.FinalDomesticUnit * olgoo / _monthDays * partialConsumptionInfo.Duration :
                (double)customerInfo.ContractualCapacity / _monthDays * partialConsumptionInfo.Duration;

            double allowedConsumption = consumptionAfter1404 > partialOlgoo ? partialOlgoo : consumptionAfter1404;
            double disAllowedConsumption = consumptionAfter1404 - allowedConsumption;

            return new TariffItemResult(allowedConsumption * _allowedMultiplier, disAllowedConsumption * _disAllowedMultiplier);
        }
        public TariffItemResult CalculateDiscount(ConsumptionPartialInfo consumptionPartialInfo,CustomerInfoOutputDto customerInfo, double abBahaDiscount, TariffItemResult boodjeAmounts)
        {
            if (abBahaDiscount <= 0)
            {
                return new TariffItemResult();
            }
            if (boodjeAmounts.Allowed == 0)
            {
                return new TariffItemResult();
            }
            if (IsConstruction(customerInfo.BranchType))
            {
                return new TariffItemResult();
            }
            if (IsHandoverDiscount(customerInfo.BranchType) &&
                IsDomesticCategory(customerInfo.UsageId))
            {
                return new TariffItemResult(boodjeAmounts.Allowed);
            }
            //اگه مدرسه با شرایط تعریف شده باشه هم بالای ظرفیت هم زیر ظرفیت تخفیف داده میشه ??
            double virstualDiscount = CalculateDiscountByVirtualCapacity(customerInfo, consumptionPartialInfo.Consumption, consumptionPartialInfo.Duration, boodjeAmounts.Summation);

            double discount= virstualDiscount > 0 ? virstualDiscount : boodjeAmounts.Allowed;
            return new TariffItemResult(discount);
        }
    }
}
