using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterSummaryByZoneTransactions
{
    [Route("v1/malfunction-meter-summary-by-zone")]
    public class MalfunctionMeterSummaryByZoneController : BaseController
    {
        private readonly IMalfunctionMeterSummaryByZoneHandler _malfunctionMeterSummaryByZoneHandler;
        private readonly IReportGenerator _reportGenerator;
        public MalfunctionMeterSummaryByZoneController(
            IMalfunctionMeterSummaryByZoneHandler malfunctionMeterSummaryByZoneHandler,
            IReportGenerator reportGenerator)
        {
            _malfunctionMeterSummaryByZoneHandler = malfunctionMeterSummaryByZoneHandler;
            _malfunctionMeterSummaryByZoneHandler.NotNull(nameof(malfunctionMeterSummaryByZoneHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MalfunctionMeterSummaryHeaderOutputDto, MalfunctionMeterSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(MalfunctionMeterInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<MalfunctionMeterSummaryHeaderOutputDto, MalfunctionMeterSummaryDataOutputDto> result = await _malfunctionMeterSummaryByZoneHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, MalfunctionMeterInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _malfunctionMeterSummaryByZoneHandler.Handle, CurrentUser, ReportLiterals.MalfunctionMeterSummary + ReportLiterals.ByZone, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(MalfunctionMeterInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 362;
            ReportOutput<MalfunctionMeterSummaryHeaderOutputDto, MalfunctionMeterSummaryDataOutputDto> result = await _malfunctionMeterSummaryByZoneHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
