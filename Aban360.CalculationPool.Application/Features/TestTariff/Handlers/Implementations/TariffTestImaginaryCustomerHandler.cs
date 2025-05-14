using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Application.Features.TestTariff.Handlers.Contrats;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.TestTariff.Handlers.Implementations
{
    internal sealed class TariffTestImaginaryCustomerHandler : ITariffTestImaginaryCustomerHandler
    {
        private readonly IBaseIntervalTariffEngine _baseIntervalTariffEngine;
        public TariffTestImaginaryCustomerHandler(IBaseIntervalTariffEngine baseIntervalTariffEngine)
        {
            _baseIntervalTariffEngine = baseIntervalTariffEngine;
            _baseIntervalTariffEngine.NotNull(nameof(baseIntervalTariffEngine));
        }
        public async Task<IntervalCalculationResultWrapper> Handle(TariffTestImaginaryInput tariffTestInput, CancellationToken cancellationToken)
        {
            Tuple<ConsumptionInfo, List<IntervalCalculationResult>> intervalCalculationResults = await _baseIntervalTariffEngine.Calculate(tariffTestInput, cancellationToken);
            List<IntervalCalculationResult3> result3 = CreateCalculationResult3(intervalCalculationResults.Item2);
            IntervalCalculationResultWrapper calculationResult = CreateCalculationResultWrapper(intervalCalculationResults.Item1, intervalCalculationResults.Item2, result3);
            return calculationResult;
        }

        private IntervalCalculationResultWrapper CreateCalculationResultWrapper(ConsumptionInfo consumptionInfo, List<IntervalCalculationResult> intervalCalculationResults, List<IntervalCalculationResult3> result3)
        {
            IntervalCalculationResultWrapper calculationResult = new(consumptionInfo.Consumption, consumptionInfo.Duration, consumptionInfo.AverageConsumption, consumptionInfo.PreviousReadingDate, consumptionInfo.CurrentReadingDate);
            calculationResult.IntervalCalculationResults = result3;
            calculationResult.IntervalCount = intervalCalculationResults.Count;
            calculationResult.Amount = intervalCalculationResults.Sum(i => i.Amount);
            return calculationResult;
        }
        private List<IntervalCalculationResult3> CreateCalculationResult3(List<IntervalCalculationResult> intervalCalculationResults)
        {
            return intervalCalculationResults
                .GroupBy(item => new { item.FromDate, item.ToDate })
                .Select(grp => new IntervalCalculationResult3
                {
                    FromDate = grp.Key.FromDate,
                    ToDate = grp.Key.ToDate,
                    CalculationInfo = grp.Select(g => new IntervalCalculationResult2()
                    {
                        Amount = g.Amount,
                        Consumption = g.Consumption,
                        Duration = g.Duration,
                        Formula = g.Formula,
                        LineItemTypeTitle = g.LineItemTypeTitle,
                        OfferingTitle = g.OfferingTitle
                    }).ToList()
                }).ToList();
        }
    }
}
