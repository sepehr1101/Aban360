using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.CalculationPool.Application.Features.Sale.Helpers
{
    internal static class SaleRuleChecker
    {
        internal static bool IsUnspecified(int usageId) => usageId == (int)UsageEnum.Unspecified;
        internal static bool IsDomestic(int usageId) => usageId == (int)UsageEnum.Domestic;
        internal static bool IsCommercial(int usageId) => usageId == (int)UsageEnum.Commercial;
        internal static bool IsDomesticCommercial(int usageId) => usageId == (int)UsageEnum.DomesticCommercial;
        internal static bool IsDomesticOrCommercial(int usageId)
        {
            int[] domestic = { (int)UsageEnum.Domestic, (int)UsageEnum.DomesticCommercial };
            return domestic.Contains(usageId);
        }
        internal static bool IsDomesticOrResidence(int usageId)
        {
            int[] domestic = { (int)UsageEnum.Domestic, (int)UsageEnum.DomesticCommercial, (int)UsageEnum.Residence };
            return domestic.Contains(usageId);
        }

    }
}
