using Aban360.Api.Cronjobs;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/ruined-meter-income-summary")]
    public class RuinedMeterIncomeSummaryController : BaseController
    {
        private readonly IRuinedMeterIncomeSummaryHandler _ruinedMeterIncomeSummaryHandler;
        private readonly IReportGenerator _reportGenerator;
        public RuinedMeterIncomeSummaryController(
            IRuinedMeterIncomeSummaryHandler ruinedMeterIncomeSummaryHandler,
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
            var result = await _ruinedMeterIncomeSummaryHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, RuinedMeterIncomeInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _ruinedMeterIncomeSummaryHandler.Handle, CurrentUser, ReportLiterals.RuinedMeterIncome, connectionId);
            return Ok(inputDto);
        }
    }
}
