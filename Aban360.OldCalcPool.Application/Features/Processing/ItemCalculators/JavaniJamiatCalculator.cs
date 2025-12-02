using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;

namespace Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators
{
    internal interface IJavaniJamiatCalculator
    {
        TariffItemResult Calculate(ConsumptionPartialInfo consumptionPartialInfo, CustomerInfoOutputDto customerInfo, double abBahaAmount, double monthlyConsumption, int olgoo);
        TariffItemResult CalculateDiscount();
    }

    internal sealed class JavaniJamiatCalculator : IJavaniJamiatCalculator
    {
        public TariffItemResult Calculate(ConsumptionPartialInfo consumptionPartialInfo, CustomerInfoOutputDto customerInfo, double abBahaAmount, double monthlyConsumption, int olgoo)
        {
            //L 2608
            if (IsUsageConstructor(customerInfo.UsageId))
            {
                return new TariffItemResult();
            }
            if (IsConstruction(customerInfo.BranchType))
            {
                return new TariffItemResult();
            }
            if (abBahaAmount == 0)
            {
                return new TariffItemResult();
            }

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
                    return new TariffItemResult();
                }

                if (villageCode > 0 &&
                    monthlyConsumption > olgoo &&
                    domesticUnit > 1 &&
                    RuralButIsMetro(customerInfo.ZoneId, villageCode))
                {
                    return new TariffItemResult(baseAmount * consumptionPartialInfo.Consumption);
                }
                else
                {
                    return new TariffItemResult();
                }
            }
            //L 2642
            if (monthlyConsumption > olgoo &&
                domesticUnit >= 1 &&
                (IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId)))
            {
                return new TariffItemResult(baseAmount * consumptionPartialInfo.Consumption);
            }
            if (!IsDomesticWithoutUnspecified(customerInfo.UsageId) && !IsGardenAndResidence(customerInfo.UsageId))
            {
                if (monthlyConsumption > customerInfo.ContractualCapacity)
                {
                    return new TariffItemResult(baseAmount * consumptionPartialInfo.Consumption);
                }
            }
            return new TariffItemResult();
        }
        public TariffItemResult CalculateDiscount()
        {
            return new TariffItemResult();
        }
    }
}
