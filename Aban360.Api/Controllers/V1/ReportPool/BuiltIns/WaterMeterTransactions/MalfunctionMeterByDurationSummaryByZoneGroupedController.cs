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
    [Route("v1/malfunction-meter-by-duration-summary-by-zone-grouped")]
    public class MalfunctionMeterByDurationSummaryByZoneGroupedController : BaseController
    {
        private readonly IMalfunctionMeterByDurationSummaryByZoneGroupedHandler _malfunctionMeterByDurationSummaryByZoneGroupedHandler;
        private readonly IReportGenerator _reportGenerator;
        public MalfunctionMeterByDurationSummaryByZoneGroupedController(
            IMalfunctionMeterByDurationSummaryByZoneGroupedHandler malfunctionMeterByDurationSummaryByZoneGroupedHandler,
            IReportGenerator reportGenerator)
        {
            _malfunctionMeterByDurationSummaryByZoneGroupedHandler = malfunctionMeterByDurationSummaryByZoneGroupedHandler;
            _malfunctionMeterByDurationSummaryByZoneGroupedHandler.NotNull(nameof(malfunctionMeterByDurationSummaryByZoneGroupedHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, ReportOutput<MalfunctionMeterByDurationSummaryDataOutputDto, MalfunctionMeterByDurationSummaryDataOutputDto>>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(MalfunctionMeterByDurationInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, ReportOutput<MalfunctionMeterByDurationSummaryDataOutputDto, MalfunctionMeterByDurationSummaryDataOutputDto>> result = await _malfunctionMeterByDurationSummaryByZoneGroupedHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, MalfunctionMeterByDurationInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _malfunctionMeterByDurationSummaryByZoneGroupedHandler.HandleFlat, CurrentUser, ReportLiterals.MalfunctionMeterByDurationSummary + ReportLiterals.ByZone, connectionId, ReportLiterals.HandleFlat);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(MalfunctionMeterByDurationInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 303;
            ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryDataOutputDto> calculationDetails = await _malfunctionMeterByDurationSummaryByZoneGroupedHandler.HandleFlat(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
