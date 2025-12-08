using Aban360.Common.Extensions;
using Aban360.Common.Timing;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using static Aban360.Common.Timing.CalculationDistanceDate;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class MeterLifeInsertHandler : IMeterLifeInsertHandler
    {
        private readonly IMeterLifeService _meterLifeService;
        public MeterLifeInsertHandler(IMeterLifeService meterLifeService)
        {
            _meterLifeService = meterLifeService;
            _meterLifeService.NotNull(nameof(meterLifeService));
        }

        public async Task Handle(CancellationToken cancellationToken)
        {
            await _meterLifeService.TruncateTable();
            IEnumerable<MeterLifeCalculationOutputDto> result = await _meterLifeService.GetFromClient();
            result.NotNull(nameof(result));
            IEnumerable<MeterLifeCalculationOutputDto> meterLif = CalcDistance(result);
            meterLif.NotNull(nameof(meterLif));
            await _meterLifeService.Insert(meterLif);
        }
        private IEnumerable<MeterLifeCalculationOutputDto> CalcDistance(IEnumerable<MeterLifeCalculationOutputDto> input)
        {
            input.ForEach(x =>
            {
                string maxDate = GetMaxDate(x.WaterInstallationDateJalali, x.LatestChangeDateJalali);
                CalcDistanceResultDto calcDistance = CalculationDistanceDate.CalcDistance(maxDate);

                x.LifeInDay = calcDistance.HasError ? -1 : calcDistance.Distance;
                x.LifeText = calcDistance.DistanceText;
            });
            return input;
        }
        private string GetMaxDate(string? firstDate, string? secondDate)
        {
            //if (string.IsNullOrWhiteSpace(firstDate) && string.IsNullOrWhiteSpace(secondDate))
            //{
            //    throw new InvalidDateException(string.Empty);
            //}
            //if (string.IsNullOrWhiteSpace(firstDate))
            //{
            //    return secondDate;
            //}
            //if (string.IsNullOrEmpty(secondDate))
            //{
            //    return firstDate;
            //}
            return firstDate.CompareTo(secondDate) >= 0 ? firstDate : secondDate;
        }
    }
}
