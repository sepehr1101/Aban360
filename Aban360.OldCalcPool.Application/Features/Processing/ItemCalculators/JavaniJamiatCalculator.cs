using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;

namespace Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators
{
    internal interface IJavaniJamiatCalculator
    {
        double Calculate(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, double abBahaAmount, double monthlyConsumption, int olgoo);
    }

    internal sealed class JavaniJamiatCalculator : IJavaniJamiatCalculator
    {
        public double Calculate(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, double abBahaAmount, double monthlyConsumption, int olgoo)
        {
            //L 2608
            if (IsUsageConstructor(customerInfo.UsageId))
            {
                return 0;
            }
            if (IsConstruction(customerInfo.BranchType))
            {
                return 0;
            }
            if (abBahaAmount == 0)
            {
                return 0;
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
                    return 0;
                }

                if (villageCode > 0 &&
                    monthlyConsumption > olgoo &&
                    domesticUnit > 1 &&
                    RuralButIsMetro(customerInfo.ZoneId, villageCode))
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

    }
}
