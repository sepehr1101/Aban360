using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffStringChecker;

namespace Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators
{
    internal interface IAvarezCalculator
    {
        double Calculate(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, double monthlyConsumption);
    }

    internal sealed class AvarezCalculator : IAvarezCalculator
    {
        const int _25000 = 25000;
        const int _20000 = 20000;
        const int _2000 = 2000;
        public double Calculate(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, double monthlyConsumption)
        {
            if (IsIndustrialAfter1404(nerkh.Date2, customerInfo.UsageId, customerInfo.BranchType))
            {
                return IsMonthlyConsumptionBelow25000(monthlyConsumption) ? Multiply(nerkh.PartialConsumption, _2000) : Multiply(nerkh.PartialConsumption, _20000);
            }
            return 0;
        }
        private bool IsIndustrialAfter1404(string date2, int usageId, int branchTypeId)
        {
            return
                   IsMoreThan1404_01_01(date2) &&
                   IsIndustrial(usageId) &&
                   IsSpecialIndustrial(branchTypeId);
        }
        private bool IsMonthlyConsumptionBelow25000(double monthlyConsumption)
        {
            return monthlyConsumption <= _25000;
        }
        private double Multiply(double partialConsumption, int _value)
        {
            return partialConsumption * _value;
        }
    }
}
