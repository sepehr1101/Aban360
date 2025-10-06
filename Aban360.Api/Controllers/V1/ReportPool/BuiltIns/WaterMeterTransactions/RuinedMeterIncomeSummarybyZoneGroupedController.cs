using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/ruined-meter-income-summary-by-zone-grouped")]
    public class RuinedMeterIncomeSummarybyZoneGroupedController : BaseController
    {
        private readonly IRuinedMeterIncomeSummaryByZoneGroupedHandler _ruinedMeterIncomeSummarybyZoneGroupedHandler;
        private readonly IReportGenerator _reportGenerator;
        public RuinedMeterIncomeSummarybyZoneGroupedController(
            IRuinedMeterIncomeSummaryByZoneGroupedHandler ruinedMeterIncomeSummarybyZoneGroupedHandler,
            IReportGenerator reportGenerator)
        {
            _ruinedMeterIncomeSummarybyZoneGroupedHandler = ruinedMeterIncomeSummarybyZoneGroupedHandler;
            _ruinedMeterIncomeSummarybyZoneGroupedHandler.NotNull(nameof(ruinedMeterIncomeSummarybyZoneGroupedHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<RuinedMeterIncomeHeaderOutputDto, ReportOutput<RuinedMeterIncomeSummaryDataOutputDto, RuinedMeterIncomeSummaryDataOutputDto>>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(RuinedMeterIncomeInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<RuinedMeterIncomeHeaderOutputDto, ReportOutput<RuinedMeterIncomeSummaryDataOutputDto, RuinedMeterIncomeSummaryDataOutputDto>> result = await _ruinedMeterIncomeSummarybyZoneGroupedHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, RuinedMeterIncomeInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _ruinedMeterIncomeSummarybyZoneGroupedHandler.HandleFlat, CurrentUser, ReportLiterals.RuinedMeterIncomeSummary + ReportLiterals.ByZone, connectionId, ReportLiterals.HandleFlat);
            return Ok(inputDto);
        }
    }
}
