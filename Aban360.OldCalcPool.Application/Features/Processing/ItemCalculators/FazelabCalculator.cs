using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using static Aban360.Common.Timing.CalculationDistanceDate;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.VirtualCapacityCalculator;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffStringChecker;

namespace Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators
{
    internal interface IFazelabCalculator
    {
        TariffItemResult Calculate(string date1, string date2, int durationAll, CustomerInfoOutputDto customerInfo, double abBahaItemAmount, string currentDateJalali, bool isAbonman, ConsumptionPartialInfo consumptionPartialInfo);
        TariffItemResult CalculateDiscount(TariffItemResult fazelabCalculationResult , double abBahaDiscount, double fazelabAmount, CustomerInfoOutputDto customerInfo, ConsumptionPartialInfo consumptionPartialInfo);
    }

    internal sealed class FazelabCalculator : IFazelabCalculator
    {
        private const string date_1404_02_31 = "1404/02/31";
        private const string _minimumValidDate = "1330/01/01";
        private const int _withoutSewage = 0;
        private const int _firstCalculation = 1;
        private const int _normal = 2;

        public TariffItemResult Calculate(string date1, string date2, int durationAll, CustomerInfoOutputDto customerInfo, double abBahaItemAmount, string currentDateJalali, bool isAbonman, ConsumptionPartialInfo consumptionPartialInfo)
        {
            double sewageAmount = 0;

            //محاسبه کارمزد دفع در کاربری های گروه خانگی ضریب 0.7
            double multiplier = GetMultiplier(isAbonman, customerInfo.UsageId);
           
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
            if (customerInfo.SewageCalcState == _withoutSewage)
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
                //TODO: Update SewageStateToNormal in DB
                return new TariffItemResult(consumptionPartialInfo.AllowedRatio* sewageAmount, consumptionPartialInfo.DisallwedRatio*sewageAmount);
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
                return new TariffItemResult(consumptionPartialInfo.AllowedRatio* sewageAmount, consumptionPartialInfo.DisallwedRatio*sewageAmount);
            }
            else if (IsTotallyNormal(customerInfo, currentDateJalali))
            {
                sewageAmount = abBahaItemAmount * multiplier;
            }
            return new TariffItemResult(consumptionPartialInfo.AllowedRatio * sewageAmount, consumptionPartialInfo.DisallwedRatio * sewageAmount);
        }
        public TariffItemResult CalculateDiscount(TariffItemResult fazelabCalculationResult, double abBahaDiscount, double fazelabAmount, CustomerInfoOutputDto customerInfo, ConsumptionPartialInfo consumptionPartialInfo)
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
            if (date_1404_02_31.MoreOrEq(consumptionPartialInfo.EndDateJalali) && IsSchool(customerInfo.UsageId))
            {
                return new TariffItemResult();
            }
            double fazelabDiscount = abBahaDiscount * GetMultiplier(false,customerInfo.UsageId);
            double virtualDiscount = CalculateDiscountByVirtualCapacity(customerInfo, consumptionPartialInfo.Consumption, consumptionPartialInfo.Duration, fazelabDiscount);
            double finalDiscount= virtualDiscount > 0 ? virtualDiscount : fazelabDiscount;//fazelabAmount
            return new TariffItemResult(finalDiscount);
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
        #endregion
    }
}
