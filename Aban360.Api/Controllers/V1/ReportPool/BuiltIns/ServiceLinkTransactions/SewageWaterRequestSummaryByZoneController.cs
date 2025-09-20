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
    [Route("v1/sewage-water-request-Summary-by-zone")]
    public class SewageWaterRequestSummaryByZoneController : BaseController
    {
        private readonly ISewageWaterRequestSummaryByZoneHandler _sewageWaterRequestSummaryByZoneHandler;
        private readonly IReportGenerator _reportGenerator;
        public SewageWaterRequestSummaryByZoneController(
            ISewageWaterRequestSummaryByZoneHandler sewageWaterRequestSummaryByZoneHandler,
            IReportGenerator reportGenerator)
        {
            _sewageWaterRequestSummaryByZoneHandler = sewageWaterRequestSummaryByZoneHandler;
            _sewageWaterRequestSummaryByZoneHandler.NotNull(nameof(sewageWaterRequestSummaryByZoneHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestSummaryByZoneDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SewageWaterRequestInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestSummaryByZoneDataOutputDto> result = await _sewageWaterRequestSummaryByZoneHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, SewageWaterRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            string reportName = inputDto.IsWater ? ReportLiterals.WaterRequestSummary + ReportLiterals.ByZone : ReportLiterals.SewageRequestSummary + ReportLiterals.ByZone;
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _sewageWaterRequestSummaryByZoneHandler.Handle, CurrentUser, reportName, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(SewageWaterRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 262;
            ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestSummaryByZoneDataOutputDto> result = await _sewageWaterRequestSummaryByZoneHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
