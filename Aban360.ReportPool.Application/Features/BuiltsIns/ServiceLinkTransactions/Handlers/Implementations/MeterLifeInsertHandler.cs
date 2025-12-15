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
            result.NotNullNorEmpty(nameof(result));
            IEnumerable<MeterLifeCalculationOutputDto> meterLif = CalcDistance(result);
            meterLif.NotNullNorEmpty(nameof(meterLif));
            await _meterLifeService.Insert(meterLif);
        }
        private IEnumerable<MeterLifeCalculationOutputDto> CalcDistance(IEnumerable<MeterLifeCalculationOutputDto> input)
        {
            return input.Select(meterlifeOutputDto =>
            {
                string maxDate = GetMaxDate(meterlifeOutputDto.WaterInstallationDateJalali, meterlifeOutputDto.LatestChangeDateJalali);
                CalcDistanceResultDto calc = CalculationDistanceDate.CalcDistance(maxDate);

                return meterlifeOutputDto with
                {
                    LifeInDay = calc.HasError ? -1 : calc.Distance,
                    LifeText = calc.DistanceText
                };
            });
        }
        private string GetMaxDate(string? firstDate, string? secondDate)
        {           
            return firstDate.CompareTo(secondDate) >= 0 ? firstDate : secondDate;
        }
    }
}
