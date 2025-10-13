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
    [Route("v1/malfunction-meter-summary-by-usage")]
    public class MalfunctionMeterSummaryByUsageController : BaseController
    {
        private readonly IMalfunctionMeterSummaryByUsageHandler _malfunctionMeterSummaryByUsageHandler;
        private readonly IReportGenerator _reportGenerator;
        public MalfunctionMeterSummaryByUsageController(
            IMalfunctionMeterSummaryByUsageHandler malfunctionMeterSummaryByUsageHandler,
            IReportGenerator reportGenerator)
        {
            _malfunctionMeterSummaryByUsageHandler = malfunctionMeterSummaryByUsageHandler;
            _malfunctionMeterSummaryByUsageHandler.NotNull(nameof(malfunctionMeterSummaryByUsageHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MalfunctionMeterSummaryHeaderOutputDto, MalfunctionMeterSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(MalfunctionMeterInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<MalfunctionMeterSummaryHeaderOutputDto, MalfunctionMeterSummaryDataOutputDto> result = await _malfunctionMeterSummaryByUsageHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, MalfunctionMeterInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _malfunctionMeterSummaryByUsageHandler.Handle, CurrentUser, ReportLiterals.MalfunctionMeterSummary + ReportLiterals.ByUsage, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(MalfunctionMeterInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 361;
            ReportOutput<MalfunctionMeterSummaryHeaderOutputDto, MalfunctionMeterSummaryDataOutputDto> result = await _malfunctionMeterSummaryByUsageHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
