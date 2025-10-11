using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    internal class LatestWaterMeterInfoGetHandler : ILatestWaterMeterInfoGetHandler
    {
        private readonly ILatestWaterMeterInfoQueryService _latestWaterMeterInfoQuery;
        private const float olgoo = 9.33f;
        public LatestWaterMeterInfoGetHandler(ILatestWaterMeterInfoQueryService latestWaterMeterInfoQuery)
        {
            _latestWaterMeterInfoQuery = latestWaterMeterInfoQuery;
            _latestWaterMeterInfoQuery.NotNull(nameof(latestWaterMeterInfoQuery));
        }

        public async Task<LatestWaterMeterInfoDto> Handle(string billId, CancellationToken cancellationToken)
        {
            LatestWaterMeterInfoDto latestWaterMeterInfo = await _latestWaterMeterInfoQuery.GetInfo(billId);
            latestWaterMeterInfo.MeterLife = GetDistance(latestWaterMeterInfo.MeterReplacementDate, latestWaterMeterInfo.WaterInstallationDateJalali);

            latestWaterMeterInfo.PossibilityEmptyUnit = GetPossibilityEmptyUnit(latestWaterMeterInfo.ConsumptionAverage, latestWaterMeterInfo.DomesticUnit, latestWaterMeterInfo.UsageId);

            return latestWaterMeterInfo;
        }
        private string GetDistance(string latestMeterChange, string waterInstallDate)
        {
            int? distance = CalculationDistanceDate.CalcDistance(string.IsNullOrWhiteSpace(latestMeterChange) ? waterInstallDate : latestMeterChange);
            return distance.HasValue ? CalculationDistanceDate.ConvertDaysToDate(distance.Value) : ExceptionLiterals.Incalculable;
        }
        private bool GetPossibilityEmptyUnit(float consumptionAverage, int domesticUnit, int usageId)
        {
            if (usageId != 1 && usageId != 3)
                return false;

            if ((consumptionAverage / domesticUnit) < olgoo)
                return true;

            return false;
        }

    }
}
