using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffStringChecker;

namespace Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators
{
    internal interface IAvarezCalculator
    {
        TariffItemResult Calculate(ConsumptionPartialInfo consumptionPartialInfo, CustomerInfoOutputDto customerInfo, double monthlyConsumption);
        TariffItemResult CalculateDiscount();
    }

    internal sealed class AvarezCalculator : IAvarezCalculator
    {
        const int _25000 = 25000;
        const int _20000 = 20000;
        const int _2000 = 2000;
        public TariffItemResult Calculate(ConsumptionPartialInfo consumptionPartialInfo, CustomerInfoOutputDto customerInfo, double monthlyConsumption)
        {
            if (IsIndustrialAfter1404(consumptionPartialInfo.EndDateJalali, customerInfo.UsageId, customerInfo.BranchType))
            {
                return monthlyConsumption <= _25000 ? 
                    new TariffItemResult(consumptionPartialInfo.Consumption * _2000) :
                    new TariffItemResult(consumptionPartialInfo.Consumption * _20000);
            }
            return new TariffItemResult();
        }
        private bool IsIndustrialAfter1404(string date2, int usageId, int branchTypeId)
        {
            return
                   IsMoreThan1404_01_01(date2) &&
                   IsIndustrial(usageId) &&
                   IsSpecialIndustrial(branchTypeId);
        }

        public TariffItemResult CalculateDiscount()
        {
            return new TariffItemResult();
        }
    }
}
