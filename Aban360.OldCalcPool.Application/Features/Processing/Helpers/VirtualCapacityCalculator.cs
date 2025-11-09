using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;

namespace Aban360.OldCalcPool.Application.Features.Processing.Helpers
{
    internal static class VirtualCapacityCalculator
    {
        const int monthDays = 30;
        internal static double CalculateDiscountByVirtualCapacity(CustomerInfoOutputDto customerInfo, double partialConsumption, int duration, double amount)
        {
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
                    return 0.45f / 0.9f;
                case 1:
                    return 0.45f / 0.9f;
                case 2:
                    return 1.2f / 1.05f;
                case 3:
                    return 4.5f / 3.9f;
                case 4:
                    return 1.2f / 1.05f;
                case 5:
                    return 4.5f / 3.9f;
                case 6:
                    return 0.45f / 0.9f;
                case 7:
                    return 3.6f / 3.9f;
                case 8:
                    return 2.4f / 2.1f;
                default:
                    return 0;
            }
        }
    }
}
