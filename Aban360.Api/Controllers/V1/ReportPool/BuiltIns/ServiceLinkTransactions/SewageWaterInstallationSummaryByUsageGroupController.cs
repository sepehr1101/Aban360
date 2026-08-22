using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/sewage-water-installation-summary-by-usage-group")]
    public class SewageWaterInstallationSummaryByUsageGroupController : BaseController
    {
        private readonly ISewageWaterInstallationSummaryByUsageGroupHandler _sewageWaterInstallationSummaryByUsageGroupHandler;
        private readonly IReportGenerator _reportGenerator;
        public SewageWaterInstallationSummaryByUsageGroupController(
            ISewageWaterInstallationSummaryByUsageGroupHandler sewageWaterInstallationSummaryByUsageGroupHandler,
            IReportGenerator reportGenerator)
        {
            _sewageWaterInstallationSummaryByUsageGroupHandler = sewageWaterInstallationSummaryByUsageGroupHandler;
            _sewageWaterInstallationSummaryByUsageGroupHandler.NotNull(nameof(sewageWaterInstallationSummaryByUsageGroupHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SewageWaterInstallationByUsageGroupInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryDataOutputDto> result = await _sewageWaterInstallationSummaryByUsageGroupHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, SewageWaterInstallationByUsageGroupInputDto inputDto, CancellationToken cancellationToken)
        {
            string reportName = (inputDto.IsWater ? ReportLiterals.WaterInstallationSummary : ReportLiterals.SewageInstallationSummary) + ReportLiterals.ByUsageGroup;
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _sewageWaterInstallationSummaryByUsageGroupHandler.Handle, CurrentUser, reportName, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(SewageWaterInstallationByUsageGroupInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 241;
            ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryDataOutputDto> calculationDetails = await _sewageWaterInstallationSummaryByUsageGroupHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
