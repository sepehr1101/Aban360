using Aban360.Api.Cronjobs;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/reading-checklist")]
    public class ReadingChecklistController : BaseController
    {
        private readonly IReadingChecklistHandler _readingChecklistHandler;
        private readonly IReportGenerator _reportGenerator;
        public ReadingChecklistController(
            IReadingChecklistHandler readingChecklistHandler,
            IReportGenerator reportGenerator)
        {
            _readingChecklistHandler = readingChecklistHandler;
            _readingChecklistHandler.NotNull(nameof(readingChecklistHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ReadingChecklistHeaderOutputDto, ReadingChecklistDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ReadingChecklistInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<ReadingChecklistHeaderOutputDto, ReadingChecklistDataOutputDto> readingChecklist = await _readingChecklistHandler.Handle(input, cancellationToken);
            return Ok(readingChecklist);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ReadingChecklistInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _readingChecklistHandler.Handle, CurrentUser, ReportLiterals.ReadingChecklist, connectionId);
            return Ok(inputDto);
        }
    }
}
