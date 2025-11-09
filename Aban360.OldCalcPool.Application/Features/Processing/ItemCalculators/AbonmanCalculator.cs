using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffDateOperations;

namespace Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators
{
    internal interface IAbonmanCalculator
    {
        double CalculateAbAb(CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, string currentDateJalali);
        double CalculateDiscount(int usageId, double abonmanAmount, double bahaDiscountAmount, bool isSpecial);
    }

    internal sealed class AbonmanCalculator : IAbonmanCalculator
    {
        const int monthDays = 30;
        const string date1400_01_01 = "1400/01/01";
        const string date1403_12_01 = "1403/12/01";
        const string date1403_12_30 = "1403/12/30";
        const string date1404_02_14 = "1404/02/14";
        const string date1404_02_31 = "1404/02/31";
        const string date1404_12_29 = "1404/12/29";
        public double CalculateAbAb(CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, string currentDateJalali)
        {
            if (!IsConstruction(customerInfo.BranchType) && IsTankerSale(customerInfo.UsageId))
            {
                return 0;
            }

            double abonAbAmount = 0;//, abonAbDiscount = 0;
            double zabon_1 = 0, zabon_2 = 0, zabon_3 = 0, zabon_4 = 0;

            zabon_1 = PartTime(date1400_01_01, date1403_12_01, meterInfo.PreviousDateJalali, currentDateJalali, new { BillId = customerInfo.BillId, ZoneId = customerInfo.ZoneId, UsageId = customerInfo.UsageId });
            zabon_2 = PartTime(date1403_12_01, date1403_12_30, meterInfo.PreviousDateJalali, currentDateJalali, new { BillId = customerInfo.BillId, ZoneId = customerInfo.ZoneId, UsageId = customerInfo.UsageId });

            if (IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId))
            {
                zabon_3 = PartTime(date1403_12_30, date1404_02_14, meterInfo.PreviousDateJalali, currentDateJalali, new { BillId = customerInfo.BillId, ZoneId = customerInfo.ZoneId, UsageId = customerInfo.UsageId });
            }
            else
            {
                zabon_3 = PartTime(date1403_12_30, date1404_02_31, meterInfo.PreviousDateJalali, currentDateJalali, new { BillId = customerInfo.BillId, ZoneId = customerInfo.ZoneId, UsageId = customerInfo.UsageId });
            }

            if (IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId))
            {
                zabon_4 = PartTime(date1404_02_14, date1404_12_29, meterInfo.PreviousDateJalali, currentDateJalali, new { BillId = customerInfo.BillId, ZoneId = customerInfo.ZoneId, UsageId = customerInfo.UsageId });
            }
            else
            {
                zabon_4 = PartTime(date1404_02_31, date1404_12_29, meterInfo.PreviousDateJalali, currentDateJalali, new { BillId = customerInfo.BillId, ZoneId = customerInfo.ZoneId, UsageId = customerInfo.UsageId });
            }

            zabon_1 = Math.Max(zabon_1, 0);
            zabon_2 = Math.Max(zabon_2, 0);
            zabon_3 = Math.Max(zabon_3, 0);
            zabon_4 = Math.Max(zabon_4, 0);

            int sumUnit = customerInfo.OtherUnit + customerInfo.DomesticUnit + customerInfo.CommertialUnit;

            if (IsVillageCollectorMeter(customerInfo.UsageId))
            {
                sumUnit = 1;
            }

            if (sumUnit <= 0)
            {
                sumUnit = 1;
            }

            abonAbAmount = (((10000.0 / monthDays) * zabon_1) + ((35000.0 / monthDays) * zabon_2) + ((45500.0 / monthDays) * zabon_3) + ((58500.0 / monthDays) * zabon_4)) * sumUnit;

            if (abonAbAmount < 0)
            {
                abonAbAmount = 0;
            }

            if (IsConstruction(customerInfo.BranchType) || IsUsageConstructor(customerInfo.UsageId))
            {
                abonAbAmount *= 2;
            }

            return abonAbAmount;
        }

        public double CalculateDiscount(int usageId, double abonmanAmount, double bahaDiscountAmount, bool isSpecial)
        {
            if (IsSpecialEducation(usageId, isSpecial))
            {
                return 0;
            }
            return bahaDiscountAmount > 0 && !IsReligiousWithCharity(usageId) ? abonmanAmount : 0;
        }
    }
}
