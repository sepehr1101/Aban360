using Aban360.Common.Exceptions;
using Aban360.OldCalcPool.Application.Constant;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using DNTPersianUtils.Core;
using static Aban360.OldCalcPool.Application.Features.Processing.Helpers.TariffRuleChecker;

namespace Aban360.OldCalcPool.Application.Features.Processing.Helpers
{
    internal interface IConsumptionCalculator
    {
        ConsumptionInfo GetConsumptionInfo(MeterInfoOutputDto meterInfo, CustomerInfoOutputDto customerInfo);
        ConsumptionInfo GetConsumptionInfoWithMonthlyConsumption(MeterDateInfoWithMonthlyConsumptionOutputDto meterInfo, CustomerInfoOutputDto customerInfo);
    }
    internal sealed class ConsumptionCalculator : IConsumptionCalculator
    {
        public ConsumptionInfo GetConsumptionInfo(MeterInfoOutputDto meterInfo, CustomerInfoOutputDto customerInfo)
        {
            int consumption = 0;
            if (meterInfo.CounterStateCode.HasValue)
            {
                if (meterInfo.CounterStateCode.Value == 3)//معکوس
                {
                    consumption = GetConsumption(meterInfo.CurrentNumber, meterInfo.PreviousNumber);
                }
                else if (meterInfo.CounterStateCode.Value == 5)//دور مجدد
                {
                    consumption = GetRoundAgainConsumption(meterInfo.PreviousNumber, meterInfo.CurrentNumber);
                }
                else
                {
                    consumption = GetConsumption(meterInfo.PreviousNumber, meterInfo.CurrentNumber);
                }
            }
            else
            {
                consumption = GetConsumption(meterInfo.PreviousNumber, meterInfo.CurrentNumber);
            }
            int duration = GetDuration(meterInfo.PreviousDateJalali, meterInfo.CurrentDateJalali);
            int finalDomesticUnit = GetFinalDomesticUnit(customerInfo, meterInfo.CurrentDateJalali);
            double dailyAverage = GetDailyConsumptionAverage(consumption, duration, finalDomesticUnit);
            ConsumptionInfo consumptionInfo = new(meterInfo.PreviousDateJalali, meterInfo.CurrentDateJalali, consumption, duration, dailyAverage, finalDomesticUnit);
            return consumptionInfo;
        }
        public ConsumptionInfo GetConsumptionInfoWithMonthlyConsumption(MeterDateInfoWithMonthlyConsumptionOutputDto meterInfo, CustomerInfoOutputDto customerInfo)
        {
            int duration = GetDuration(meterInfo.PreviousDateJalali, meterInfo.CurrentDateJalali);
            int finalDomesticUnit = GetFinalDomesticUnit(customerInfo, meterInfo.CurrentDateJalali);
            double dailyAverage = meterInfo.MonthlyAverageConsumption / 30;
            int consumption = GetConsumption(dailyAverage, duration, finalDomesticUnit);
            ConsumptionInfo consumptionInfo = new(meterInfo.PreviousDateJalali, meterInfo.CurrentDateJalali, consumption, duration, dailyAverage, finalDomesticUnit);
            return consumptionInfo;
        }

        private int GetConsumption(int previousNumber, int currentNumber)
        {
            return currentNumber - previousNumber;
        }
        private int GetRoundAgainConsumption(int previousNumber, int currentNumber)
        {
            int characterCount = previousNumber.ToString().Length;
            int maximalCounter = int.Parse(new string('9', characterCount));
            int maxNumber = maximalCounter - previousNumber;

            return maxNumber + currentNumber;
        }
        private int GetDuration(string previousDate, string currentDate)
        {
            int thresholdDay = 4;
            var previousGregorian = previousDate.ToGregorianDateTime();
            var currentGregorian = currentDate.ToGregorianDateTime();
            int duration = (currentGregorian.Value - previousGregorian.Value).Days;
            if (duration <= thresholdDay)
            {
                throw new InvalidBillIdException(Literals.InvalidDuration);
            }
            return duration;
        }
        private double GetDailyConsumptionAverage(int masraf, int duration, int domesticUnit)
        {
            return masraf / (double)duration / domesticUnit;
        }
        private int GetConsumption(double dailyCosumption, int duration, int domesticUnit)
        {
            double consumption = (dailyCosumption * duration * domesticUnit);
            return (int)Math.Round(consumption);
        }
        private int GetFinalDomesticUnit(CustomerInfoOutputDto customerInfo, string readingDateJalali)
        {
            if (IsGardenAndResidence(customerInfo.UsageId))
            {
                return customerInfo.OtherUnit + customerInfo.DomesticUnit;
            }
            int finalHousehold = GetHouseholdUnit(customerInfo.HouseholdNumber, customerInfo.HouseholdDate, readingDateJalali);
            if (finalHousehold > 0)
            {
                return finalHousehold;
            }
            return customerInfo.DomesticUnit - customerInfo.EmptyUnit < 1 ? 1 : customerInfo.DomesticUnit - customerInfo.EmptyUnit;
        }
        private int GetHouseholdUnit(int householdUnit, string? householdDate, string readingDateJalali)
        {
            if (householdUnit <= 0)
            {
                return 0;
            }
            if (string.IsNullOrWhiteSpace(householdDate))
            {
                return 0;
            }
            DateTime? expireHouseHoldGregorian = householdDate.ToGregorianDateTime();
            if (!expireHouseHoldGregorian.HasValue)
            {
                return 0;
            }
            DateTime? readingDateGregorian = readingDateJalali.ToGregorianDateTime();
            if (!readingDateGregorian.HasValue)
            {
                throw new InvalidDateException(readingDateJalali);
            }
            if (expireHouseHoldGregorian.Value.AddYears(1) < readingDateGregorian.Value)
            {
                return 0;
            }
            return householdUnit;
        }
    }
}
