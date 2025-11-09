using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using System.Runtime.InteropServices;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffStringChecker;

namespace Aban360.OldCalcPool.Application.Features.Processing.Helpers
{
    internal interface IAbBahaCalculator
    {
        CalculateAbBahaOutputDto CalculateAbBaha(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, ZaribGetDto zarib, AbAzadFormulaDto abAzad8And39, string currentDateJalali, bool isVillageCalculation, double monthlyConsumption, int _olgoo, decimal multiplierAbBaha, [Optional] int? c, [Optional] IEnumerable<int> tagIds);
    }

    internal sealed class AbBahaCalculator : BaseExpressionCalculator, IAbBahaCalculator
    {
        const int monthDays = 30;
        public CalculateAbBahaOutputDto CalculateAbBaha(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, ZaribGetDto zarib, AbAzadFormulaDto abAzad8And39, string currentDateJalali, bool isVillageCalculation, double monthlyConsumption, int _olgoo, decimal multiplierAbBaha, [Optional] int? c, [Optional] IEnumerable<int> tagIds)
        {
            double abBahaAmount = 0, oldAbBahaAmount = 0, abBahaFromExpression = 0, oldAbBahaZarib = 1.15;
            double duration = nerkh.Duration;
            abBahaFromExpression = CalcFormulaByRate(nerkh.Vaj, monthlyConsumption, _olgoo, c, tagIds);
            (double, double) abBahaValues = (0, 0);

            if (CheckZero(duration, monthlyConsumption, nerkh.Vaj))
            {
                return new CalculateAbBahaOutputDto(0, (0, 0));
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
                    double contractualCapacityInDuration = ((double)customerInfo.ContractualCapacity / monthDays) * duration;

                    if (IsCharitySchoolOrConsumptionGtCapacity(nerkh, customerInfo, contractualCapacityInDuration))
                    {
                        double disallowedPartialConsumption = nerkh.PartialConsumption - contractualCapacityInDuration;
                        double allowedPartialConsumption = contractualCapacityInDuration;

                        if (nerkh.PartialConsumption < contractualCapacityInDuration ||
                            IsReligiousAndZeroCapacity(customerInfo))
                        {
                            disallowedPartialConsumption = 0;
                            allowedPartialConsumption = nerkh.PartialConsumption;
                        }//L 1153

                        (long, long) _2Amount = Get2Amount(nerkh, customerInfo, abAzad8And39, abBahaFromExpression, _olgoo, monthlyConsumption,c,tagIds);

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

            return new CalculateAbBahaOutputDto(abBahaAmount, abBahaValues);
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
            return (customerInfo.ContractualCapacity > 0 && !IsConstruction(customerInfo.BranchType));
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
    }
}
