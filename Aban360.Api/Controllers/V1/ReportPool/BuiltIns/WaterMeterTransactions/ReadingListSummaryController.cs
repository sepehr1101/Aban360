using Aban360.Api.Cronjobs;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Excel;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/reading-list-summary")]
    public class ReadingListSummaryController : BaseController
    {
        private readonly IReadingListSummaryHandler _readingListSummary;
        private readonly IReportGenerator _reportGenerator;
        public ReadingListSummaryController(
            IReadingListSummaryHandler readingListSummary,
            IReportGenerator reportGenerator)
        {
            _readingListSummary = readingListSummary;
            _readingListSummary.NotNull(nameof(_readingListSummary));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ReadingListHeaderOutputDto, ReadingListSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ReadingListInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ReadingListHeaderOutputDto, ReadingListSummaryDataOutputDto> waterSales = await _readingListSummary.Handle(inputDto, cancellationToken);
            return Ok(waterSales);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ReadingListInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _readingListSummary.Handle, CurrentUser, ReportLiterals.ReadingListSummary, connectionId);
            return Ok(inputDto);
        }
    }
}
