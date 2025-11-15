using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using System.Runtime.InteropServices;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffStringChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.VirtualCapacityCalculator;

namespace Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators
{
    internal interface IAbBahaCalculator
    {
        CalculateAbBahaOutputDto Calculate(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, ZaribGetDto zarib, AbAzadFormulaDto abAzad8And39, string currentDateJalali, bool isVillageCalculation, double monthlyConsumption, int _olgoo, [Optional] int? c, [Optional] IEnumerable<int> tagIds);
        double CalculateDiscount(ZaribGetDto zarib, bool isVillageCalculation, double monthlyConsumption, CustomerInfoOutputDto customerInfo, NerkhGetDto nerkh, int olgoo, CalculateAbBahaOutputDto calculateAbBahaOutputDto, bool isFull, int finalDomesticUnit);
    }

    internal sealed class AbBahaCalculator : BaseExpressionCalculator, IAbBahaCalculator
    {
        const int monthDays = 30;
        const int c_1404 = 90000;
        public CalculateAbBahaOutputDto Calculate(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, ZaribGetDto zarib, AbAzadFormulaDto abAzad8And39, string currentDateJalali, bool isVillageCalculation, double monthlyConsumption, int _olgoo, [Optional] int? c, [Optional] IEnumerable<int> tagIds)
        {
            double abBahaAmount = 0, oldAbBahaAmount = 0, abBahaFromExpression = 0, oldAbBahaZarib = 1.15;
            double duration = nerkh.Duration;
            abBahaFromExpression = CalcFormulaByRate(nerkh.Vaj, monthlyConsumption, _olgoo, c, tagIds);
            decimal multiplierAbBaha = GetMultiplier(zarib, _olgoo, IsDomesticCategory(customerInfo.UsageId), isVillageCalculation, monthlyConsumption, customerInfo.BranchType);
            (double, double) abBahaValues = (0, 0);
            (long, long) _2Amount = (0, 0);

            if (CheckZero(duration, monthlyConsumption, nerkh.Vaj))
            {
                return new CalculateAbBahaOutputDto(0, (0, 0),0,0,0);
            }

            if (IsGardenOrDweltyAfter1400_12_24OrIsDomestic(customerInfo, nerkh) &&
                !IsReligious(customerInfo.UsageId) &&
                !IsConstruction(customerInfo.BranchType))
            {
                abBahaFromExpression = CalcFormulaByRate(nerkh.Vaj, monthlyConsumption, _olgoo, c, tagIds);
                abBahaAmount = abBahaFromExpression * nerkh.PartialConsumption;

                if (IsLessThan1403_09_13AndOvajNotZero(nerkh))
                {
                    double oldAbBahaFromExpression = CalcFormulaByRate(nerkh.OVaj, monthlyConsumption, _olgoo, c, tagIds);
                    oldAbBahaAmount = nerkh.PartialConsumption * oldAbBahaFromExpression * oldAbBahaZarib;
                }

                if (IsLessThan1403_09_13(nerkh.Date2) &&
                    monthlyConsumption <= _olgoo &&
                    abBahaAmount > oldAbBahaAmount &&
                    IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
                    !IsConstruction(customerInfo.BranchType))
                {
                    //?????
                    abBahaAmount = oldAbBahaAmount;
                }
            }
            else
            {
                // foxpro:1139
                if (HasCapacityAndNotConstruction(customerInfo) ||
                    IsReligious(customerInfo.UsageId))
                {
                    double contractualCapacityInDuration = (double)customerInfo.ContractualCapacity / monthDays * duration;

                    if (IsCharitySchoolOrConsumptionGtCapacity(nerkh, customerInfo, contractualCapacityInDuration))
                    {
                        double allowedPartialConsumption = Math.Min(contractualCapacityInDuration, nerkh.PartialConsumption);
                        double disallowedPartialConsumption = (nerkh.PartialConsumption - allowedPartialConsumption)>0? nerkh.PartialConsumption - allowedPartialConsumption:0;                        

                        if (nerkh.PartialConsumption < contractualCapacityInDuration ||
                            IsReligiousAndZeroCapacity(customerInfo))
                        {
                            disallowedPartialConsumption = 0;
                            allowedPartialConsumption = nerkh.PartialConsumption;
                        }//L 1153
                        // از این پارامتر بابت محاسبه تخفیف  استفاده خواهد شد
                        _2Amount = Get2Amount(nerkh, customerInfo, abAzad8And39, abBahaFromExpression, _olgoo, monthlyConsumption,c,tagIds);

                        abBahaValues.Item1 = _2Amount.Item1 * allowedPartialConsumption;
                        abBahaValues.Item2 = _2Amount.Item2 * disallowedPartialConsumption;
                        abBahaAmount = abBahaValues.Item1 + abBahaValues.Item2;
                    }
                    else
                    {
                        abBahaAmount = nerkh.PartialConsumption * abBahaFromExpression;
                    }
                }
                else
                {
                    if (!IsConstruction(customerInfo.BranchType))
                    {
                        abBahaAmount = nerkh.PartialConsumption * abBahaFromExpression;
                    }
                }
            }
            if (IsConstruction(customerInfo.BranchType))
            {
                abBahaAmount = CalcFormulaByRate(abAzad8And39.Formula, monthlyConsumption, _olgoo, c, tagIds) * nerkh.PartialConsumption;
            }
            //L 1553
            //L 1558

            if (IsVillageDomesticNotConstruction(customerInfo) &&
                !IsRuralButIsMetro(customerInfo) &&
                abBahaAmount != 0)
            {//L 1578
                float multiplier = IsLessThan1403_09_13OrMonthlyBelowEqOlgoo(nerkh, monthlyConsumption, _olgoo) ? 0.5f : 0.65f;
                (abBahaAmount, oldAbBahaAmount, isVillageCalculation) = MultiplyCalculation(abBahaAmount, oldAbBahaAmount, multiplier);
            }

            //L 1600 approximately
            if (IsDolatabadOrHabibabadAndDomesticAndNotConstruction(customerInfo, abBahaAmount))
            {//L 1604
                float multiplier = IsLessThan1403_09_13OrMonthlyBelowEqOlgoo(nerkh, monthlyConsumption, _olgoo) ? 0.5f : 0.65f;
                (abBahaAmount, oldAbBahaAmount, isVillageCalculation) = MultiplyCalculation(abBahaAmount, oldAbBahaAmount, multiplier);
            }//foxpro:1620

            abBahaAmount = abBahaAmount * (double)multiplierAbBaha;            
            oldAbBahaAmount = oldAbBahaAmount * (double)multiplierAbBaha;// foxpro:1755
            abBahaValues = CheckAbBahaValues(abBahaAmount, abBahaValues);
            bool isDomestic = IsDomestic(customerInfo.UsageId);
            double abBaha1 = _2Amount.Item1 > 0 ? abBahaValues.Item1 * (double)multiplierAbBaha : 0;
            double abBaha2 = _2Amount.Item2 > 0 ? abBahaValues.Item2 * (double)multiplierAbBaha : 0;

            return new CalculateAbBahaOutputDto(abBahaAmount, abBahaValues, abBaha1, abBaha2, (double)multiplierAbBaha);
        }

        public double CalculateDiscount(ZaribGetDto zarib, bool isVillageCalculation, double monthlyConsumption, CustomerInfoOutputDto customerInfo, NerkhGetDto nerkh, int olgoo, CalculateAbBahaOutputDto calculateAbBahaOutputDto, bool isFull, int finalDomesticUnit)
        {
            if (calculateAbBahaOutputDto.AbBahaAmount == 0)
            {
                return 0;
            }
            decimal multiplier = GetMultiplier(zarib, olgoo, IsDomesticCategory(customerInfo.UsageId), isVillageCalculation, monthlyConsumption, customerInfo.BranchType);
            double partialOlgoo = IsDomestic(customerInfo.UsageId) ?
               (double)finalDomesticUnit * olgoo / monthDays * nerkh.Duration :
               (double)customerInfo.ContractualCapacity / monthDays * nerkh.Duration;

            if (IsHandoverDiscount(customerInfo.BranchType) &&
                IsDomesticWithoutUnspecified(customerInfo.UsageId))
            {
                if (isFull)
                {
                    return calculateAbBahaOutputDto.AbBahaAmount;
                }
                double mullahMultiplier = IsMullah(customerInfo.BranchType) ? 0.5 : 1;
                if (nerkh.PartialConsumption <= olgoo)// در صورتی که مصرف زیر الگو بود کامل معاف میشود
                {
                    return calculateAbBahaOutputDto.AbBahaAmount * mullahMultiplier;
                }
                else//در صورتی که بالای الگو بود بخش زیر الگو معاف و بالای الگو اخذ شود
                {
                    //long partialAmount = (long)(partialOlgoo / nerkh.PartialConsumption * amount);
                    //return partialAmount;
                    return (long)(calculateAbBahaOutputDto.AbBaha1 > 0 ? calculateAbBahaOutputDto.AbBaha1 : c_1404 * 0.01 * partialOlgoo * olgoo * (double)multiplier) * mullahMultiplier;
                }
            }
            //if (IsMullah(customerInfo.BranchType))
            //{
            //    double allowedPartialConsumption = Math.Min(partialOlgoo, nerkh.PartialConsumption);
            //    double disallowedPartialConsumption = (nerkh.PartialConsumption - allowedPartialConsumption) > 0 ? nerkh.PartialConsumption - allowedPartialConsumption : 0;
            //    return (long)(allowedPartialConsumption * 0.5 + disallowedPartialConsumption * 0.35) * 0.5;
            //}
            if (IsReligiousWithCharity(customerInfo.UsageId))//TODO: error golzar
            {
                //در صورتی که بالای الگو بود بخش زیر الگو معاف و بالای الگو اخذ شود
                //C*0.1                
                return (long)(calculateAbBahaOutputDto.AbBaha1 > 0 ? calculateAbBahaOutputDto.AbBaha1 : c_1404 * 0.1 * partialOlgoo * olgoo * (double)multiplier);
            }
            
            double virtualDiscount = CalculateDiscountByVirtualCapacity(customerInfo, nerkh.PartialConsumption, nerkh.Duration, calculateAbBahaOutputDto.AbBahaAmount);
            return virtualDiscount > 0 ? (long)virtualDiscount : 0;
        }

        private bool IsLessThan1403_09_13AndOvajNotZero(NerkhGetDto nerkh)
        {
            return !string.IsNullOrWhiteSpace(nerkh.OVaj) &&
                                nerkh.OVaj.Trim() != "0" &&
                                IsLessThan1403_09_13(nerkh.Date2);
        }

        private bool IsLessThan1403_09_13OrMonthlyBelowEqOlgoo(NerkhGetDto nerkh, double monthlyConsumption, int _olgoo)
        {
            return IsLessThan1403_09_13(nerkh.Date2) || monthlyConsumption <= _olgoo;
        }

        private bool IsDolatabadOrHabibabadAndDomesticAndNotConstruction(CustomerInfoOutputDto customerInfo, double abBahaAmount)
        {
            return IsDolatabadOrHabibabadWithConditionEshtrak(customerInfo.ZoneId, customerInfo.ReadingNumber) &&
                   IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
                   !IsConstruction(customerInfo.BranchType) &&
                   abBahaAmount != 0;
        }

        private bool IsGardenOrDweltyAfter1400_12_24OrIsDomestic(CustomerInfoOutputDto customerInfo, NerkhGetDto nerkh)
        {
            return IsGardenOrDweltyAfter1400_12_24(customerInfo.UsageId, nerkh.Date1) || IsDomestic(customerInfo.UsageId);
        }

        private bool HasCapacityAndNotConstruction(CustomerInfoOutputDto customerInfo)
        {
            return customerInfo.ContractualCapacity > 0 && !IsConstruction(customerInfo.BranchType);
        }

        private bool IsRuralButIsMetro(CustomerInfoOutputDto customerInfo)
        {
            var (hasVillageCode, villageCode) = HasVillageCode(customerInfo.VillageId);
            if (!hasVillageCode)
            {
                return false;
            }
            return RuralButIsMetro(customerInfo.ZoneId, customerInfo.ReadingNumber) ||
                   RuralButIsMetro(customerInfo.ZoneId, villageCode);
        }

        private bool IsVillageDomesticNotConstruction(CustomerInfoOutputDto customerInfo)
        {
            return IsVillage(customerInfo.ZoneId) &&
                   IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
                   !IsConstruction(customerInfo.BranchType);
        }

        private bool IsReligiousAndZeroCapacity(CustomerInfoOutputDto customerInfo)
        {
            return customerInfo.ContractualCapacity == 0 &&
                   IsReligiousWithCharity(customerInfo.UsageId);
        }

        private (double, double) CheckAbBahaValues(double abBahaAmount, (double, double) abBahaValues)
        {
            if (abBahaValues == (0, 0))
            {
                abBahaValues = (abBahaAmount, 0);
            }
            return abBahaValues;
        }

        private bool IsCharitySchoolOrConsumptionGtCapacity(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, double contractualCapacityInDuration)
        {
            return nerkh.PartialConsumption > contractualCapacityInDuration ||
                   IsCharityOrSchool(customerInfo.UsageId);
        }

        private (long, long) Get2Amount(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, AbAzadFormulaDto abAzad8And39, double abBahaFromExpression, int olgoo, double monthlyConsumption, [Optional] int? c, [Optional] IEnumerable<int> tagIds)
        {
            if (IsReligiousWithCharity(customerInfo.UsageId))
            {
                return IsConstruction(customerInfo.BranchType) ? (450000, 450000) : Get2PartAmount(nerkh.Date2);//  foxpro:1178
            }
            return GetEducationOrBathMultiplier(customerInfo.UsageId, nerkh.Date1, nerkh.Date2, customerInfo.IsSpecial, (long)CalcFormulaByRate(abAzad8And39.Formula, monthlyConsumption, olgoo, c, tagIds), abBahaFromExpression);//Azad:39
        }
        private (long, long) GetEducationOrBathMultiplier(int usageId, string nerkhDate1, string nerkhDate2, bool isSpecial, long abAzad, double abBahaFromExpression)
        {
            string date1402_04_23 = "1402/04/23";
            string date1403_06_25 = "1403/06/25";
            string date1403_09_13 = "1403/09/13";
            string date1404_02_31 = "1404/02/31";
            (long, long) _8644_8644 = (8644, 8644);
            (long, long) _4323_225000 = (4323, 225000);
            (long, long) _4323_350000 = (4323, 350000);
            (long, long) _7000_350000 = (7000, 350000);
            (long, long) _9000_450000 = (9000, 450000);
            //start line 1228

            if (IsEducationOrBath(usageId))
            {
                if (LessThanEq(nerkhDate2, date1402_04_23))
                {
                    return usageId == 11 ? _8644_8644 : _4323_225000;
                }
                if (IsGtFromLqTo(nerkhDate2, date1402_04_23, date1403_06_25))
                {
                    return usageId == 11 ? _8644_8644 : _4323_225000;
                }
                else if (IsGtFromLqTo(nerkhDate2, date1403_06_25, date1403_09_13))
                {
                    return usageId == 11 ? _8644_8644 : _4323_350000;
                }
                else if (IsGtFromLqTo(nerkhDate2, date1403_09_13, date1404_02_31))
                {
                    return _7000_350000;
                }
                return _9000_450000;
            }
            else
            {
                return ((long)abBahaFromExpression, abAzad);
            }
        }
        private (double, double, bool) MultiplyCalculation(double abBaha, double oldAbBaha, double multiplier)
        {
            return (abBaha * multiplier, oldAbBaha * multiplier, true);
        }
        private double CalcFormulaByRate(string formula, double monthlyAverageConsumption, int olgoo, [Optional] int? c, [Optional] IEnumerable<int> tagIds)
        {
            object parameters = new { X = monthlyAverageConsumption, C = c, S = olgoo, tags = tagIds.ToArray() };
            double value = Eval<double>(formula, parameters);
            return value;
        }
        private (long, long) Get2PartAmount(string nerkhDate2)
        {
            string date1400_12_25 = "1400/12/25";
            string date1402_04_23 = "1402/04/23";
            string date1403_06_25 = "1403/06/25";
            string date1403_09_13 = "1403/09/13";
            string date1404_02_31 = "1404/02/31";

            if (StringConditionMoreThan(date1400_12_25, nerkhDate2))
            {
                return (3776, 168110);
            }
            else if (StringConditionMoreThan(nerkhDate2, date1400_12_25) && StringConditionMoreThan(date1402_04_23, nerkhDate2))
            {
                return (4040, 168110);
            }
            else if (StringConditionMoreThan(nerkhDate2, date1402_04_23) && StringConditionMoreThan(date1403_06_25, nerkhDate2))
            {
                return (4040, 168110);
            }
            else if (StringConditionMoreThan(nerkhDate2, date1403_06_25) && StringConditionMoreThan(date1403_09_13, nerkhDate2))
            {
                return (4323, 350000);
            }
            else if (StringConditionMoreThan(nerkhDate2, date1403_09_13) && StringConditionMoreThan(date1404_02_31, nerkhDate2))
            {
                return (7000, 350000);
            }
            else if (StringConditionMoreThan(nerkhDate2, date1404_02_31)) //nerkhDate2 > '1404/02/31'
            {
                return (9000, 450000);
            }
            return (0, 0);
        }
        private decimal GetMultiplier(ZaribGetDto zarib, int olgoo, bool isDomestic, bool isVillage, double monthlyConsumption, int branchType)
        {
            double zbSelection = 1;

            if (IsConstruction(branchType) && !isVillage)
            {
                return zarib.Zb;
            }
            if (isVillage && isDomestic)
            {
                return zarib.Zarib_baha;
            }
            else if (!isDomestic && !isVillage)
            {
                return zarib.Zb;
            }
            else if (!isDomestic && isVillage)
            {
                return zarib.Zb_r;
            }
            else if (isDomestic && !isVillage)
            {
                if (IsBetween(monthlyConsumption, 0, 5))
                    return zarib.Zb1;
                else if (IsBetween(monthlyConsumption, 5, 10))
                    return zarib.Zb2;
                else if (IsBetween(monthlyConsumption, 10, olgoo))
                    return zarib.Zb3;
                else if (IsBetween(monthlyConsumption, olgoo, olgoo * 1.5))
                    return zarib.Zb4;
                else if (IsBetween(monthlyConsumption, olgoo * 1.5, olgoo * 2))
                    return zarib.Zb5;
                else if (IsBetween(monthlyConsumption, olgoo * 2, olgoo * 3))
                    return zarib.Zb6;
                else if (monthlyConsumption > olgoo * 3)
                    return zarib.Zb7;
            }

            return 1;
        }       
    }
}
