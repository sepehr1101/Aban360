using Aban360.Common.Exceptions;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using DNTPersianUtils.Core;
using Aban360.OldCalcPool.Application.Constant;

namespace Aban360.OldCalcPool.Application.Features.Processing.Helpers
{
    internal interface IConsumptionCalculator
    {
        ConsumptionInfo GetConsumptionInfo(MeterInfoInputDto input, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo);
        ConsumptionInfo GetConsumptionInfo(MeterInfoByPreviousDataInputDto input, CustomerInfoOutputDto customerInfo);
        ConsumptionInfo GetConsumptionInfo(MeterImaginaryInputDto input, CustomerInfoOutputDto customerInfo);
    }
    internal sealed class ConsumptionCalculator: IConsumptionCalculator
    {
        const int thresholdDay = 4;

        public ConsumptionInfo GetConsumptionInfo(MeterInfoInputDto input, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo)
        {
            int consumption = GetConsumption(meterInfo.PreviousNumber, input.CurrentMeterNumber);
            int duration = GetDuration(meterInfo.PreviousDateJalali, input.CurrentDateJalali);
            int finalDomesticUnit = GetFinalDomesticUnit(customerInfo, input.CurrentDateJalali);
            double dailyAverage = GetDailyConsumptionAverage(consumption, duration, finalDomesticUnit);
            ConsumptionInfo consumptionInfo = new(meterInfo.PreviousDateJalali, input.CurrentDateJalali, consumption, duration, dailyAverage, finalDomesticUnit);
            return consumptionInfo;
        }
        public ConsumptionInfo GetConsumptionInfo(MeterInfoByPreviousDataInputDto input, CustomerInfoOutputDto customerInfo)
        {
            int consumption = GetConsumption(input.PreviousNumber, input.CurrentMeterNumber);
            int duration = GetDuration(input.PreviousDateJalali, input.CurrentDateJalali);
            int finalDomesticUnit = GetFinalDomesticUnit(customerInfo, input.CurrentDateJalali);
            double dailyAverage = GetDailyConsumptionAverage(consumption, duration, finalDomesticUnit);
            ConsumptionInfo consumptionInfo = new(input.PreviousDateJalali, input.CurrentDateJalali, consumption, duration, dailyAverage, finalDomesticUnit);
            return consumptionInfo;
        }
        public ConsumptionInfo GetConsumptionInfo(MeterImaginaryInputDto input, CustomerInfoOutputDto customerInfo)
        {
            int consumption = GetConsumption(input.MeterPreviousData.PreviousNumber, input.MeterPreviousData.CurrentMeterNumber);
            int duration = GetDuration(input.MeterPreviousData.PreviousDateJalali, input.MeterPreviousData.CurrentDateJalali);
            int finalDomesticUnit = GetFinalDomesticUnit(customerInfo, input.MeterPreviousData.CurrentDateJalali);
            double dailyAverage = GetDailyConsumptionAverage(consumption, duration, finalDomesticUnit);
            ConsumptionInfo consumptionInfo = new(input.MeterPreviousData.PreviousDateJalali, input.MeterPreviousData.CurrentDateJalali, consumption, duration, dailyAverage, finalDomesticUnit);
            return consumptionInfo;
        }

        private int GetConsumption(int previousNumber, int currentNumber)
        {
            return currentNumber - previousNumber;
        }
        private int GetDuration(string previousDate, string currentDate)
        {
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
        private int GetFinalDomesticUnit(CustomerInfoOutputDto customerInfo, string readingDateJalali)
        {
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
