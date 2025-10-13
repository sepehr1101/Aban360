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
    [Route("v1/ruined-meter-income-summary-by-usage")]
    public class RuinedMeterIncomeSummarybyUsageController : BaseController
    {
        private readonly IRuinedMeterIncomeSummarybyUsageHandler _ruinedMeterIncomeSummarybyUsageHandler;
        private readonly IReportGenerator _reportGenerator;
        public RuinedMeterIncomeSummarybyUsageController(
            IRuinedMeterIncomeSummarybyUsageHandler ruinedMeterIncomeSummarybyUsageHandler,
            IReportGenerator reportGenerator)
        {
            _ruinedMeterIncomeSummarybyUsageHandler = ruinedMeterIncomeSummarybyUsageHandler;
            _ruinedMeterIncomeSummarybyUsageHandler.NotNull(nameof(ruinedMeterIncomeSummarybyUsageHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(RuinedMeterIncomeInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeSummaryDataOutputDto> result = await _ruinedMeterIncomeSummarybyUsageHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, RuinedMeterIncomeInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _ruinedMeterIncomeSummarybyUsageHandler.Handle, CurrentUser, ReportLiterals.RuinedMeterIncomeSummary+ReportLiterals.ByUsage, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(RuinedMeterIncomeInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 411;
            ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeSummaryDataOutputDto> result = await _ruinedMeterIncomeSummarybyUsageHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
