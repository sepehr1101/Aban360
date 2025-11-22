using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Application.Features.Processing.Validations
{
    internal static class TariffValidator
    {
        const int thresholdDay = 4;
        internal static void ValidatePreviousDate(string previousDateJalali)
        {
            DateOnly? previousDate = previousDateJalali.ToGregorianDateOnly();
            if (!previousDate.HasValue)
            {
                throw new TariffCalcException(ExceptionLiterals.InvalidDate);
            }
            if (previousDate.Value > DateOnly.FromDateTime(DateTime.Now.AddDays(-thresholdDay)))
            {
                throw new TariffCalcException(ExceptionLiterals.InvalidPreviousDateInvoice(thresholdDay));
            }
        }

        internal static void ValidateCounterStateCode(int? counterStateCode, int currentNumber, int previousNumber)
        {                  
            if (counterStateCode.HasValue)
            {
                if (IsValidState(counterStateCode.Value))
                {
                    throw new TariffCalcException(ExceptionLiterals.IncalculableWithCounterStateCode);
                }
                else if (IsChangedOrReverse(counterStateCode.Value) && currentNumber > previousNumber)
                {
                    throw new TariffCalcException(ExceptionLiterals.ConfilictBetweenCounterNumberAndCounteState);
                }
            }
        }
        private static bool IsValidState(int counterStateCode)
        {
            int[] invalidCounterStateCode = [4, 6, 7, 8, 9, 10];
            return invalidCounterStateCode.Contains(counterStateCode);
        }
        private static bool IsChangedOrReverse(int counterStateCode)
        {
            int changeCode = 3;
            int reverseCode = 5;
            return counterStateCode == changeCode || counterStateCode == reverseCode;
        }
    }
}
