using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffDateOperations;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Application.Features.Processing.Helpers;

namespace Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators
{
    internal interface IAbonmanCalculator
    {
        TariffItemResult CalculateAb(CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, string currentDateJalali, ConsumptionPartialInfo consumptionPartialInfo);
        TariffItemResult CalculateDiscount(int usageId, int branchTypeId, double abonmanAmount, double bahaDiscountAmount, bool isSpecial, ConsumptionInfo consumptionInfo, CustomerInfoOutputDto customerInfo, ConsumptionPartialInfo consumptionPartialInfo, double abonAllowed);
    }

    internal sealed class AbonmanCalculator : IAbonmanCalculator
    {
        const int monthDays = 30;
        const string date_begin = "1330/01/01";
        const string date1400_01_01 = "1400/01/01";
        const string date1403_12_01 = "1403/12/01";
        const string date1403_12_30 = "1403/12/30";
        const string date1404_02_14 = "1404/02/14";
        const string date1404_02_31 = "1404/02/31";
        const string date1404_09_09 = "1404/09/09";
        const string date1404_12_29 = "1404/12/29";

        const double amountTo1403_12_01 = 10000.0;
        const double amountTo1403_12_30 = 35000.0;
        const double amountTo404_02_31 = 45500.0;
        const double amountTo1404_09_09 = 58500.0;
        const double amountTo1404_12_29 = 71500.0;

        public TariffItemResult CalculateAb(CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, string currentDateJalali, ConsumptionPartialInfo consumptionPartialInfo)
        {
            if (!IsConstruction(customerInfo.BranchType) && IsTankerSale(customerInfo.UsageId))
            {
                return new TariffItemResult();
            }
            if(IsTankerSale(customerInfo.UsageId))
            {
                return new TariffItemResult();
            }

            double abonAbAmount = 0;
            double durationPart_1 = 0, durationPart_2 = 0, durationPart_3 = 0, durationPart_4 = 0, durationPart_5=0;

            durationPart_1 = PartTime(date_begin, date1403_12_01, meterInfo.PreviousDateJalali, currentDateJalali, new { customerInfo.BillId, customerInfo.ZoneId, customerInfo.UsageId });
            durationPart_2 = PartTime(date1403_12_01, date1403_12_30, meterInfo.PreviousDateJalali, currentDateJalali, new { customerInfo.BillId, customerInfo.ZoneId, customerInfo.UsageId });

            if (IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId))
            {
                durationPart_3 = PartTime(date1403_12_30, date1404_02_14, meterInfo.PreviousDateJalali, currentDateJalali, new { customerInfo.BillId, customerInfo.ZoneId, customerInfo.UsageId });
            }
            else
            {
                durationPart_3 = PartTime(date1403_12_30, date1404_02_31, meterInfo.PreviousDateJalali, currentDateJalali, new { customerInfo.BillId,  customerInfo.ZoneId, customerInfo.UsageId });
            }

            if (IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId))
            {
                durationPart_4 = PartTime(date1404_02_14, date1404_09_09, meterInfo.PreviousDateJalali, currentDateJalali, new { customerInfo.BillId,  customerInfo.ZoneId, customerInfo.UsageId });
            }
            else
            {
                durationPart_4 = PartTime(date1404_02_31, date1404_09_09, meterInfo.PreviousDateJalali, currentDateJalali, new { customerInfo.BillId, customerInfo.ZoneId, customerInfo.UsageId });
            }
            durationPart_5 = PartTime(date1404_09_09, date1404_12_29, meterInfo.PreviousDateJalali, currentDateJalali, new { customerInfo.BillId, customerInfo.ZoneId, customerInfo.UsageId });

            int sumUnit = customerInfo.OtherUnit + customerInfo.DomesticUnit + customerInfo.CommertialUnit;

            if (IsVillageCollectorMeter(customerInfo.UsageId))
            {
                sumUnit = 1;
            }

            if (sumUnit <= 0)
            {
                sumUnit = 1;
            }

            abonAbAmount = sumUnit*
                (((amountTo1403_12_01 / monthDays) * durationPart_1) +
                ((amountTo1403_12_30 / monthDays) * durationPart_2) +
                ((amountTo404_02_31 / monthDays) * durationPart_3) +
                ((amountTo1404_09_09 / monthDays) * durationPart_4) +
                ((amountTo1404_12_29 / monthDays) * durationPart_5));

            if (abonAbAmount < 0)
            {
                abonAbAmount = 0;
            }

            if (IsConstruction(customerInfo.BranchType) || IsUsageConstructor(customerInfo.UsageId))
            {
                abonAbAmount *= 2;
            }
            return new TariffItemResult(consumptionPartialInfo.AllowedRatio * abonAbAmount, consumptionPartialInfo.DisallwedRatio * abonAbAmount);
        }

        public TariffItemResult CalculateDiscount(int usageId, int branchTypeId, double abonmanAmount, double bahaDiscountAmount, bool isSpecial, ConsumptionInfo consumptionInfo, CustomerInfoOutputDto customerInfo, ConsumptionPartialInfo consumptionPartialInfo, double abonAllowed)
        {
            if (IsSpecialEducation(usageId, isSpecial))
            {
                return new TariffItemResult();
            }
            if (IsConstruction(branchTypeId))
            {
                return new TariffItemResult();
            }
            if (IsUsageConstructor(usageId))
            {
                return new TariffItemResult();
            }
            if (IsUnderSocialService(branchTypeId) &&
                date1403_12_01.MoreOrEq(consumptionPartialInfo.EndDateJalali))
            {
                return new TariffItemResult(abonAllowed);
            }            

            if (IsHandoverDiscount(customerInfo.BranchType) &&
              IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
              date1403_12_01.MoreOrEq(consumptionPartialInfo.EndDateJalali))
            {
                return new TariffItemResult(abonAllowed);
            }

            if (IsUnderSocialService(branchTypeId))
            {
                return new TariffItemResult(abonmanAmount);
            }

            if (IsReligiousWithCharity(usageId))
            {
                double abonmanPerUnit = abonmanAmount / customerInfo.UnitAll;
                double abonmanCommercial = abonmanPerUnit * customerInfo.CommertialUnit;
                double nonCommercial = abonmanAmount - abonmanCommercial;
                return consumptionInfo.MonthlyAverageConsumption <= customerInfo.ContractualCapacity ?
                    new TariffItemResult(nonCommercial) : new TariffItemResult();
            }
            return bahaDiscountAmount > 0 && !IsReligiousWithCharity(usageId) ?
            new TariffItemResult(abonmanAmount) : new TariffItemResult();
        }
    }
}
