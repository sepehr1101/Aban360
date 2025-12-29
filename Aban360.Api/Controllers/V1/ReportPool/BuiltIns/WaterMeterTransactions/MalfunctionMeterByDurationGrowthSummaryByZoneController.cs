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
    [Route("v1/malfunction-meter-by-duration-growth-summary-by-zone")]
    public class MalfunctionMeterByDurationGrowthSummaryByZoneController : BaseController
    {
        private readonly IMalfunctionMeterByDurationGrowthSummaryByZoneHandler _malfunctionMeterByDurationGrowthSummaryByZoneHandler;
        private readonly IReportGenerator _reportGenerator;
        public MalfunctionMeterByDurationGrowthSummaryByZoneController(
            IMalfunctionMeterByDurationGrowthSummaryByZoneHandler malfunctionMeterByDurationGrowthSummaryByZoneHandler,
            IReportGenerator reportGenerator)
        {
            _malfunctionMeterByDurationGrowthSummaryByZoneHandler = malfunctionMeterByDurationGrowthSummaryByZoneHandler;
            _malfunctionMeterByDurationGrowthSummaryByZoneHandler.NotNull(nameof(malfunctionMeterByDurationGrowthSummaryByZoneHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryByZoneDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(MalfunctionMeterByDurationGrowthInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryByZoneDataOutputDto> result = await _malfunctionMeterByDurationGrowthSummaryByZoneHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, MalfunctionMeterByDurationGrowthInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _malfunctionMeterByDurationGrowthSummaryByZoneHandler.Handle, CurrentUser, ReportLiterals.MalfunctionMeterByDurationGrowthSummary + ReportLiterals.ByZone, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(MalfunctionMeterByDurationGrowthInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 304;
            ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryByZoneDataOutputDto> calculationDetails = await _malfunctionMeterByDurationGrowthSummaryByZoneHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
