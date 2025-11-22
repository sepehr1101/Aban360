using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.OldCalcPool.Application.Features.Processing.ItemCalculators
{
    internal interface ITaxCalculator
    {
        TariffItemResult Calculate(params double[] amounts);
        TariffItemResult CalculateDiscount(params double[] amounts);
    }
    internal sealed class TaxCalculator: ITaxCalculator
    {
        const float vatRate = 0.1f;
        public TariffItemResult Calculate(params double[] amounts)
        {
            return new TariffItemResult(amounts.Sum() * vatRate);
        }
        public TariffItemResult CalculateDiscount(params double[] amounts)
        {
            return new TariffItemResult(amounts.Sum() * vatRate);
        }
    }
}
