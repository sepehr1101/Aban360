using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/ruined-meter-income-summary-by-zone")]
    public class RuinedMeterIncomeSummaryByZoneController : BaseController
    {
        private readonly IRuinedMeterIncomeSummaryByZoneHandler _ruinedMeterIncomeSummaryHandler;
        private readonly IReportGenerator _reportGenerator;
        public RuinedMeterIncomeSummaryByZoneController(
            IRuinedMeterIncomeSummaryByZoneHandler ruinedMeterIncomeSummaryHandler,
            IReportGenerator reportGenerator)
        {
            _ruinedMeterIncomeSummaryHandler = ruinedMeterIncomeSummaryHandler;
            _ruinedMeterIncomeSummaryHandler.NotNull(nameof(ruinedMeterIncomeSummaryHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(RuinedMeterIncomeInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeSummaryDataOutputDto> result = await _ruinedMeterIncomeSummaryHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, RuinedMeterIncomeInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _ruinedMeterIncomeSummaryHandler.Handle, CurrentUser, ReportLiterals.RuinedMeterIncomeSummary + ReportLiterals.ByZone, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(RuinedMeterIncomeInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode =412 ;
            ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeSummaryDataOutputDto> result = await _ruinedMeterIncomeSummaryHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
