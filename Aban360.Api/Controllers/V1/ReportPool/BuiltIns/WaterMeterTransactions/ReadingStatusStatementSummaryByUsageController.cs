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
    [Route("v1/reading-status-statement-summary-by-usage")]
    public class ReadingStatusStatementSummaryByUsageController : BaseController
    {
        private readonly IReadingStatusStatementSummaryByUsageHandler _readingStatusStatementSummaryByUsage;
        private readonly IReportGenerator _reportGenerator;
        public ReadingStatusStatementSummaryByUsageController(
            IReadingStatusStatementSummaryByUsageHandler readingStatusStatementSummaryByUsage,
            IReportGenerator reportGenerator)
        {
            _readingStatusStatementSummaryByUsage = readingStatusStatementSummaryByUsage;
            _readingStatusStatementSummaryByUsage.NotNull(nameof(_readingStatusStatementSummaryByUsage));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ReadingStatusStatementInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementSummaryDataOutputDto> statusStatementSummaryByUsage = await _readingStatusStatementSummaryByUsage.Handle(inputDto, cancellationToken);
            return Ok(statusStatementSummaryByUsage);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ReadingStatusStatementInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _readingStatusStatementSummaryByUsage.Handle, CurrentUser, ReportLiterals.ReadingStatusStatementSummary + ReportLiterals.ByUsage, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(ReadingStatusStatementInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode =391 ;
            ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementSummaryDataOutputDto> statusStatement = await _readingStatusStatementSummaryByUsage.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(statusStatement, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
