using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using static Aban360.Common.Timing.CalculationDistanceDate;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.VirtualCapacityCalculator;

namespace Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators
{
    internal interface IFazelabCalculator
    {
        double Calculate(string date1, string date2, int durationAll, CustomerInfoOutputDto customerInfo, double abBahaItemAmount, string currentDateJalali, bool isAbonman);
        double CalculateDiscount(double abBahaDiscount, double fazelabAmount, CustomerInfoOutputDto customerInfo, NerkhGetDto nerkh);
    }

    internal sealed class FazelabCalculator : IFazelabCalculator
    {
        private const string _minimumValidDate = "1330/01/01";
        private const int _withoutSewage = 0;
        private const int _firstCalculation = 1;
        private const int _normal = 2;

        public double Calculate(string date1, string date2, int durationAll, CustomerInfoOutputDto customerInfo, double abBahaItemAmount, string currentDateJalali, bool isAbonman)
        {
            double sewageAmount = 0;
            //محاسبه کارمزد دفع در کاربری های گروه خانگی ضریب 0.7
            double multiplier = GetMultiplier(isAbonman, customerInfo.UsageId);
           
            if (IsConstruction(customerInfo.BranchType) && !isAbonman)
            {
                return 0;
            }
            if (customerInfo.SewageCalcState == _withoutSewage)
            {
                return 0;
            }
            // ثبت نصب بعد از تاریخ قرائت لحاظ شده
            if (IsInstallAfterReading(date2, customerInfo))
            {
                return 0;
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
                return sewageAmount;
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
                return sewageAmount;
            }
            else if (IsTotallyNormal(customerInfo, currentDateJalali))
            {
                sewageAmount = abBahaItemAmount * multiplier;
            }
            return sewageAmount;
        }
        public double CalculateDiscount(double abBahaDiscount, double fazelabAmount, CustomerInfoOutputDto customerInfo, NerkhGetDto nerkh)
        {
            if (abBahaDiscount <= 0)
            {
                return 0;
            }
            if (fazelabAmount <= 0)
            {
                return 0;
            }

            double fazelabDiscount = abBahaDiscount * GetMultiplier(false,customerInfo.UsageId);
            double virtualDiscount = CalculateDiscountByVirtualCapacity(customerInfo, nerkh.PartialConsumption, nerkh.Duration, fazelabDiscount);
            return virtualDiscount > 0 ? virtualDiscount : fazelabDiscount;//fazelabAmount
        }


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
    }
}
