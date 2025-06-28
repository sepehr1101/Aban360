using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Application.Features.TestTariff.Handlers.Contrats;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.GatewayAdhoc.Features.ConsumersInfo.Contracts;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;

namespace Aban360.CalculationPool.Application.Features.TestTariff.Handlers.Implementations
{
    internal sealed class TariffTestBatchHandler : ITariffTestBatchHandler
    {
        private readonly IBaseIntervalTariffEngine _baseIntervalTariffEngine;
        private readonly IIntervalBillPrerequisiteInfoAddHoc _intervalBillPrerequisiteInfoAddHocHandler;          
        private readonly ISubscriptionEventQueryService _eventQueryService;

        public TariffTestBatchHandler(IBaseIntervalTariffEngine baseIntervalTariffEngine,
            IIntervalBillPrerequisiteInfoAddHoc intervalBillPrerequisiteInfoHandler,       
            ISubscriptionEventQueryService subscriptionEventQueryService)
        {
            _baseIntervalTariffEngine = baseIntervalTariffEngine;
            _baseIntervalTariffEngine.NotNull(nameof(baseIntervalTariffEngine));

            _intervalBillPrerequisiteInfoAddHocHandler = intervalBillPrerequisiteInfoHandler;
            _intervalBillPrerequisiteInfoAddHocHandler.NotNull(nameof(_intervalBillPrerequisiteInfoAddHocHandler));

            _eventQueryService = subscriptionEventQueryService;
            _eventQueryService.NotNull(nameof(_eventQueryService));
        }
        public async Task<CaluclationIntervalDiscrepancytWrapper> Handle(CaluclationIntervalBatchTestInput testInput, CancellationToken cancellationToken)
        {
            IEnumerable<IntervalBillSubscriptionInfo> infos = await _intervalBillPrerequisiteInfoAddHocHandler.Handle(testInput.ZoneId, testInput.RegisterDate, testInput.FormReadingNumber, testInput.ToReadingNumber, cancellationToken);
            IEnumerable<EventsSummaryOutputDataDto> eventInfos = await _eventQueryService.GetBillDto(testInput.ZoneId, testInput.RegisterDate, testInput.FormReadingNumber, testInput.ToReadingNumber);
            ICollection<CaluclationIntervalDiscrepancy> discrepancies = new List<CaluclationIntervalDiscrepancy>();
            foreach (IntervalBillSubscriptionInfo info in infos)
            {
                var eventInfo = eventInfos.FirstOrDefault(e => e.BillId == info.BillId.Trim());
                if (eventInfo is not null)
                {
                    double amount = await GetAmount(info, eventInfo, cancellationToken);
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

        private async Task<double> GetAmount(IntervalBillSubscriptionInfo info, EventsSummaryOutputDataDto? eventInfo, CancellationToken cancellationToken)
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
    }
}
