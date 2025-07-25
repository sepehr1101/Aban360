﻿using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using DNTPersianUtils.Core;

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

            result.MeterLife = CalculationDistanceDate.CalcDistance(result.LatestMeterChangeDate is null ?
                                result.WaterInstallDate : result.LatestMeterChangeDate);

          //  result.SiphonLife = CalculationDistanceDate.CalcDistance(result.HasSewage ?
          //                      result.SiphonInstallationDate:result.LastChangeSiphonDate);


            result.SiphonLife = CalculationDistanceDate.CalcDistance(
                result.HasSewage ?
                     (result.LastChangeSiphonDate.ToGregorianDateOnly()>result.SiphonInstallationDate.ToGregorianDateOnly()?
                        (result.LastChangeSiphonDate):
                        (result.SiphonInstallationDate)):
                     (ExceptionLiterals.InvalidDate));

            return result;
        }
    }
}
