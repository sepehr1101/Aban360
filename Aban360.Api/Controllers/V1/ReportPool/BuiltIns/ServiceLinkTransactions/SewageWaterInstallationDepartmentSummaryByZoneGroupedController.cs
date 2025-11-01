using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/sewage-water-installation-department-summary-by-zone-grouped")]
    public class SewageWaterInstallationDepartmentSummaryByZoneGroupedController : BaseController
    {
        private readonly ISewageWaterInstallationDepartmentSummaryByZoneIdGroupingHandler _sewageWaterInstallationDepartmentSummaryByZoneGroupingHandler;
        private readonly IReportGenerator _reportGenerator;
        public SewageWaterInstallationDepartmentSummaryByZoneGroupedController(
            ISewageWaterInstallationDepartmentSummaryByZoneIdGroupingHandler sewageWaterInstallationDepartmentSummaryByZoneGroupingHandler,
            IReportGenerator reportGenerator)
        {
            _sewageWaterInstallationDepartmentSummaryByZoneGroupingHandler = sewageWaterInstallationDepartmentSummaryByZoneGroupingHandler;
            _sewageWaterInstallationDepartmentSummaryByZoneGroupingHandler.NotNull(nameof(sewageWaterInstallationDepartmentSummaryByZoneGroupingHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SewageWaterInstallationHeaderOutputDto, ReportOutput<SewageWaterInstallationSummaryByZoneIdDateOutputDto, SewageWaterInstallationSummaryByZoneIdDateOutputDto>>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SewageWaterInstallationInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<SewageWaterInstallationHeaderOutputDto, ReportOutput<SewageWaterInstallationSummaryByZoneIdDateOutputDto, SewageWaterInstallationSummaryByZoneIdDateOutputDto>> result = await _sewageWaterInstallationDepartmentSummaryByZoneGroupingHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, SewageWaterInstallationInputDto inputDto, CancellationToken cancellationToken)
        {
            string reportName = (inputDto.IsWater ? ReportLiterals.WaterInstallationDepartmentSummary : ReportLiterals.SewageInstallationDepartmentSummary) + ReportLiterals.ByZone;
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _sewageWaterInstallationDepartmentSummaryByZoneGroupingHandler.HandleFlat, CurrentUser, reportName, connectionId, ReportLiterals.HandleFlat);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(SewageWaterInstallationInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 653;
            ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryByZoneIdDateOutputDto> result = await _sewageWaterInstallationDepartmentSummaryByZoneGroupingHandler.HandleFlat(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
