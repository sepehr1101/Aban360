using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Application.Features.Processing.Helpers;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using System.Runtime.InteropServices;
using static Aban360.Common.Timing.CalculationDistanceDate;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffStringChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.VirtualCapacityCalculator;

namespace Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators
{
    internal interface IFazelabCalculator
    {
        TariffItemResult Calculate(NerkhGetDto? nerkh, double? monthlyConsumption, int? s, int? c, ZaribGetDto zarib, string date1, string date2, int durationAll, CustomerInfoOutputDto customerInfo, double abBahaItemAmount, string currentDateJalali, bool isAbonman, ConsumptionPartialInfo consumptionPartialInfo, TariffItemResult abCalcResult, out double multiplier);
        TariffItemResult CalculateDiscount(NerkhGetDto nerkh, TariffItemResult fazelabCalculationResult , double abBahaDiscount, double fazelabAmount, CustomerInfoOutputDto customerInfo, ConsumptionPartialInfo consumptionPartialInfo);
    }

    internal sealed class FazelabCalculator : BaseExpressionCalculator, IFazelabCalculator
    {       
        private const string _minimumValidDate = "1330/01/01";
        private const string date_1405_03_15 = "1406/03/15";
        private const int _withoutSewage = 0;
        private const int _firstCalculation = 1;
        private const int _normal = 2;
        const double _villageAllowedMultiplier = 0.5;
        const double _villageDisallowedMultiplier = 0.65;

        public TariffItemResult Calculate(NerkhGetDto? nerkh, double? monthlyConsumption, int? s, int? c, ZaribGetDto zarib, string date1, string date2, int durationAll, CustomerInfoOutputDto customerInfo, double abBahaItemAmount, string currentDateJalali, bool isAbonman, ConsumptionPartialInfo consumptionPartialInfo, TariffItemResult abCalcResult, out double multiplier)
        {
            if (date2.IsLt(date_1405_03_15))
            {
                double sewageAmount = 0;

                //محاسبه کارمزد دفع در کاربری های گروه خانگی ضریب 0.7
                multiplier = GetMultiplier(isAbonman, customerInfo.UsageId);

                if (IsConstruction(customerInfo.BranchType) && !isAbonman)
                {
                    return new TariffItemResult();
                }
                if (IsUsageConstructor(customerInfo.UsageId) && !isAbonman)
                {
                    return new TariffItemResult();
                }
                if (IsTankerSale(customerInfo.UsageId))
                {
                    return new TariffItemResult();
                }
                if (customerInfo.SewageCalcState == _withoutSewage || string.IsNullOrWhiteSpace(customerInfo.SewageInstallationDateJalali))
                {
                    return new TariffItemResult();
                }
                // ثبت نصب بعد از تاریخ قرائت لحاظ شده
                if (IsInstallAfterReading(date2, customerInfo))
                {
                    return new TariffItemResult();
                }
                else if (IsFirstCalculation(date2, customerInfo))
                {
                    CalcDistanceResultDto calcDistance = CalcDistance(customerInfo.SewageInstallationDateJalali, date2, true, customerInfo);
                    int duration = 0;
                    if (calcDistance.HasError)
                    {
                        throw new TariffDateException(customerInfo.BillId + " - " + ExceptionLiterals.Incalculable);
                    }
                    duration = calcDistance.Distance;
                    sewageAmount = (abBahaItemAmount / durationAll) * duration * multiplier;
                    double allowedRatio = abCalcResult.Allowed / (abCalcResult.Summation > 0 ? abCalcResult.Summation : 1);
                    //TODO: Update SewageStateToNormal in DB
                    return new TariffItemResult(allowedRatio * sewageAmount, (1 - allowedRatio) * sewageAmount);
                }
                //نرمال اما تاریخ نصب قبل از تاریخ قرائت و بعد از ابتدای دوره مصرف، پس بخشی از آن باید حساب شود
                else if (InstallBetweenReadingPeriod(date1, date2, customerInfo))
                {
                    CalcDistanceResultDto calcDistance = CalcDistance(customerInfo.SewageInstallationDateJalali, date2, true, customerInfo);
                    int duration = 0;
                    if (calcDistance.HasError)
                    {
                        throw new TariffDateException(customerInfo.BillId + " - " + ExceptionLiterals.Incalculable);
                    }
                    duration = calcDistance.Distance;
                    sewageAmount = (abBahaItemAmount / durationAll) * duration * multiplier;
                    double allowedRatio = abCalcResult.Allowed / (abCalcResult.Summation > 0 ? abCalcResult.Summation : 1);
                    return new TariffItemResult(allowedRatio * sewageAmount, (1 - allowedRatio) * sewageAmount);
                }
                else if (IsTotallyNormal(customerInfo, currentDateJalali))
                {
                    sewageAmount = abBahaItemAmount * multiplier;
                }
                //TODO: ظاهرا بی تاثیر نیاز به refactor
                if (!isAbonman && abCalcResult.Allowed > 0 && abCalcResult.Disallowed > 0)
                {
                    double allowedRatio = abCalcResult.Allowed / (abCalcResult.Summation > 0 ? abCalcResult.Summation : 1);
                    return new TariffItemResult(abCalcResult.Allowed * multiplier, abCalcResult.Disallowed * multiplier);
                }
                return new TariffItemResult(consumptionPartialInfo.AllowedRatio * sewageAmount, consumptionPartialInfo.DisallwedRatio * sewageAmount);
            }
            else
            {               
                multiplier = 1;
                if (isAbonman)
                {
                    return new TariffItemResult();
                }

                if (IsConstruction(customerInfo.BranchType))
                {
                    return new TariffItemResult();
                }
                if (IsUsageConstructor(customerInfo.UsageId))
                {
                    return new TariffItemResult();
                }
                if (IsTankerSale(customerInfo.UsageId))
                {
                    return new TariffItemResult();
                }
                if (customerInfo.SewageCalcState == _withoutSewage || string.IsNullOrWhiteSpace(customerInfo.SewageInstallationDateJalali))
                {
                    return new TariffItemResult();
                }

                int duration=0;
                // ثبت نصب بعد از تاریخ قرائت لحاظ شده
                if (IsInstallAfterReading(date2, customerInfo))
                {
                    return new TariffItemResult();
                }                
                else if (IsFirstCalculation(date2, customerInfo))
                {
                    CalcDistanceResultDto calcDistance = CalcDistance(customerInfo.SewageInstallationDateJalali, date2, true, customerInfo);                   
                    if (calcDistance.HasError)
                    {
                        throw new TariffDateException(customerInfo.BillId + " - " + ExceptionLiterals.Incalculable);
                    }
                    //TODO: Update SewageStateToNormal in DB
                    duration = calcDistance.Distance;
                }
                //نرمال اما تاریخ نصب قبل از تاریخ قرائت و بعد از ابتدای دوره مصرف، پس بخشی از آن باید حساب شود
                else if (InstallBetweenReadingPeriod(date1, date2, customerInfo))
                {
                    CalcDistanceResultDto calcDistance = CalcDistance(customerInfo.SewageInstallationDateJalali, date2, true, customerInfo);                    
                    if (calcDistance.HasError)
                    {
                        throw new TariffDateException(customerInfo.BillId + " - " + ExceptionLiterals.Incalculable);
                    }
                    duration = calcDistance.Distance;
                }
                else if (IsTotallyNormal(customerInfo, currentDateJalali))
                {
                    duration = consumptionPartialInfo.Duration;
                }
                bool isVillage = IsRural(nerkh, customerInfo, consumptionPartialInfo, monthlyConsumption.Value, s.Value);
                bool isDomestic = IsDomesticWithoutUnspecified(customerInfo.UsageId);
                decimal multiplierAbBaha = GetMultiplier(zarib, s.Value, IsDomesticCategory(customerInfo.UsageId), isVillage, monthlyConsumption.Value, customerInfo.BranchType);
                decimal k1 = GetK1(zarib, s.Value, isVillage);
                decimal allowedKModifier = isVillage && isDomestic ? (decimal)_villageAllowedMultiplier : 1M;
                decimal disAllowedKModifier = isVillage && isDomestic ? (decimal)_villageDisallowedMultiplier : 1M;
                (double, double) values = CalcFormula(nerkh, monthlyConsumption.Value, s.Value, c, zoneMultiplier: multiplierAbBaha, zoneMultiplier2:k1, duration, allowedKModifier, disAllowedKModifier, customerInfo, consumptionPartialInfo);
                return new TariffItemResult(values.Item1, values.Item2);
            }
        }
        public TariffItemResult CalculateDiscount(NerkhGetDto nerkh,TariffItemResult fazelabCalculationResult, double abBahaDiscount, double fazelabAmount, CustomerInfoOutputDto customerInfo, ConsumptionPartialInfo consumptionPartialInfo)
        {
            if (nerkh.Date2.IsLt(date_1405_03_15))
            {
                if (abBahaDiscount <= 0)
                {
                    return new TariffItemResult();
                }
                if (fazelabAmount <= 0)
                {
                    return new TariffItemResult();
                }
                if (IsConstruction(customerInfo.BranchType))
                {
                    return new TariffItemResult();
                }
                if (IsUsageConstructor(customerInfo.UsageId))
                {
                    return new TariffItemResult();
                }
                /* if (date_1404_02_31.MoreOrEq(consumptionPartialInfo.EndDateJalali) && IsSchool(customerInfo.UsageId))
                 {
                     return new TariffItemResult();
                 }*/
                double fazelabDiscount = abBahaDiscount * GetMultiplier(false, customerInfo.UsageId);
                double virtualDiscount = CalculateDiscountByVirtualCapacity(customerInfo, consumptionPartialInfo.Consumption, consumptionPartialInfo.Duration, fazelabDiscount, consumptionPartialInfo);
                double finalDiscount = virtualDiscount > 0 ? virtualDiscount : fazelabDiscount;//fazelabAmount
                return new TariffItemResult(finalDiscount);
            }
            else
            {
                if (abBahaDiscount <= 0)
                {
                    return new TariffItemResult();
                }
                if (fazelabAmount <= 0)
                {
                    return new TariffItemResult();
                }
                if (IsConstruction(customerInfo))
                {
                    return new TariffItemResult();
                }
                if (IsUnderSocialService(customerInfo.BranchType) &&
                   IsDomesticWithoutUnspecified(customerInfo.UsageId))
                {
                    return new TariffItemResult(fazelabCalculationResult.Allowed);
                }
                if (IsMullah(customerInfo.BranchType) && customerInfo.UnitAll == 1)
                {
                    return new TariffItemResult(fazelabCalculationResult.Allowed);
                }
                if (HasReligiousDiscount(customerInfo.UsageId))
                {
                    return new TariffItemResult(fazelabCalculationResult.Allowed);
                }
                double virtualDiscount = CalculateDiscountByVirtualCapacity(customerInfo, consumptionPartialInfo.Consumption, consumptionPartialInfo.Duration, fazelabCalculationResult.Summation, consumptionPartialInfo);
                double finalDiscount = virtualDiscount > 0 ? virtualDiscount : abBahaDiscount;//fazelabAmount
                return new TariffItemResult(finalDiscount);
            }
        }

        #region private methods
        private bool IsTotallyNormal(CustomerInfoOutputDto customerInfo, string currentDateJalali)
        {
            return
                customerInfo.SewageCalcState == _normal ||
                currentDateJalali == customerInfo.SewageInstallationDateJalali;
        }

        private bool InstallBetweenReadingPeriod(string date1, string date2, CustomerInfoOutputDto customerInfo)
        {
            return customerInfo.SewageCalcState == _normal &&
                   string.Compare(date2, customerInfo.SewageInstallationDateJalali) > 0 &&
                   string.Compare(date1, customerInfo.SewageInstallationDateJalali) <= 0 &&
                   string.Compare(customerInfo.SewageInstallationDateJalali, _minimumValidDate) > 0 &&
                   customerInfo.SewageInstallationDateJalali.Trim().Length == 10;
        }

        private bool IsFirstCalculation(string date2, CustomerInfoOutputDto customerInfo)
        {
            int jalaliDateLength = 10;
            return customerInfo.SewageCalcState == _firstCalculation &&
                   string.Compare(date2, customerInfo.SewageInstallationDateJalali) > 0 &&
                   customerInfo.SewageInstallationDateJalali.Trim().Length == jalaliDateLength;
        }

        private bool IsInstallAfterReading(string date2, CustomerInfoOutputDto customerInfo)
        {
            if (string.Compare(customerInfo.SewageRegisterDate, _minimumValidDate) >= 0 &&
                string.Compare(customerInfo.SewageRegisterDate, date2) > 0)
            {
                return true;
            }
            if (customerInfo.SewageCalcState == _normal &&
                string.Compare(date2, customerInfo.SewageInstallationDateJalali) < 0)
            {
                return true;
            }
            return false;
        }

        private double GetMultiplier(bool isAbonman, int usageId)
        {
            return !isAbonman && IsDomesticCategory(usageId) ? 0.7 : 1;
        }
        private (double, double) CalcFormula(NerkhGetDto nerkh, double monthlyAverageConsumption, int olgoo, int? c, decimal zoneMultiplier, decimal zoneMultiplier2, int duration, decimal allowedKModifier, decimal disAllowedKModifier, CustomerInfoOutputDto customerInfo, ConsumptionPartialInfo consumptionPartialInfo, [Optional] IEnumerable<int> tagIds)
        {
            object parametersAllowed = new
            {
                X = monthlyAverageConsumption,
                C = c,
                S = olgoo,
                K = (double)zoneMultiplier,
                K1= (double)(zoneMultiplier2 * allowedKModifier),
                D = (double)duration,
                L = consumptionPartialInfo.AllowedConsumption,
                Q = consumptionPartialInfo.DisallowedConsumtion,
                T = (double)(IsDomesticWithoutUnspecified(customerInfo.UsageId) ? customerInfo.DomesticUnitForHousehold : customerInfo.UnitAll),
                Z = (double)customerInfo.ContractualCapacity,
                tags = tagIds?.ToArray()
            };
            object parametersDisallwed = new
            {
                X = monthlyAverageConsumption,
                C = c,
                S = olgoo,
                K = (double)zoneMultiplier,
                K1 = (double)(zoneMultiplier2 * allowedKModifier),
                D = (double)duration,
                L = consumptionPartialInfo.AllowedConsumption,
                Q = consumptionPartialInfo.DisallowedConsumtion,
                T = (double)(IsDomesticWithoutUnspecified(customerInfo.UsageId) ? customerInfo.DomesticUnitForHousehold : customerInfo.UnitAll),
                Z = (double)customerInfo.ContractualCapacity,
                tags = tagIds?.ToArray()
            };
            double allowed = Eval<double>(nerkh.AllowedSewageFormula, parametersAllowed);
            double disallowed = Eval<double>(nerkh.DisallowedSewageFormula, parametersDisallwed);
            return (allowed, disallowed);
        }
        private decimal GetK1(ZaribGetDto zarib, int olgoo, bool isVillage)
        {
            return zarib.Zb3;
        }
        private decimal GetMultiplier(ZaribGetDto zarib, int olgoo, bool isDomestic, bool isVillage, double monthlyConsumption, int branchType)
        {
            decimal rawMultiplier = GetRawMultiplier(zarib, olgoo, isDomestic, isVillage, monthlyConsumption, branchType);
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

            //غیر مسکونی روستایی
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
        private bool IsRural(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, ConsumptionPartialInfo consumptionPartialInfo, double monthlyConsumption, int _olgoo)
        {
            if ((IsVillageDomesticNotConstruction(customerInfo) && !IsRuralButIsMetro(customerInfo)) ||
                IsDolatabadOrHabibabadAndDomesticAndNotConstruction(customerInfo))
            {
                return true;
            }
            return false;
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

        private bool IsDolatabadOrHabibabadAndDomesticAndNotConstruction(CustomerInfoOutputDto customerInfo)
        {
            return IsDolatabadOrHabibabadWithConditionEshtrak(customerInfo.ZoneId, customerInfo.ReadingNumber) &&
                   IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
                   !IsConstruction(customerInfo.BranchType);
        }
        #endregion
    }
}
