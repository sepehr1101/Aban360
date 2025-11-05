using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/sewage-water-distance-request-installation-summary-by-zone-grouped")]
    public class SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedController : BaseController
    {
        private readonly ISewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler _sewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler;
        private readonly IReportGenerator _reportGenerator;
        public SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedController(
            ISewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler sewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler,
            IReportGenerator reportGenerator)
        {
            _sewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler = sewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler;
            _sewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler.NotNull(nameof(sewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SewageWaterDistanceHeaderOutputDto, ReportOutput<SewageWaterDistanceSummaryByZoneGroupedDataOutputDto, SewageWaterDistanceSummaryByZoneGroupedDataOutputDto>>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SewageWaterDistanceofRequestAndInstallationByZoneInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<SewageWaterDistanceHeaderOutputDto, ReportOutput<SewageWaterDistanceSummaryByZoneGroupedDataOutputDto, SewageWaterDistanceSummaryByZoneGroupedDataOutputDto>> result = await _sewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, SewageWaterDistanceofRequestAndInstallationByZoneInputDto inputDto, CancellationToken cancellationToken)
        {
            string reportName = GetTitle(inputDto.IsWater, inputDto.IsInstallation);
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _sewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler.HandleFlat, CurrentUser, reportName, connectionId, ReportLiterals.HandleFlat);
            return Ok(inputDto);
        }


        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(SewageWaterDistanceofRequestAndInstallationByZoneInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 283;
            ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceSummaryByZoneGroupedDataOutputDto> result = await _sewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler.HandleFlat(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }

        private string GetTitle(bool IsWater, bool IsInstallation)
        {
            if (IsWater)
            {
                return IsInstallation ? ReportLiterals.WaterDistanceInstallationRegisterDetail : ReportLiterals.WaterDistanceRequestRegisterDetail;
            }
            else
            {
                return IsInstallation ? ReportLiterals.SewageDistanceInstallationeRegisterDetail : ReportLiterals.SewageDistanceRequesteRegisterDetail;
            }
        }

    }
}
