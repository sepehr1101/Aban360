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
    [Route("v1/sewage-water-request-Summary-by-zone-and-usage")]
    public class SewageWaterRequestSummaryByZoneAndUsageController : BaseController
    {
        private readonly ISewageWaterRequestSummaryByZoneAndUsageHandler _sewageWaterRequestSummaryByZoneAndUsageHandler;
        private readonly IReportGenerator _reportGenerator;
        public SewageWaterRequestSummaryByZoneAndUsageController(
            ISewageWaterRequestSummaryByZoneAndUsageHandler sewageWaterRequestSummaryByZoneAndUsageHandler,
            IReportGenerator reportGenerator)
        {
            _sewageWaterRequestSummaryByZoneAndUsageHandler = sewageWaterRequestSummaryByZoneAndUsageHandler;
            _sewageWaterRequestSummaryByZoneAndUsageHandler.NotNull(nameof(sewageWaterRequestSummaryByZoneAndUsageHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SewageWaterRequestInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestSummaryDataOutputDto> result = await _sewageWaterRequestSummaryByZoneAndUsageHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, SewageWaterRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            string reportName = (inputDto.IsWater ? ReportLiterals.WaterRequestSummary : ReportLiterals.SewageRequestSummary) + ReportLiterals.ByUsageAndZone;
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _sewageWaterRequestSummaryByZoneAndUsageHandler.Handle, CurrentUser, reportName, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(SewageWaterRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 265;
            ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestSummaryDataOutputDto> result = await _sewageWaterRequestSummaryByZoneAndUsageHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
