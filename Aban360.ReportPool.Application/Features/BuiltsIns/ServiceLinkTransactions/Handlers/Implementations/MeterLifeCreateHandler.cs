using Aban360.Common.Extensions;
using Aban360.Common.Timing;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using static Aban360.Common.Timing.CalculationDistanceDate;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class MeterLifeCreateHandler : IMeterLifeCreateHandler
    {
        private readonly IMeterLifeService _meterLifeService;
        public MeterLifeCreateHandler(IMeterLifeService meterLifeService)
        {
            _meterLifeService = meterLifeService;
            _meterLifeService.NotNull(nameof(meterLifeService));
        }

        public async Task Handle(CancellationToken cancellationToken)
        {
            IEnumerable<MeterLifeCalculationOutputDto> result = await _meterLifeService.GetFromClient();
            IEnumerable<MeterLifeCalculationOutputDto> meterLif = await CalcDistance(result);
            await _meterLifeService.Create(meterLif);
        }
        private async Task<IEnumerable<MeterLifeCalculationOutputDto>> CalcDistance(IEnumerable<MeterLifeCalculationOutputDto> input)
        {
            input.ForEach(x =>
            {
                string maxDate = GetMaxDate(x.WaterInstallationDateJalali, x.LatestChangeDataJalali);
                CalcDistanceResultDto calcDistance = CalculationDistanceDate.CalcDistance(maxDate);

                x.LifeInDay = calcDistance.HasError ? -1 : calcDistance.Distance;
                x.LifeText = calcDistance.DistanceText;
            });
            return input;
        }
        private string GetMaxDate(string firstDate, string? secondDate)
        {
            return firstDate.CompareTo(secondDate) >= 0 ? firstDate : secondDate;
        }
    }
}
