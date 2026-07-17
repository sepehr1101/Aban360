using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
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
        TariffItemResult Calculate(NerkhGetDto nerkh, NerkhGetDto nerkh1403, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, ZaribGetDto zarib, AbAzadFormulaDto abAzad8And39, ConsumptionPartialInfo consumptionPartialInfo, string currentDateJalali, bool isVillageCalculation, double monthlyConsumption, int _olgoo, [Optional] int? c, [Optional] IEnumerable<int> tagIds, out double villageMultiplier);
        TariffItemResult CalculateDiscount(ConsumptionPartialInfo consumptionPartialInfo, ZaribGetDto zarib, bool isVillageCalculation, double monthlyConsumption, CustomerInfoOutputDto customerInfo, NerkhGetDto nerkh, int olgoo, TariffItemResult calculateAbBahaOutputDto, int finalDomesticUnit);
    }

    internal sealed class AbBahaCalculator : BaseExpressionCalculator, IAbBahaCalculator
    {
        const int monthDays = 30;
        const double _oldAbBahaZarib= 1.15;
        const double _villageAllowedMultiplier = 0.5;
        const double _villageDisallowedMultiplier = 0.65;
        const double _mullahMultiplier = 0.5;

        const string date_1400_12_25 = "1400/12/25";
        const string date_1402_04_23 = "1402/04/23";
        const string date_1402_12_29 = "1402/12/29";
        const string date_1403_06_25 = "1403/06/25";
        const string date_1403_09_13 = "1403/09/13";
        const string date_1403_12_30 = "1403/12/30";
        const string date_1404_02_31 = "1404/02/31";
        const string date_1404_09_09 = "1404/09/09";
        const string date_1405_01_31 = "1405/01/31";
        const string date_1405_03_15 = "1406/03/15";

        (long, long) _zero = (0, 0);
        (long, long) _8644_8644 = (8644, 8644);
        (long, long) _4323_225000 = (4323, 225000);
        (long, long) _4323_350000 = (4323, 350000);
        (long, long) _7000_350000 = (7000, 350000);
        (long, long) _9000_450000 = (9000, 450000);
        (long, long) _11000_550000 = (11000, 550000);
        (long, long) _14300_715000 = (14300, 715000);

        public TariffItemResult Calculate(NerkhGetDto nerkh, NerkhGetDto nerkh1403, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, ZaribGetDto zarib, AbAzadFormulaDto abAzad8And39, ConsumptionPartialInfo consumptionPartialInfo, string currentDateJalali, bool isVillageCalculation, double monthlyConsumption, int _olgoo, [Optional] int? c, [Optional] IEnumerable<int> tagIds, out double villageMultiplier)
        {
            double duration = consumptionPartialInfo.Duration;
            decimal multiplierAbBaha = GetMultiplier(zarib, _olgoo, IsDomesticCategory(customerInfo.UsageId), isVillageCalculation, monthlyConsumption, customerInfo.BranchType);
            villageMultiplier = GetVillageMultiplier(nerkh, customerInfo, consumptionPartialInfo, monthlyConsumption, _olgoo);

            if (nerkh.Date2.IsLt(date_1405_03_15))
            {
                double abBahaAmount = 0, oldAbBahaAmount = 0, abBahaFromExpression = 0;
                string formula = GetFormula(nerkh, nerkh1403, consumptionPartialInfo);
                abBahaFromExpression = CalcFormulaByRate(formula, monthlyConsumption, _olgoo, c, tagIds);

                //case 1: is zero
                if (CheckZero(duration, monthlyConsumption, formula))
                {
                    return new TariffItemResult();
                }

                //case 2: is construction
                if (IsConstruction(customerInfo))
                {
                    abBahaAmount = CalcFormulaByRate(abAzad8And39.Formula, monthlyConsumption, _olgoo, c, tagIds) * consumptionPartialInfo.Consumption;
                    return new TariffItemResult(abBahaAmount * (double)multiplierAbBaha);
                }

                //case 3: before 1403/09/13 and domestic group and below olgoo:
                if (IsLessThan1403_09_13(consumptionPartialInfo.EndDateJalali) &&
                    IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
                    monthlyConsumption <= _olgoo)
                {
                    oldAbBahaAmount = CalculateOldAbBahaIfPossible(nerkh, customerInfo, consumptionPartialInfo, monthlyConsumption, _olgoo, c, tagIds, oldAbBahaAmount, _oldAbBahaZarib);
                    abBahaAmount = abBahaFromExpression * monthlyConsumption * duration / monthDays;
                    (double, bool) abBahaItems = ShouldUseOldAbBaha(consumptionPartialInfo, customerInfo, monthlyConsumption, _olgoo, abBahaAmount, oldAbBahaAmount);
                    return new TariffItemResult(abBahaItems.Item1 * (double)multiplierAbBaha * villageMultiplier * customerInfo.PureDomesticUnit);
                }

                //case 4: require old ab baha but not religious
                //TODO: villageMultiplier ضرب شود در زیر الگو یا نه
                if (IsGardenOrDweltyAfter1400_12_24OrIsDomestic(customerInfo, consumptionPartialInfo) &&
                    IsBefore1403_09_13(consumptionPartialInfo.EndDateJalali))
                {
                    bool isOld = false;
                    abBahaAmount = abBahaFromExpression * monthlyConsumption * duration / monthDays; //* consumptionPartialInfo.Consumption;
                    oldAbBahaAmount = CalculateOldAbBahaIfPossible(nerkh, customerInfo, consumptionPartialInfo, monthlyConsumption, _olgoo, c, tagIds, oldAbBahaAmount, _oldAbBahaZarib);
                    (double, bool) abBahaItems = ShouldUseOldAbBaha(consumptionPartialInfo, customerInfo, monthlyConsumption, _olgoo, abBahaAmount, oldAbBahaAmount);
                    abBahaAmount = abBahaItems.Item1;
                    double upToOlgooAmount = IsDomesticWithoutUnspecified(customerInfo.UsageId) ?
                        43744.85 * (double)multiplierAbBaha * villageMultiplier * consumptionPartialInfo.Duration / monthDays * customerInfo.PureDomesticUnit
                        : 0;

                    double overalAmount = abBahaAmount * (double)multiplierAbBaha * villageMultiplier * customerInfo.PureDomesticUnit;

                    double aboveOlgoo = overalAmount > upToOlgooAmount ? overalAmount - upToOlgooAmount : 0;
                    upToOlgooAmount = overalAmount > upToOlgooAmount ? upToOlgooAmount : overalAmount;

                    return new TariffItemResult(upToOlgooAmount, aboveOlgoo);
                }

                //case 5: domestic group and subFormula not null
                if (IsGardenOrDwelty(customerInfo.UsageId) && SubFormaulaNotNull(nerkh))
                {
                    double upToOlgooAmount = CalcFormulaByRate(nerkh.AllowedFormula, _olgoo, _olgoo, c, tagIds);
                    upToOlgooAmount = upToOlgooAmount * (double)multiplierAbBaha * ((double)_olgoo / monthDays * duration) * villageMultiplier;
                    abBahaAmount = abBahaFromExpression * consumptionPartialInfo.Consumption * (double)multiplierAbBaha * villageMultiplier;

                    double aboveOlgoo = abBahaAmount > upToOlgooAmount ? abBahaAmount - upToOlgooAmount : 0;
                    upToOlgooAmount = abBahaAmount > upToOlgooAmount ? upToOlgooAmount : abBahaAmount;

                    return new TariffItemResult(upToOlgooAmount, aboveOlgoo);
                }

                //case 6: domestic group and subFormula null and below olgoo
                if (IsGardenOrDwelty(customerInfo.UsageId) && monthlyConsumption <= _olgoo)
                {
                    abBahaAmount = abBahaFromExpression * consumptionPartialInfo.Consumption * villageMultiplier;
                    return new TariffItemResult(abBahaAmount * (double)multiplierAbBaha);
                }

                //case 7 domestic group and subFormula is null and above olgoo
                if (IsGardenOrDwelty(customerInfo.UsageId) && monthlyConsumption > _olgoo)
                {
                    //double upToOlgooVillageMultiplier= GetVillageMultiplier(nerkh, customerInfo, consumptionPartialInfo, monthlyConsumption, _olgoo);
                    double upToOlgooAmount = CalcFormulaByRate(nerkh.Vaj, _olgoo, _olgoo, c, tagIds);
                    upToOlgooAmount = upToOlgooAmount * (double)multiplierAbBaha * ((double)_olgoo / monthDays * duration) * villageMultiplier;

                    abBahaAmount = abBahaFromExpression * consumptionPartialInfo.Consumption * (double)multiplierAbBaha * villageMultiplier;

                    double aboveOlgoo = abBahaAmount > upToOlgooAmount ? abBahaAmount - upToOlgooAmount : 0;
                    upToOlgooAmount = abBahaAmount > upToOlgooAmount ? upToOlgooAmount : abBahaAmount;

                    return new TariffItemResult(upToOlgooAmount, aboveOlgoo);
                }

                //case 8: (is religious or has capacity) and is charity !
                if ((HasCapacity(customerInfo) || IsReligious(customerInfo.UsageId)) &&
                     IsCharitySchoolOrConsumptionGtCapacity(consumptionPartialInfo, customerInfo, consumptionPartialInfo.OlgooOrCapacityInDuration))
                {
                    (double, double) abBahaValues = (0, 0);
                    (long, long) _2Amount = Get2Amount(consumptionPartialInfo, customerInfo, abAzad8And39, abBahaFromExpression, _olgoo, monthlyConsumption, c, tagIds);
                    abBahaValues.Item1 = _2Amount.Item1 * (IsReligiousAndZeroCapacity(customerInfo) ? consumptionPartialInfo.Consumption : consumptionPartialInfo.AllowedConsumption);
                    abBahaValues.Item2 = _2Amount.Item2 * (IsReligiousAndZeroCapacity(customerInfo) ? 0 : consumptionPartialInfo.DisallowedConsumtion);
                    abBahaAmount = abBahaValues.Item1 + abBahaValues.Item2;
                    abBahaAmount = abBahaAmount * (double)multiplierAbBaha * villageMultiplier;
                    abBahaValues = CheckAbBahaValues(abBahaAmount, abBahaValues);
                    double abBaha1 = abBahaValues.Item1 * (double)multiplierAbBaha;
                    double abBaha2 = abBahaValues.Item2 * (double)multiplierAbBaha;
                    return new TariffItemResult(abBaha1, abBaha2);
                }

                //case 9: other
                abBahaAmount = consumptionPartialInfo.Consumption * abBahaFromExpression * (double)multiplierAbBaha * villageMultiplier;
                return new TariffItemResult(abBahaAmount);
            }
            else
            {
                //case 1: is zero
                if (CheckZero(duration, monthlyConsumption, nerkh.AllowedFormula))
                {
                    return new TariffItemResult();
                }

                //case 2: is construction
                if (IsConstruction(customerInfo))
                {
                    (double, double) abBahaAmountConstruction = CalcFormula(abAzad8And39.AllowedFormula, abAzad8And39.DisallowedFormula, monthlyConsumption, _olgoo, c, multiplierAbBaha, 1, 1, 1, customerInfo, consumptionPartialInfo, tagIds);
                    return new TariffItemResult(abBahaAmountConstruction.Item1, abBahaAmountConstruction.Item2);
                }

                //case3: as formuala is
                bool isVillage = IsRural(nerkh, customerInfo, consumptionPartialInfo, monthlyConsumption, _olgoo);
                bool isDomestic = IsDomesticWithoutUnspecified(customerInfo.UsageId);
                decimal k1 = GetK1(zarib, _olgoo, isVillage);

                decimal allowedKModifier = isVillage && isDomestic ? (decimal)_villageAllowedMultiplier : 1M;
                decimal disAllowedKModifier = isVillage && isDomestic ? (decimal)_villageDisallowedMultiplier : 1M;
                (double, double) abBahaAmount = CalcFormula(nerkh.AllowedFormula, nerkh.DisallowedFormula, monthlyConsumption, _olgoo, c, multiplierAbBaha, k1, allowedKModifier, disAllowedKModifier, customerInfo, consumptionPartialInfo, tagIds);
                return new TariffItemResult(abBahaAmount.Item1, abBahaAmount.Item2);
            }
        }

        public TariffItemResult CalculateDiscount(ConsumptionPartialInfo consumptionPartialInfo, ZaribGetDto zarib, bool isVillageCalculation, double monthlyConsumption, CustomerInfoOutputDto customerInfo, NerkhGetDto nerkh, int olgoo, TariffItemResult calculateAbBahaOutputDto, int finalDomesticUnit)
        {
            if (nerkh.Date2.IsLt(date_1405_03_15))
            {
                if (calculateAbBahaOutputDto.Summation == 0)
                {
                    return new TariffItemResult();
                }
                if (IsConstruction(customerInfo.BranchType))
                {
                    return new TariffItemResult();
                }
                decimal multiplier = GetMultiplier(zarib, olgoo, IsDomesticCategory(customerInfo.UsageId), isVillageCalculation, monthlyConsumption, customerInfo.BranchType);

                if (IsUnderSocialService(customerInfo.BranchType) &&
                    IsDomesticWithoutUnspecified(customerInfo.UsageId))
                {
                    double discountVillageMultiplier = isVillageCalculation ? _villageAllowedMultiplier : 1;
                    double villageMultiplier = GetVillageMultiplier(nerkh, customerInfo, consumptionPartialInfo, monthlyConsumption, olgoo);
                    double allowedDiscount = calculateAbBahaOutputDto.Allowed / villageMultiplier * discountVillageMultiplier; /* * villageMultiplier.Item1*/ ;
                    return new TariffItemResult(allowedDiscount);
                }
                if (IsMullah(customerInfo.BranchType) && isVillageCalculation && customerInfo.UnitAll == 1)
                {
                    double villageMultiplier = GetVillageMultiplier(nerkh, customerInfo, consumptionPartialInfo, monthlyConsumption, olgoo);
                    double allowedDiscount = (calculateAbBahaOutputDto.Allowed / villageMultiplier) * _mullahMultiplier;
                    return new TariffItemResult(allowedDiscount);
                }
                if (IsReligiousWithCharity(customerInfo.UsageId))
                {
                    return new TariffItemResult(calculateAbBahaOutputDto.Allowed);
                }
                if (IsQuranAfter1404_01_01(customerInfo.UsageId, consumptionPartialInfo.StartDateJalali))
                {
                    return new TariffItemResult(calculateAbBahaOutputDto.Allowed);
                }
                double virtualDiscount = CalculateDiscountByVirtualCapacity(customerInfo, consumptionPartialInfo.Consumption, consumptionPartialInfo.Duration, calculateAbBahaOutputDto.Summation, consumptionPartialInfo);
                double finalVirtualDiscount = virtualDiscount > 0 ? (long)virtualDiscount : 0;
                return new TariffItemResult(finalVirtualDiscount);
            }
            else
            {
                if (calculateAbBahaOutputDto.Summation == 0)
                {
                    return new TariffItemResult();
                }
                if (IsConstruction(customerInfo.BranchType))
                {
                    return new TariffItemResult();
                }

                if (IsUnderSocialService(customerInfo.BranchType) &&
                    IsDomesticWithoutUnspecified(customerInfo.UsageId))
                {
                    return new TariffItemResult(calculateAbBahaOutputDto.Allowed);
                }
                if (IsMullah(customerInfo.BranchType) && isVillageCalculation && customerInfo.UnitAll == 1)
                {
                    return new TariffItemResult(calculateAbBahaOutputDto.Allowed);
                }
                if (HasReligiousDiscount(customerInfo.UsageId))
                {
                    return new TariffItemResult(calculateAbBahaOutputDto.Allowed);
                }
                double virtualDiscount = CalculateDiscountByVirtualCapacity(customerInfo, consumptionPartialInfo.Consumption, consumptionPartialInfo.Duration, calculateAbBahaOutputDto.Summation, consumptionPartialInfo);
                double finalVirtualDiscount = virtualDiscount > 0 ? (long)virtualDiscount : 0;
                return new TariffItemResult(finalVirtualDiscount);
            }
        }

        #region private methods
        private bool SubFormaulaNotNull(NerkhGetDto nerkh)
        {
            return !string.IsNullOrWhiteSpace(nerkh.AllowedFormula) &&
                   !string.IsNullOrWhiteSpace(nerkh.DisallowedFormula);
        }
        private bool IsLessThan1403_09_13AndOvajNotZero(NerkhGetDto nerkh, ConsumptionPartialInfo consumptionPartialInfo)
        {
            return !string.IsNullOrWhiteSpace(nerkh.OVaj) &&
                                nerkh.OVaj.Trim() != "0" &&
                                IsLessThan1403_09_13(consumptionPartialInfo.EndDateJalali);
        }

        private bool IsLessThan1403_09_13OrMonthlyBelowEqOlgoo(ConsumptionPartialInfo consumptionPartialInfo, double monthlyConsumption, int _olgoo)
        {
            return IsLessThan1403_09_13(consumptionPartialInfo.EndDateJalali) || monthlyConsumption <= _olgoo;
        }

        private bool IsDolatabadOrHabibabadAndDomesticAndNotConstruction(CustomerInfoOutputDto customerInfo)
        {
            return IsDolatabadOrHabibabadWithConditionEshtrak(customerInfo.ZoneId, customerInfo.ReadingNumber) &&
                   IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
                   !IsConstruction(customerInfo.BranchType) ;
        }

        private bool IsGardenOrDweltyAfter1400_12_24OrIsDomestic(CustomerInfoOutputDto customerInfo, ConsumptionPartialInfo consumptionPartialInfo)
        {
            return IsGardenOrDweltyAfter1400_12_24(customerInfo.UsageId, consumptionPartialInfo.StartDateJalali) || IsDomestic(customerInfo.UsageId);
        }
        private bool IsBefore1403_09_13(string date2)
        {
            return date2.CompareTo(date_1403_09_13) <= 0;
        }

        private bool HasCapacity(CustomerInfoOutputDto customerInfo)
        {
            return customerInfo.ContractualCapacity > 0;
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

        private bool IsCharitySchoolOrConsumptionGtCapacity(ConsumptionPartialInfo consumptionPartialInfo, CustomerInfoOutputDto customerInfo, double contractualCapacityInDuration)
        {
            return consumptionPartialInfo.Consumption > contractualCapacityInDuration ||
                   IsCharityOrSchool(customerInfo.UsageId);
        }

        private (long, long) Get2Amount(ConsumptionPartialInfo consumptionPartialInfo, CustomerInfoOutputDto customerInfo, AbAzadFormulaDto abAzad8And39, double abBahaFromExpression, int olgoo, double monthlyConsumption, [Optional] int? c, [Optional] IEnumerable<int> tagIds)
        {
            if (IsReligiousWithCharity(customerInfo.UsageId) ||
                IsQuranAfter1404_01_01(customerInfo.UsageId, consumptionPartialInfo.StartDateJalali))
            {
                return Get2PartAmountReligious(consumptionPartialInfo.EndDateJalali);
            }
            return GetEducationOrBathMultiplier(customerInfo.UsageId, consumptionPartialInfo.StartDateJalali, consumptionPartialInfo.EndDateJalali, customerInfo.IsSpecial, (long)CalcFormulaByRate(abAzad8And39.Formula, monthlyConsumption, olgoo, c, tagIds), abBahaFromExpression);//Azad:39
        }
        private (long, long) GetEducationOrBathMultiplier(int usageId, string nerkhDate1, string nerkhDate2, bool isSpecial, long abAzad, double abBahaFromExpression)
        {  
            if (IsEducationOrBath(usageId))
            {
                if (LessThanEq(nerkhDate2, date_1402_04_23))
                {
                    return IsBath(usageId) ? _8644_8644 : _4323_225000;
                }
                if (IsGtFromLqTo(nerkhDate2, date_1402_04_23, date_1403_06_25))
                {
                    return IsBath(usageId) ? _8644_8644 : _4323_225000;
                }
                else if (IsGtFromLqTo(nerkhDate2, date_1403_06_25, date_1403_09_13))
                {
                    return IsBath(usageId) ? _8644_8644 : _4323_350000;
                }
                else if (IsGtFromLqTo(nerkhDate2, date_1403_09_13, date_1404_02_31))
                {
                    return _7000_350000;
                }
                else if(IsGtFromLqTo(nerkhDate2, date_1404_02_31, date_1404_09_09))
                {
                    return _9000_450000;
                }
                else if (IsGtFromLqTo(nerkhDate2, date_1404_09_09, date_1405_01_31))
                {
                    return _11000_550000;
                }
                return _14300_715000;
            }
            else
            {
                return ((long)abBahaFromExpression, abAzad);
            }
        }
        private (long, long) Get2PartAmountReligious(string nerkhDate2)
        {   
            if (date_1400_12_25.MoreOrEq(nerkhDate2))
            {
                return _4323_225000;//_3766_168110;
            }
            else if (IsGtFromLqTo(nerkhDate2, date_1400_12_25, date_1402_04_23))
            {
                return _4323_225000;//_4040_168110;
            }

            // در تاریخ 25 فروردین 1405 به 2 نرخ شکسته شد
            /*else if (IsGtFromLqTo(nerkhDate2, date_1402_04_23, date_1403_06_25))
            {
                return _4323_225000;
            }*/
            else if (IsGtFromLqTo(nerkhDate2, date_1402_04_23, date_1402_12_29))
            {
                return _4323_225000;
            }
            else if (IsGtFromLqTo(nerkhDate2, date_1402_12_29, date_1403_06_25))
            {
                return (4323, 45000);//_4323_225000; 25 فروردین 1405
            }

            else if (IsGtFromLqTo(nerkhDate2, date_1403_06_25, date_1403_09_13))
            {
                return (4323, 70000);//_4323_350000;//25 فروردین 1405
            }
            //به دو نرخ شکسته شد در تاریخ 5 اسفند 1404
            //else if (IsGtFromLqTo(nerkhDate2, date_1403_09_13, date_1404_02_31))
            //{
            //    return _7000_350000;
            //}
            else if (IsGtFromLqTo(nerkhDate2, date_1403_09_13, date_1403_12_30))
            {
                return (7000, 70000);//_7000_350000 (changed in 1405/01/25)
            }
            else if (IsGtFromLqTo(nerkhDate2, date_1403_12_30, date_1404_02_31))
            {
                return (7000, 70000);// _7000_350000;
            }
            else if (IsGtFromLqTo(nerkhDate2, date_1404_02_31, date_1404_09_09))
            {
                return (9000, 90000);//_9000_450000;
            }
            else if (IsGtFromLqTo(nerkhDate2, date_1404_09_09, date_1405_01_31))
            {
                return (11000, 110000);//_11000_550000;
            }
            else if (nerkhDate2.MoreOrEq(date_1405_01_31))
            {
                return (14300, 143000);
            }
            return _zero;
        }

        private double CalcFormulaByRate(string formula, double monthlyAverageConsumption, int olgoo, [Optional] int? c, [Optional] IEnumerable<int> tagIds)
        {
            object parameters = new { X = monthlyAverageConsumption, C = c, S = olgoo, tags = tagIds.ToArray() };
            double value = Eval<double>(formula, parameters);
            return value;
        }     
        private (double,double) CalcFormula(string allowedFormula, string disallowedFormula, double monthlyAverageConsumption, int olgoo, int? c, decimal zoneMultiplier, decimal zoneMultipler2, decimal allowedKModifier, decimal disAllowedKModifier, CustomerInfoOutputDto customerInfo, ConsumptionPartialInfo consumptionPartialInfo,[Optional] IEnumerable<int> tagIds)
        {
            object parametersAllowed = new
            {
                X = monthlyAverageConsumption,
                C = c,
                S = olgoo,
                K = (double)(zoneMultiplier*allowedKModifier),
                K1= (double)(zoneMultipler2*allowedKModifier),
                D = (double)consumptionPartialInfo.Duration,
                L = (double)consumptionPartialInfo.AllowedConsumption,
                Q = (double)consumptionPartialInfo.DisallowedConsumtion,
                T = (double)(IsDomesticWithoutUnspecified(customerInfo.UsageId) ? customerInfo.DomesticUnitForHousehold : customerInfo.UnitAll),
                Z = (double)customerInfo.ContractualCapacity,
                tags = tagIds.ToArray()
            };
            object parametersDisallowed = new
            {
                X = monthlyAverageConsumption,
                C = c,
                S = olgoo,
                K = (double)(zoneMultiplier*disAllowedKModifier),
                K1 = (double)(zoneMultipler2 * allowedKModifier),
                D = (double)consumptionPartialInfo.Duration,
                L = (double)consumptionPartialInfo.AllowedConsumption,
                Q = (double)consumptionPartialInfo.DisallowedConsumtion,
                T = (double)(IsDomesticWithoutUnspecified(customerInfo.UsageId) ? customerInfo.DomesticUnitForHousehold : customerInfo.UnitAll),
                Z = (double)customerInfo.ContractualCapacity,
                tags = tagIds.ToArray()
            };
            double allowed = Eval<double>(allowedFormula, parametersAllowed);
            double disallowed = Eval<double>(disallowedFormula, parametersDisallowed);
            return (allowed, disallowed);
        }
        private decimal GetMultiplier(ZaribGetDto zarib, int olgoo, bool isDomestic, bool isVillage, double monthlyConsumption, int branchType)
        {
            decimal rawMultiplier= GetRawMultiplier(zarib, olgoo, isDomestic, isVillage, monthlyConsumption, branchType);
            return (!isDomestic && rawMultiplier < 1) || (IsConstruction(branchType) && rawMultiplier < 1) ?
                1 : rawMultiplier;
        }
        private decimal GetRawMultiplier(ZaribGetDto zarib, int olgoo, bool isDomestic, bool isVillage, double monthlyConsumption, int branchType)
        {
            double zbSelection = 1;

            if (IsConstruction(branchType) && !isVillage)
            {
                return zarib.Zb;
            }

            // مسکونی روستایی
            if (isVillage && isDomestic)
            {
                return zarib.Zarib_baha;
            }

            //غیرمسکونی شهری
            else if (!isDomestic && !isVillage)
            {
                return zarib.Zb;
            }

            //غیرمسکونی روستایی
            else if (!isDomestic && isVillage)
            {
                return zarib.Zb_r;
            }

            //در طبقات الگو، مسکونی شهری
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

            //سایر
            return 1;
        }       
        private decimal GetK1(ZaribGetDto zarib, int olgoo, bool isVillage)
        {
            return zarib.Zb3;
        }
        private string GetOldFormula(string oldVaj, bool isDomestic, double monthlyConsumption)
        {
            if (!isDomestic)
            {
                return oldVaj;
            }
            if (IsBetween(monthlyConsumption, 0, 5))
                return "(X*1860)";
            if (IsBetween(monthlyConsumption, 5.0000001, 10))
                return "(X*2783)-4615";
            if (IsBetween(monthlyConsumption, 10.0000001, 14))
                return "(X*3706)-13845";

            return oldVaj;
        }        
        private string GetFormula(NerkhGetDto nerkh, NerkhGetDto nerkh1403, ConsumptionPartialInfo consumptionPartialInfo)
        {
           return LessThanEq(consumptionPartialInfo.EndDateJalali, date_1403_12_30) ? nerkh1403.Vaj : nerkh.Vaj;
        }

        private double CalculateOldAbBahaIfPossible(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, ConsumptionPartialInfo consumptionPartialInfo, double monthlyConsumption, int _olgoo, int? c, IEnumerable<int> tagIds, double oldAbBahaAmount, double _oldAbBahaZarib)
        {
            if (IsLessThan1403_09_13AndOvajNotZero(nerkh, consumptionPartialInfo))
            {
                string oldFormula = GetOldFormula(nerkh.OVaj, IsDomestic(customerInfo.UsageId), monthlyConsumption);
                double oldAbBahaFromExpression = CalcFormulaByRate(oldFormula, monthlyConsumption, _olgoo, c, tagIds);
                oldAbBahaAmount = ((double)consumptionPartialInfo.Duration / (double)monthDays) * oldAbBahaFromExpression * _oldAbBahaZarib;
            }
            return oldAbBahaAmount;
        }

        private (double, bool) ShouldUseOldAbBaha(ConsumptionPartialInfo consumptionPartialInfo, CustomerInfoOutputDto customerInfo, double monthlyConsumption, int _olgoo, double abBahaAmount, double oldAbBahaAmount)
        {
            bool isOld = false;
            if (IsLessThan1403_09_13(consumptionPartialInfo.EndDateJalali) &&
                monthlyConsumption <= _olgoo &&
                abBahaAmount > oldAbBahaAmount &&
                IsDomesticWithoutUnspecified(customerInfo.UsageId))
            {
                abBahaAmount = oldAbBahaAmount;
                isOld = true;
            }

            return (abBahaAmount, isOld);
        }

        private double GetVillageMultiplier(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, ConsumptionPartialInfo consumptionPartialInfo, double monthlyConsumption, int _olgoo)
        {
            if ((IsVillageDomesticNotConstruction(customerInfo) && !IsRuralButIsMetro(customerInfo)) ||
                IsDolatabadOrHabibabadAndDomesticAndNotConstruction(customerInfo))
            {
                double multiplier = IsLessThan1403_09_13OrMonthlyBelowEqOlgoo(consumptionPartialInfo, monthlyConsumption, _olgoo) ?
                    _villageAllowedMultiplier : _villageDisallowedMultiplier;
                return multiplier;
            }
            return 1;
        }
        private bool IsRural(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, ConsumptionPartialInfo consumptionPartialInfo, double monthlyConsumption, int _olgoo)
        {
            if ((IsVillageDomesticNotConstruction(customerInfo) && !IsRuralButIsMetro(customerInfo)) ||
                IsDolatabadOrHabibabadAndDomesticAndNotConstruction(customerInfo))
            {
               
                return true;
            }
            return false;
        }
        #endregion
    }
}
