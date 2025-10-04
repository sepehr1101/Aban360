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
    [Route("v1/reading-status-statement-summary-by-zone")]
    public class ReadingStatusStatementSummaryByZoneController : BaseController
    {
        private readonly IReadingStatusStatementSummaryByZoneHandler _readingStatusStatementSummaryByZone;
        private readonly IReportGenerator _reportGenerator;
        public ReadingStatusStatementSummaryByZoneController(
            IReadingStatusStatementSummaryByZoneHandler readingStatusStatementSummaryByZone,
            IReportGenerator reportGenerator)
        {
            _readingStatusStatementSummaryByZone = readingStatusStatementSummaryByZone;
            _readingStatusStatementSummaryByZone.NotNull(nameof(_readingStatusStatementSummaryByZone));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ReadingStatusStatementInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementSummaryDataOutputDto> statusStatementSummaryByZone = await _readingStatusStatementSummaryByZone.Handle(inputDto, cancellationToken);
            return Ok(statusStatementSummaryByZone);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ReadingStatusStatementInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _readingStatusStatementSummaryByZone.Handle, CurrentUser, ReportLiterals.ReadingStatusStatementSummary + ReportLiterals.ByZone, connectionId);
            return Ok(inputDto);
        }
    }
}
