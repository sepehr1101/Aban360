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
    [Route("v1/sewage-water-installation-summary-by-zone-and-usage")]
    public class SewageWaterInstallationSummaryByZoneAndUsageController : BaseController
    {
        private readonly ISewageWaterInstallationSummaryByZoneAndUsageHandler _sewageWaterInstallationSummaryByZoneAndUsageHandler;
        private readonly IReportGenerator _reportGenerator;
        public SewageWaterInstallationSummaryByZoneAndUsageController(
            ISewageWaterInstallationSummaryByZoneAndUsageHandler sewageWaterInstallationSummaryByZoneAndUsageHandler,
            IReportGenerator reportGenerator)
        {
            _sewageWaterInstallationSummaryByZoneAndUsageHandler = sewageWaterInstallationSummaryByZoneAndUsageHandler;
            _sewageWaterInstallationSummaryByZoneAndUsageHandler.NotNull(nameof(sewageWaterInstallationSummaryByZoneAndUsageHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SewageWaterInstallationInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryDataOutputDto> result = await _sewageWaterInstallationSummaryByZoneAndUsageHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, SewageWaterInstallationInputDto inputDto, CancellationToken cancellationToken)
        {
            string reportName = (inputDto.IsWater ? ReportLiterals.WaterInstallationSummary: ReportLiterals.SewageInstallationSummary)+ReportLiterals.ByUsageAndZone;
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _sewageWaterInstallationSummaryByZoneAndUsageHandler.Handle, CurrentUser, reportName, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(SewageWaterInstallationInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 245;
            ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryDataOutputDto> calculationDetails = await _sewageWaterInstallationSummaryByZoneAndUsageHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
