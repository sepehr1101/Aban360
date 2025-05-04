using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Domain.Features.Rule.Entties;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.GatewayAdhoc.Features.ConsumersInfo.Contracts;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using org.matheval;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal sealed class TariffCalculationByDetailHandler : BaseCalculator, ITariffCalculationByDetailHandler
    {
        private readonly IIntervalBillPrerequisiteInfoAddHoc _intervalBillPrerequisiteInfoAddHocHandler;
        private readonly ITariffQueryService _tariffQueryService;
        private readonly ITariffConstantQueryService _tariffConstantQueryService;
        private readonly ISubscriptionEventQueryService _eventQueryService;

        public TariffCalculationByDetailHandler(
            IIntervalBillPrerequisiteInfoAddHoc intervalBillPrerequisiteInfoHandler,
            ITariffQueryService tariffQueryService,
            ITariffConstantQueryService tariffConstantQueryService,
            ISubscriptionEventQueryService subscriptionEventQueryService)
        {
            _intervalBillPrerequisiteInfoAddHocHandler = intervalBillPrerequisiteInfoHandler;
            _intervalBillPrerequisiteInfoAddHocHandler.NotNull(nameof(_intervalBillPrerequisiteInfoAddHocHandler));

            _tariffQueryService = tariffQueryService;
            _tariffQueryService.NotNull(nameof(_tariffQueryService));

            _tariffConstantQueryService = tariffConstantQueryService;
            _tariffConstantQueryService.NotNull(nameof(tariffConstantQueryService));

            _eventQueryService = subscriptionEventQueryService;
            _eventQueryService.NotNull(nameof(_eventQueryService));
        }
        public async Task<CaluclationIntervalDiscrepancytWrapper> Handle(TariffByDetailCreateDto testInput, CancellationToken cancellationToken)
        {
            IEnumerable<IntervalBillSubscriptionInfo> infos = await _intervalBillPrerequisiteInfoAddHocHandler.Handle(testInput.ZoneId, testInput.FromDate, testInput.ToDate, testInput.UsageId, testInput.IndividualTypeId, testInput.Handover, testInput.FromReadingNumber, testInput.ToReadingNumber,cancellationToken);
            IEnumerable<EventsSummaryDto> eventInfos = await _eventQueryService.GetBillDto(testInput.ZoneId, testInput.FromReadingNumber, testInput.ToReadingNumber);
            ICollection<CaluclationIntervalDiscrepancy> discrepancies = new List<CaluclationIntervalDiscrepancy>();
            foreach (IntervalBillSubscriptionInfo info in infos)
            {
                var eventInfo = eventInfos.FirstOrDefault(e => e.BillId == info.BillId.Trim());
                if (eventInfo is not null)
                {
                    var amount = await CalculateOne(eventInfo.PreviousMeterDate, eventInfo.CurrentMeterDate, eventInfo.PreviousMeterNumber.Value, eventInfo.NextMeterNumber.Value, info);
                    var discrepency = new CaluclationIntervalDiscrepancy() { Amount = Convert.ToInt64(amount), BillId = info.BillId, CustomerNumber = 0, FromReadingDate = eventInfo.PreviousMeterDate, FromWaterMeterNumber = eventInfo.PreviousMeterNumber.Value, ToWaterMeterNumber = eventInfo.NextMeterNumber.Value };
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

        private async Task<double> CalculateOne(string @from, string @to, int previousNumber, int currentNumber, IntervalBillSubscriptionInfo info)
        {
            string previousReadingDate = from;
            string currentReadingDate = to;
            int consumption = GetConsumption(previousNumber, currentNumber);
            int duration = GetDuration(previousReadingDate, currentReadingDate);
            double average = GetDailyConsumptionAverage(consumption, duration);

            ICollection<Tariff> rawTariffs = await GetRawTariffs(previousReadingDate, currentReadingDate);
            ICollection<Tariff> tariffs = GetTariffs(rawTariffs, average, previousReadingDate, currentReadingDate);
            List<IntervalCalculationResult> intervalCalculationResults = CreateCalculationResult(info, tariffs);
            double amount = intervalCalculationResults.Sum(x => x.Amount);
            return amount;
        }
        private List<IntervalCalculationResult> CreateCalculationResult(IntervalBillSubscriptionInfo info, ICollection<Tariff> tariffs)
        {
            List<IntervalCalculationResult> intervalCalculationResults = new List<IntervalCalculationResult>();
            foreach (var tariff in tariffs)
            {
                Expression expressionCondition = GetExpression(tariff.Condition, info);
                bool conditionSatisfied = expressionCondition.Eval<bool>();
                if (!conditionSatisfied)
                {
                    continue;
                }
                Expression expressionFormula = GetExpression(tariff.Formula, info);
                double result = expressionFormula.Eval<double>();
                intervalCalculationResults.Add(new IntervalCalculationResult(tariff, result));
            }

            return intervalCalculationResults;
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
        private async Task<ICollection<Tariff>> GetRawTariffs(string @from, string @to)
        {
            ICollection<Tariff> allTariffs = await _tariffQueryService.Get(from, to);
            return allTariffs;
        }
        private ICollection<Tariff> GetTariffs(ICollection<Tariff> rawTariffs, double average, string fromDate, string toDate)
        {
            if (rawTariffs == null)
            {
                return new List<Tariff>();
            }
            rawTariffs.First().FromDateJalali = fromDate;
            rawTariffs.Last().ToDateJalali = toDate;
            foreach (Tariff tariff in rawTariffs)
            {
                int duration = GetDuration(tariff.FromDateJalali, tariff.ToDateJalali);
                double consumption = GetConsumption(average, duration);
                tariff.Duration = duration;
                tariff.Consumption = consumption;
            }
            return rawTariffs;
        }
        private async Task<Dictionary<string, object>> GetTariffConstantsDictionary(string @from, string @to, double average, IntervalBillSubscriptionInfo info)
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            ICollection<TariffConstant> tariffConstatns = await _tariffConstantQueryService.Get(from, to);
            if (tariffConstatns is null)
            {
                return keyValuePairs;
            }
            List<IntervalCalculationResult> intervalCalculationResults = new List<IntervalCalculationResult>();
            foreach (var tariffConstant in tariffConstatns)
            {
                Expression expressionCondition = GetExpression(tariffConstant.Condition, info);
                bool conditionSatisfied = expressionCondition.Eval<bool>();
                if (!conditionSatisfied)
                {
                    continue;
                }
                keyValuePairs.Add(tariffConstant.Title, tariffConstant.Key);
            }
            return keyValuePairs;
        }
        private int GetConsumption(int previousNumber, int currentNumber)
        {
            return currentNumber - previousNumber;
        }
        private double GetConsumption(double consumptionDailyAverage, int duration)
        {
            return consumptionDailyAverage * duration;
        }
        private int GetDuration(string previousDate, string currentDate)
        {
            var previousGregorian = previousDate.ToGregorianDateTime();
            var currentGregorian = currentDate.ToGregorianDateTime();
            return (currentGregorian.Value - previousGregorian.Value).Days;
        }
        private double GetDailyConsumptionAverage(int masraf, int duration)
        {
            return (double)masraf / (double)duration;
        }
    }
}
