using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using DNTPersianUtils.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    internal class BranchSpecificationInfoGetHandler : IBranchSpecificationInfoGetHandler
    {
        private readonly IBranchSpecificationInfoService _BranchSpecificationSummaryInfoService;
        public BranchSpecificationInfoGetHandler(IBranchSpecificationInfoService BranchSpecificationSummaryInfoService)
        {
            _BranchSpecificationSummaryInfoService = BranchSpecificationSummaryInfoService;
            _BranchSpecificationSummaryInfoService.NotNull(nameof(BranchSpecificationSummaryInfoService));
        }

        public async Task<BranchSpecificationInfoDto> Handle(string billId, CancellationToken cancellationToken)
        {
            BranchSpecificationInfoDto result = await _BranchSpecificationSummaryInfoService.GetInfo(billId);

            result.MeterLife = GetDistance(result.LatestMeterChangeDate, result.WaterInstallDate);

            //  result.SiphonLife = CalculationDistanceDate.CalcDistance(result.HasSewage ?
            //                      result.SiphonInstallationDate:result.LastChangeSiphonDate);


            result.SiphonLife = GetDistance(GetSiphonDate(result.HasSewage,result.LastChangeSiphonDate,result.SiphonInstallationDate));

            return result;
        }
        private string GetDistance(string latestMeterChange, string waterInstallDate)
        {
            int? distance = CalculationDistanceDate.CalcDistance(string.IsNullOrWhiteSpace(latestMeterChange) ? waterInstallDate : latestMeterChange);
            return distance.HasValue ? CalculationDistanceDate.ConvertDaysToDate(distance.Value) : ExceptionLiterals.Incalculable;
        }
        private string GetDistance(string date)

        {
            int? distance = CalculationDistanceDate.CalcDistance(date);
            return distance.HasValue ? CalculationDistanceDate.ConvertDaysToDate(distance.Value) : ExceptionLiterals.Incalculable;
        }

        private string GetSiphonDate(bool hasSewage, string lastChange, string installDate)
        {
            return hasSewage ?
                   (lastChange.ToGregorianDateOnly() > installDate.ToGregorianDateOnly() ?lastChange :installDate) :
                   (ExceptionLiterals.InvalidDate);
        }
    }
}
