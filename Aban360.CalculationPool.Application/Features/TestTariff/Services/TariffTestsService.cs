using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.GatewayAdhoc.Features.ConsumersInfo.Contracts;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.TestTariff.Services;
public interface ITariffTestsService
{
    Task<CaluclationIntervalDiscrepancytWrapper> TariffTestBatchAsync(CaluclationIntervalBatchTestInput testInput, CancellationToken cancellationToken);
    Task<IntervalCalculationResultWrapper> TariffTestImaginaryCustomerAsync(TariffTestImaginaryInput tariffTestInput, CancellationToken cancellationToken);
    Task<IntervalCalculationResultWrapper> TariffTestSingleCustomerAsync(TariffTestInput tariffTestInput, CancellationToken cancellationToken);
}

internal sealed class TariffTestsService : ITariffTestsService
{
    private readonly IBaseIntervalTariffEngine _baseIntervalTariffEngine;
    private readonly IIntervalBillPrerequisiteInfoAddHoc _intervalBillPrerequisiteInfoAddHocHandler;
    private readonly ISubscriptionEventQueryService _eventQueryService;
    public TariffTestsService(IBaseIntervalTariffEngine baseIntervalTariffEngine, IIntervalBillPrerequisiteInfoAddHoc intervalBillPrerequisiteInfoHandler, ISubscriptionEventQueryService subscriptionEventQueryService)
    {
        _baseIntervalTariffEngine = baseIntervalTariffEngine;
        _baseIntervalTariffEngine.NotNull(nameof(baseIntervalTariffEngine));
        _intervalBillPrerequisiteInfoAddHocHandler = intervalBillPrerequisiteInfoHandler;
        _intervalBillPrerequisiteInfoAddHocHandler.NotNull(nameof(_intervalBillPrerequisiteInfoAddHocHandler));
        _eventQueryService = subscriptionEventQueryService;
        _eventQueryService.NotNull(nameof(_eventQueryService));
    }

    public async Task<CaluclationIntervalDiscrepancytWrapper> TariffTestBatchAsync(CaluclationIntervalBatchTestInput testInput, CancellationToken cancellationToken)
    {
        IEnumerable<IntervalBillSubscriptionInfo> infos = await _intervalBillPrerequisiteInfoAddHocHandler.Handle(testInput.ZoneId, testInput.RegisterDate, testInput.FormReadingNumber, testInput.ToReadingNumber, cancellationToken);
        IEnumerable<WaterEventsSummaryOutputDataDto> eventInfos = await _eventQueryService.GetBillDto(testInput.ZoneId, testInput.RegisterDate, testInput.FormReadingNumber, testInput.ToReadingNumber);
        ICollection<CaluclationIntervalDiscrepancy> discrepancies = new List<CaluclationIntervalDiscrepancy>();
        foreach (IntervalBillSubscriptionInfo info in infos)
        {
            var eventInfo = eventInfos.FirstOrDefault(e => e.BillId == info.BillId.Trim());
            if (eventInfo is not null)
            {
                double amount = await TariffTestBatchGetAmount(info, eventInfo, cancellationToken);
                var discrepency = new CaluclationIntervalDiscrepancy()
                {
                    Amount = Convert.ToInt64(amount),
                    BillId = info.BillId,
                    CustomerNumber = 0,
                    FromReadingDate = eventInfo.PreviousMeterDate,
                    FromWaterMeterNumber = eventInfo.PreviousMeterNumber.Value,
                    ToWaterMeterNumber = eventInfo.NextMeterNumber.Value
                };
                discrepancies.Add(discrepency);
            }
        }

        var res = new CaluclationIntervalDiscrepancytWrapper()
        {
            CurrentSystemSum = discrepancies.Sum(d => d.Amount),
            DifferenceSum = 56997,
            PreviousSystemSum = eventInfos.Sum(e => e.DebtAmount.Value),
            DiscrepancyDetails = discrepancies
        };
        res.DifferenceSum = res.CurrentSystemSum - res.PreviousSystemSum;
        return res;
    }

    private async Task<double> TariffTestBatchGetAmount(IntervalBillSubscriptionInfo info, WaterEventsSummaryOutputDataDto? eventInfo, CancellationToken cancellationToken)
    {
        var tariffTestInput = new TariffTestInput()
        {
            BillId = info.BillId,
            CurrentReadingDate = eventInfo.CurrentMeterDate,
            CurrentReadingNumber = eventInfo.PreviousMeterNumber.Value,
            PreviousReadingDate = eventInfo.PreviousMeterDate,
            PreviousReadingNumber = eventInfo.NextMeterNumber.Value
        };
        Tuple<ConsumptionInfo, List<IntervalCalculationResult>> intervalCalculationResults = await _baseIntervalTariffEngine.Calculate(tariffTestInput, cancellationToken);
        double amount = intervalCalculationResults.Item2.Sum(x => x.Amount);
        return amount;
    }

    public async Task<IntervalCalculationResultWrapper> TariffTestImaginaryCustomerAsync(TariffTestImaginaryInput tariffTestInput, CancellationToken cancellationToken)
    {
        Tuple<ConsumptionInfo, List<IntervalCalculationResult>> intervalCalculationResults = await _baseIntervalTariffEngine.Calculate(tariffTestInput, cancellationToken);
        List<IntervalCalculationResult3> result3 = TariffTestImaginaryCustomerCreateCalculationResult3(intervalCalculationResults.Item2);
        IntervalCalculationResultWrapper calculationResult = TariffTestImaginaryCustomerCreateCalculationResultWrapper(intervalCalculationResults.Item1, intervalCalculationResults.Item2, result3);
        return calculationResult;
    }

    private IntervalCalculationResultWrapper TariffTestImaginaryCustomerCreateCalculationResultWrapper(ConsumptionInfo consumptionInfo, List<IntervalCalculationResult> intervalCalculationResults, List<IntervalCalculationResult3> result3)
    {
        IntervalCalculationResultWrapper calculationResult = new(consumptionInfo.Consumption, consumptionInfo.Duration, consumptionInfo.DailyAverageConsumption, consumptionInfo.PreviousReadingDate, consumptionInfo.CurrentReadingDate);
        calculationResult.IntervalCalculationResults = result3;
        calculationResult.IntervalCount = intervalCalculationResults.Count;
        calculationResult.Amount = intervalCalculationResults.Sum(i => i.Amount);
        return calculationResult;
    }

    private List<IntervalCalculationResult3> TariffTestImaginaryCustomerCreateCalculationResult3(List<IntervalCalculationResult> intervalCalculationResults)
    {
        return intervalCalculationResults.GroupBy(item => new { item.FromDate, item.ToDate }).Select(grp => new IntervalCalculationResult3 { FromDate = grp.Key.FromDate, ToDate = grp.Key.ToDate, CalculationInfo = grp.Select(g => new IntervalCalculationResult2() { Amount = g.Amount, Consumption = g.Consumption, Duration = g.Duration, Formula = g.Formula, LineItemTypeTitle = g.LineItemTypeTitle, OfferingTitle = g.OfferingTitle }).ToList() }).ToList();
    }

    public async Task<IntervalCalculationResultWrapper> TariffTestSingleCustomerAsync(TariffTestInput tariffTestInput, CancellationToken cancellationToken)
    {
        Tuple<ConsumptionInfo, List<IntervalCalculationResult>> intervalCalculationResults = await _baseIntervalTariffEngine.Calculate(tariffTestInput, cancellationToken);
        List<IntervalCalculationResult3> result3 = TariffTestSingleCustomerCreateCalculationResult3(intervalCalculationResults.Item2);
        IntervalCalculationResultWrapper calculationResult = TariffTestSingleCustomerCreateCalculationResultWrapper(intervalCalculationResults.Item1, intervalCalculationResults.Item2, result3);
        return calculationResult;
    }

    private IntervalCalculationResultWrapper TariffTestSingleCustomerCreateCalculationResultWrapper(ConsumptionInfo consumptionInfo, List<IntervalCalculationResult> intervalCalculationResults, List<IntervalCalculationResult3> result3)
    {
        IntervalCalculationResultWrapper calculationResult = new(consumptionInfo.Consumption, consumptionInfo.Duration, consumptionInfo.DailyAverageConsumption, consumptionInfo.PreviousReadingDate, consumptionInfo.CurrentReadingDate);
        calculationResult.IntervalCalculationResults = result3;
        calculationResult.IntervalCount = intervalCalculationResults.Count;
        calculationResult.Amount = intervalCalculationResults.Sum(i => i.Amount);
        return calculationResult;
    }

    private List<IntervalCalculationResult3> TariffTestSingleCustomerCreateCalculationResult3(List<IntervalCalculationResult> intervalCalculationResults)
    {
        return intervalCalculationResults.GroupBy(item => new { item.FromDate, item.ToDate }).Select(grp => new IntervalCalculationResult3 { FromDate = grp.Key.FromDate, ToDate = grp.Key.ToDate, CalculationInfo = grp.Select(g => new IntervalCalculationResult2() { Amount = g.Amount, Consumption = g.Consumption, Duration = g.Duration, Formula = g.Formula, LineItemTypeTitle = g.LineItemTypeTitle, OfferingTitle = g.OfferingTitle }).ToList() }).ToList();
    }
}
