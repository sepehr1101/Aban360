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
    [Route("v1/reading-daily-statement")]
    public class ReadingDailyStatementController : BaseController
    {
        private readonly IReadingDailyStatementHandler _readingDailyStatement;
        private readonly IReportGenerator _reportGenerator;
        public ReadingDailyStatementController(
            IReadingDailyStatementHandler readingDailyStatement,
            IReportGenerator reportGenerator)
        {
            _readingDailyStatement = readingDailyStatement;
            _readingDailyStatement.NotNull(nameof(_readingDailyStatement));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ReadingDailyStatementHeaderOutputDto, ReadingDailyStatementDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ReadingDailyStatementInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ReadingDailyStatementHeaderOutputDto, ReadingDailyStatementDataOutputDto> dailyStatement = await _readingDailyStatement.Handle(inputDto, cancellationToken);
            return Ok(dailyStatement);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ReadingDailyStatementInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _readingDailyStatement.Handle, CurrentUser, ReportLiterals.ReadingDailyStatement, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(ReadingDailyStatementInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 400;
            ReportOutput<ReadingDailyStatementHeaderOutputDto, ReadingDailyStatementDataOutputDto> dailyStatement = await _readingDailyStatement.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(dailyStatement, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
