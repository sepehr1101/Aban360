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
    [Route("v1/reading-list-summary-by-zone-grouped")]
    public class ReadingListSummaryByZoneGroupedController : BaseController
    {
        private readonly IReadingListSummaryByZoneGroupedHandler _readingListSummary;
        private readonly IReportGenerator _reportGenerator;
        public ReadingListSummaryByZoneGroupedController(
            IReadingListSummaryByZoneGroupedHandler readingListSummary,
            IReportGenerator reportGenerator)
        {
            _readingListSummary = readingListSummary;
            _readingListSummary.NotNull(nameof(_readingListSummary));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ReadingListHeaderOutputDto, ReportOutput<ReadingListSummaryDataOutputDto, ReadingListSummaryDataOutputDto>>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ReadingListInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ReadingListHeaderOutputDto, ReportOutput<ReadingListSummaryDataOutputDto, ReadingListSummaryDataOutputDto>> waterSales = await _readingListSummary.Handle(inputDto, cancellationToken);
            return Ok(waterSales);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ReadingListInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _readingListSummary.HandleFlat, CurrentUser, ReportLiterals.ReadingListSummary + ReportLiterals.ByZone, connectionId, ReportLiterals.HandleFlat);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(ReadingListInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 323;
            ReportOutput<ReadingListHeaderOutputDto, ReadingListSummaryDataOutputDto> result = await _readingListSummary.HandleFlat(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
