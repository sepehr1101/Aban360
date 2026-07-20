using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class MeterReadingValidateHandler : IMeterReadingValidateHandler
    {
        const int _consumptionLimit = 50;
        const int _domesticConsumptionLimitPercent = 30;
        const int _nonDomesticConsumptionLimitPercent = 30;
        const long _dailyDomesticAmount = 700_000;
        const long _dailyNonDomesticAmount = 1_000_000;
        int[] _misReadCounterStateCode = [4, 7, 8];
        int[] _specialCounterStateCode = [1, 2, 3, 5];
        public HighLowEnum GetAttentionState(MeterReadingDetailDataOutputDto meterReading, MeterFlowStepEnum latestFlowStep)
        {
            return latestFlowStep switch
            {
                MeterFlowStepEnum.Calculated => ConsumptionAttention(meterReading),
                MeterFlowStepEnum.ConsumptionChecked => GetAmountAttention(meterReading),
                _ => throw new ReadingException(ExceptionLiterals.InvalidFlowStep)
            };
        }
        public bool IsAttentionCounterState(int counterStateCode) => _specialCounterStateCode.Contains(counterStateCode);
        private HighLowEnum GetAmountAttention(MeterReadingDetailDataOutputDto input)
        {
            if (_misReadCounterStateCode.Contains(input.CurrentCounterStateCode)) return HighLowEnum.Normal;
            if (input.SumItems == 0) return HighLowEnum.Zero;

            int totalUnit = input.DomesticUnit + input.CommercialUnit + input.OtherUnit;
            int duration = (int)(!input.Modat.HasValue || input.Modat <= 0 ? 1 : input.Modat);
            double perUnitAmount = input.SumItems.Value / (totalUnit == 0 ? 1 : totalUnit);
            double dailyPerUnitAmount = perUnitAmount / duration;

            if (IsDomestic(input.UsageId) && dailyPerUnitAmount > _dailyDomesticAmount) return HighLowEnum.High;
            if (!IsDomestic(input.UsageId) && dailyPerUnitAmount > _dailyNonDomesticAmount) return HighLowEnum.High;

            return HighLowEnum.Normal;
        }
        private HighLowEnum ConsumptionAttention(MeterReadingDetailDataOutputDto input)
        {
            if (IsDomestic(input.UsageId))
            {
                return GetConsumptionAttention(_domesticConsumptionLimitPercent, input.LastMonthlyConsumption.Value, input.MonthlyConsumption.Value, input.CurrentCounterStateCode, input.UsageId);
            }
            else
            {
                return GetConsumptionAttention(_nonDomesticConsumptionLimitPercent, input.ContractualCapacity, input.MonthlyConsumption.Value, input.CurrentCounterStateCode, input.UsageId);
            }
        }
        private HighLowEnum GetConsumptionAttention(int limitPercent, double previousItem, double consumption, int counterStateCode, int usageId)
        {
            if (_misReadCounterStateCode.Contains(counterStateCode)) return HighLowEnum.Normal;
            if (consumption == 0) return HighLowEnum.Zero;

            double limitValue = (double)(previousItem * limitPercent / 100d);
            double minValue = (double)(previousItem - limitValue);
            double maxValue = (double)(previousItem + limitValue);

            if (consumption < minValue) return HighLowEnum.Low;
            if (consumption > maxValue) return HighLowEnum.High;
            if (IsDomestic(usageId) && consumption > _consumptionLimit) return HighLowEnum.High;
            return HighLowEnum.Normal;
        }
        private bool IsDomestic(int usageId)
        {
            int[] domesticUsage = [1, 3];
            return domesticUsage.Contains(usageId);
        }
    }
}
