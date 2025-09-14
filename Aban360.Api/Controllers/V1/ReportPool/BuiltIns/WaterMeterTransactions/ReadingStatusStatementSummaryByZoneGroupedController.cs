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
    [Route("v1/reading-status-statement-summary-by-zone-grouped")]
    public class ReadingStatusStatementSummaryByZoneGroupedController : BaseController
    {
        private readonly IReadingStatusStatementSummaryByZoneGroupedHandler _readingStatusStatementSummaryByZoneGrouped;
        private readonly IReportGenerator _reportGenerator;
        public ReadingStatusStatementSummaryByZoneGroupedController(
            IReadingStatusStatementSummaryByZoneGroupedHandler readingStatusStatementSummaryByZoneGrouped,
            IReportGenerator reportGenerator)
        {
            _readingStatusStatementSummaryByZoneGrouped = readingStatusStatementSummaryByZoneGrouped;
            _readingStatusStatementSummaryByZoneGrouped.NotNull(nameof(_readingStatusStatementSummaryByZoneGrouped));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ReadingStatusStatementHeaderOutputDto, ReportOutput<ReadingStatusStatementSummaryDataOutputDto, ReadingStatusStatementSummaryDataOutputDto>>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ReadingStatusStatementInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ReadingStatusStatementHeaderOutputDto, ReportOutput<ReadingStatusStatementSummaryDataOutputDto, ReadingStatusStatementSummaryDataOutputDto>> statusStatementSummaryByZoneGrouped = await _readingStatusStatementSummaryByZoneGrouped.Handle(inputDto, cancellationToken);
            return Ok(statusStatementSummaryByZoneGrouped);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ReadingStatusStatementInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _readingStatusStatementSummaryByZoneGrouped.Handle, CurrentUser, ReportLiterals.ReadingStatusStatementSummary + ReportLiterals.ByZone, connectionId);
            return Ok(inputDto);
        }
    }
}
