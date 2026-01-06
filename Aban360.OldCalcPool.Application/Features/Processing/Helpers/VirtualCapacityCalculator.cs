using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;

namespace Aban360.OldCalcPool.Application.Features.Processing.Helpers
{
    internal static class VirtualCapacityCalculator
    {
        const string date1403_12_30 = "1403/12/30";
        const int monthDays = 30;
        internal static double CalculateDiscountByVirtualCapacity(CustomerInfoOutputDto customerInfo, double partialConsumption, int duration, double amount, ConsumptionPartialInfo consumptionPartialInfo)
        {
            if(date1403_12_30.MoreOrEq(consumptionPartialInfo.EndDateJalali))
            {
                return 0;
            }
            if(!IsSpecialEducation(customerInfo.UsageId, customerInfo.IsSpecial))
            {
                return 0;
            }
            int virtualCapacity = GetVirtualCapacity(customerInfo, duration);
            if (virtualCapacity > 0)
            {
                return partialConsumption <= virtualCapacity ? amount : 0;
            }
            return 0;
        }
        private static int GetVirtualCapacity(CustomerInfoOutputDto customerInfo, int duration)
        {
            float multiplier = GetVirtualMultiplier(customerInfo.VirtualCategoryId);
            if (IsSpecialEducation(customerInfo.UsageId, customerInfo.IsSpecial) && multiplier > 0)
            {
                double partialCapacity = (double)customerInfo.ContractualCapacity / monthDays * duration;
                return Convert.ToInt32(Math.Round(partialCapacity * multiplier, 1));
            }
            return 0;
        }
        private static float GetVirtualMultiplier(int virtualCategoryId)
        {
            switch (virtualCategoryId)
            {
                case 0:
                    return 0.9f / 0.45f;
                case 1:
                    return 0.9f / 0.45f;
                case 2:
                    return 1.05f / 1.2f;
                case 3:
                    return 3.9f / 4.5f;
                case 4:
                    return 1.05f / 1.2f;
                case 5:
                    return 3.9f / 4.5f;
                case 6:
                    return 0.9f / 0.45f;
                case 7:
                    return 3.9f / 3.6f;
                case 8:
                    return 2.1f / 2.4f;
                default:
                    return 0;
            }
        }
    }
}
