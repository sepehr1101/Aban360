using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/reading-issue-distancebill-summary-by-zone-grouped")]
    public class ReadingIssueDistanceBillSummaryByZoneGroupedController : BaseController
    {
        private readonly IReadingIssueDistanceBillSummaryByZoneGroupedHandler _readingIssueDistanceHandler;
        private readonly IReportGenerator _reportGenerator;
        public ReadingIssueDistanceBillSummaryByZoneGroupedController(
            IReadingIssueDistanceBillSummaryByZoneGroupedHandler readingIssueDistanceHandler,
            IReportGenerator reportGenerator)
        {
            _readingIssueDistanceHandler = readingIssueDistanceHandler;
            _readingIssueDistanceHandler.NotNull(nameof(readingIssueDistanceHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReportOutput<ReadingIssueDistanceBillSummryDataOutputDto, ReadingIssueDistanceBillSummryDataOutputDto>>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ReadingIssueDistanceBillInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReportOutput<ReadingIssueDistanceBillSummryDataOutputDto, ReadingIssueDistanceBillSummryDataOutputDto>> readingIssueDistance = await _readingIssueDistanceHandler.Handle(input, cancellationToken);
            return Ok(readingIssueDistance);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ReadingIssueDistanceBillInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _readingIssueDistanceHandler.HandleFlat, CurrentUser, ReportLiterals.ReadingIssueDistanceBillSummary + ReportLiterals.ByZone, connectionId, ReportLiterals.HandleFlat);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(ReadingIssueDistanceBillInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 423;
            ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReadingIssueDistanceBillSummryDataOutputDto> readingIssueDistance = await _readingIssueDistanceHandler.HandleFlat(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(readingIssueDistance, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
