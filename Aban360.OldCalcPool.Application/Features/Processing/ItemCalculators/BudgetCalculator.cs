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
        TariffItemResult Calculate(NerkhGetDto nerkhDto, CustomerInfoOutputDto customerInfo, string currentDateJalali, double monthlyConsumption, double olgoo, ConsumptionInfo consumptionInfo);
        double CalculateDiscount(CustomerInfoOutputDto customerInfo, double abBahaDiscount, TariffItemResult boodjeAmounts, NerkhGetDto nerkh);
    }

    internal sealed class BudgetCalculator : IBudgetCalculator
    {
        const int _allowedMultiplier = 2000;
        const int _disAllowedMultiplier = 4000;
        const int _monthDays = 30;
        public TariffItemResult Calculate(NerkhGetDto nerkhDto, CustomerInfoOutputDto customerInfo, string currentDateJalali, double monthlyConsumption, double olgoo, ConsumptionInfo consumptionInfo)
        {
            double consumptionAfter1404 = 0;
            string _1404_01_01 = "1404/01/01";
            string _1403_12_30 = "1403/12/30";
            if (nerkhDto.Date2.CompareTo(_1404_01_01) < 0)
            {
                return new TariffItemResult();
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
                return new TariffItemResult(consumptionAfter1404 * _allowedMultiplier, 0);
            }
            if (IsUsageConstructor(customerInfo.UsageId))
            {
                return new TariffItemResult(consumptionAfter1404 * _allowedMultiplier, 0);
            }

            double partialOlgoo = IsDomesticCategory(customerInfo.UsageId) ?
                (double)consumptionInfo.FinalDomesticUnit * olgoo / _monthDays * nerkhDto.Duration :
                (double)customerInfo.ContractualCapacity / _monthDays * nerkhDto.Duration;

            double allowedConsumption = consumptionAfter1404 > partialOlgoo ? partialOlgoo : consumptionAfter1404;
            double disAllowedConsumption = consumptionAfter1404 - allowedConsumption;

            return new TariffItemResult(allowedConsumption * _allowedMultiplier, disAllowedConsumption * _disAllowedMultiplier);
        }
        public double CalculateDiscount(CustomerInfoOutputDto customerInfo, double abBahaDiscount, TariffItemResult boodjeAmounts, NerkhGetDto nerkh)
        {
            if (abBahaDiscount <= 0)
            {
                return 0;
            }
            if (boodjeAmounts.Allowed == 0)
            {
                return 0;
            }
            if (IsConstruction(customerInfo.BranchType))
            {
                return 0;
            }
            if (IsHandoverDiscount(customerInfo.BranchType) &&
                IsDomesticCategory(customerInfo.UsageId))
            {
                return boodjeAmounts.Allowed;
            }
            //اگه مدرسه با شرایط تعریف شده باشه هم بالای ظرفیت هم زیر ظرفیت تخفیف داده میشه ??
            double virstualDiscount = CalculateDiscountByVirtualCapacity(customerInfo, nerkh.PartialConsumption, nerkh.Duration, boodjeAmounts.Summation);

            return virstualDiscount > 0 ? virstualDiscount : boodjeAmounts.Allowed;
        }
    }
}
