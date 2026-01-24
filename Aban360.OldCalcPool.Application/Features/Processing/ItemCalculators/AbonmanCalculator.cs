using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffDateOperations;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Application.Features.Processing.Helpers;

namespace Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators
{
    internal interface IAbonmanCalculator
    {
        TariffItemResult CalculateAb(CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, string currentDateJalali, ConsumptionPartialInfo consumptionPartialInfo, out double before1404_12_02, out double before1404);
        TariffItemResult CalculateDiscount(int usageId, int branchTypeId, double abonmanAmount, double bahaDiscountAmount, bool isSpecial, ConsumptionInfo consumptionInfo, CustomerInfoOutputDto customerInfo, ConsumptionPartialInfo consumptionPartialInfo, double abonAllowed, TariffItemResult abonmanResult, double before1403_12_02, double before1404);
    }

    internal sealed class AbonmanCalculator : IAbonmanCalculator
    {
        const int olgooBefore1404=14;
        const int monthDays = 30;
        const string date_begin = "1330/01/01";
        //const string date1400_01_01 = "1400/01/01";
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

        public TariffItemResult CalculateAb(CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, string currentDateJalali, ConsumptionPartialInfo consumptionPartialInfo, out double before1403_12_02, out double before1404)
        {
            before1403_12_02 = 0;
            before1404 = 0;
            if (!IsConstruction(customerInfo.BranchType) && IsTankerSale(customerInfo.UsageId))
            {
                return new TariffItemResult();
            }
            if(IsTankerSale(customerInfo.UsageId))
            {
                return new TariffItemResult();
            }

            double abonAbAmount = 0;
            double durationTo1403_12_01 = 0, durationTo1403_12_30 = 0, durationTo1404_02_14Or31 = 0, duration1404_09_09 = 0, duration1404_12_29=0;

            durationTo1403_12_01 = PartTime(date_begin, date1403_12_01, meterInfo.PreviousDateJalali, currentDateJalali, new { customerInfo.BillId, customerInfo.ZoneId, customerInfo.UsageId });
            durationTo1403_12_30 = PartTime(date1403_12_01, date1403_12_30, meterInfo.PreviousDateJalali, currentDateJalali, new { customerInfo.BillId, customerInfo.ZoneId, customerInfo.UsageId });

            if (IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId))
            {
                durationTo1404_02_14Or31 = PartTime(date1403_12_30, date1404_02_14, meterInfo.PreviousDateJalali, currentDateJalali, new { customerInfo.BillId, customerInfo.ZoneId, customerInfo.UsageId });
            }
            else
            {
                durationTo1404_02_14Or31 = PartTime(date1403_12_30, date1404_02_31, meterInfo.PreviousDateJalali, currentDateJalali, new { customerInfo.BillId,  customerInfo.ZoneId, customerInfo.UsageId });
            }

            if (IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId))
            {
                duration1404_09_09 = PartTime(date1404_02_14, date1404_09_09, meterInfo.PreviousDateJalali, currentDateJalali, new { customerInfo.BillId,  customerInfo.ZoneId, customerInfo.UsageId });
            }
            else
            {
                duration1404_09_09 = PartTime(date1404_02_31, date1404_09_09, meterInfo.PreviousDateJalali, currentDateJalali, new { customerInfo.BillId, customerInfo.ZoneId, customerInfo.UsageId });
            }
            duration1404_12_29 = PartTime(date1404_09_09, date1404_12_29, meterInfo.PreviousDateJalali, currentDateJalali, new { customerInfo.BillId, customerInfo.ZoneId, customerInfo.UsageId });

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
                (((amountTo1403_12_01 / monthDays) * durationTo1403_12_01) +
                ((amountTo1403_12_30 / monthDays) * durationTo1403_12_30) +
                ((amountTo404_02_31 / monthDays) * durationTo1404_02_14Or31) +
                ((amountTo1404_09_09 / monthDays) * duration1404_09_09) +
                ((amountTo1404_12_29 / monthDays) * duration1404_12_29));

            if (abonAbAmount < 0)
            {
                abonAbAmount = 0;
            }

            if (IsConstruction(customerInfo.BranchType) || IsUsageConstructor(customerInfo.UsageId))
            {
                abonAbAmount *= 2;
            }
            before1403_12_02 = sumUnit * (amountTo1403_12_01 / monthDays * durationTo1403_12_01);
            before1404 = (before1403_12_02) + sumUnit * (amountTo1403_12_30 / monthDays * durationTo1403_12_30);
            return new TariffItemResult(consumptionPartialInfo.AllowedRatio * abonAbAmount, consumptionPartialInfo.DisallwedRatio * abonAbAmount);
        }

        public TariffItemResult CalculateDiscount(int usageId, int branchTypeId, double abonmanAmount, double bahaDiscountAmount, bool isSpecial, ConsumptionInfo consumptionInfo, CustomerInfoOutputDto customerInfo, ConsumptionPartialInfo consumptionPartialInfo, double abonAllowed, TariffItemResult abonmanResult, double before1403_12_02, double before1404)
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

            if (IsUnderSocialService(customerInfo.BranchType) &&
             IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
             date1403_12_01.MoreOrEq(consumptionPartialInfo.EndDateJalali) &&
             (consumptionPartialInfo.DisallowedConsumtion >0))
            {
                return new TariffItemResult();
            }

            if (IsUnderSocialService(branchTypeId) &&
                IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
                consumptionPartialInfo.EndDateJalali.More(date1403_12_01) &&
                consumptionPartialInfo.DisallowedConsumtion <= 0)
            {
                return new TariffItemResult(abonmanAmount);
            }
            //todo abon 
            if (IsUnderSocialService(branchTypeId) &&
               IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
               date1403_12_01.MoreOrEq(consumptionPartialInfo.StartDateJalali) &&
               consumptionPartialInfo.EndDateJalali.More(date1403_12_01) &&
               consumptionPartialInfo.DisallowedConsumtion > 0 &&
               consumptionInfo.MonthlyAverageConsumption<olgooBefore1404)
            {
                return new TariffItemResult(abonmanAmount);
            }

            if (IsUnderSocialService(branchTypeId) &&
               IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
               consumptionPartialInfo.EndDateJalali.More(date1403_12_01) &&
               consumptionPartialInfo.DisallowedConsumtion > 0)
            {                
                double abonTmp = abonmanAmount - before1403_12_02;
                return abonTmp > 0 ? new TariffItemResult(abonTmp) : new TariffItemResult();
            }
            if (IsMullah(customerInfo.BranchType) && abonmanResult.Disallowed > 0)
            {
                return new TariffItemResult();
            }

            if (IsReligiousWithCharity(usageId))
            {
                double abonmanPerUnit = abonmanAmount / customerInfo.UnitAll;
                double abonmanCommercial = abonmanPerUnit * customerInfo.CommertialUnit;
                double nonCommercial = abonmanAmount - abonmanCommercial;
                return consumptionInfo.MonthlyAverageConsumption <= customerInfo.ContractualCapacity ?
                    new TariffItemResult(nonCommercial) : new TariffItemResult();
            }
            if(IsQuranIn1403ContinuesNext(usageId,consumptionPartialInfo.StartDateJalali, consumptionPartialInfo.EndDateJalali))
            {
                double abonmanPerUnit = abonmanAmount / customerInfo.UnitAll;
                double abonmanCommercial = abonmanPerUnit * customerInfo.CommertialUnit;
                double nonCommercial = abonmanAmount - abonmanCommercial;
                return consumptionInfo.MonthlyAverageConsumption <= customerInfo.ContractualCapacity ?
                    new TariffItemResult(nonCommercial - before1404) : new TariffItemResult();
            }
            if(IsQuranBefore1404_01_01(usageId, consumptionPartialInfo.EndDateJalali))
            {
                return new TariffItemResult();
            }
            if(IsQuranAfter1404_01_01(usageId, consumptionPartialInfo.StartDateJalali))
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
