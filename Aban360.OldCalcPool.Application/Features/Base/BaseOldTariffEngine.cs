using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;

namespace Aban360.CalculationPool.Application.Features.Base
{
    internal class BaseOldTariffEngine : BaseExpressionCalculator
    {
        //todo: Do not use 'BaseException' directy

        /// <summary>
        /// تنها تابع با دسترسی پابلیک بابت محاسبه تک رکورد جدول نرخ
        /// </summary>
        /// <returns>مقدار خروجی بعد از اتمام نوشتن کد، اصلاح شود</returns>
        public object CalculateWaterBill()
        {
            //object parameters= new {X=consumptionAverage};
            //long _value = expression.Eval<long>();
            throw new NotImplementedException();
        }

        private ConsumptionInfo GetConsumptionInfo()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// از خلاصه نویسی استفاده شود
        /// </summary>
        /// <returns>عدد محاسبه شده‌ی آب‌بها</returns>
        private long CalculateAbBaha()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private long CalculateAbBahaDiscount(/*long abBaha... other parameters*/)
        {
            throw new NotImplementedException();
        }

        private long CalculateBoodjePart1()
        {
            throw new NotImplementedException();
        }
        private (long,long) CalculateBoodjePart1Discount()
        {
            throw new NotImplementedException();
        }

        private long CalculateBoodjePart2()
        {
            throw new NotImplementedException();
        }
        private (long, long) CalculateBoodjePart2Discount()
        {
            throw new NotImplementedException();
        }

        private long CalculateFazelab()
        {
            throw new NotImplementedException();
        }
        private long CalculateFazelabDiscount()
        {
            throw new NotImplementedException();
        }
        private long CalculateAbonmanAb()
        {
            throw new NotImplementedException();
        }
        private long CalculateAbonmanAbDiscount()
        {
            throw new NotImplementedException();
        }

        private long CalculateAbonmanFazelab()
        {
            throw new NotImplementedException();
        }
        private long CalculateAbonmanFazelabDiscount()
        {
            throw new NotImplementedException();
        }

        private long CalculateJavaniJamiat()
        {
            throw new NotImplementedException();
        }
        private long CalculateJavaniJamiatDiscount()
        {
             throw new NotImplementedException();
        }

        private long CalculateAvarez()
        {
            throw new NotImplementedException();
        }
        private long CalculateAvarezDiscount()
        {
            throw new NotImplementedException();
        }

        private long CalculateFasleGarm()
        {
            throw new NotImplementedException();
        }
        private long CalculateFasleGarmDiscount()
        {
            throw new NotImplementedException();
        }
    }
}
