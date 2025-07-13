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
        public ReadingChecklistController(IReadingChecklistHandler readingChecklistHandler)
        {
            _readingChecklistHandler = readingChecklistHandler;
            _readingChecklistHandler.NotNull(nameof(readingChecklistHandler));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ReadingChecklistHeaderOutputDto, ReadingChecklistDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ReadingChecklistInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<ReadingChecklistHeaderOutputDto, ReadingChecklistDataOutputDto> readingChecklist = await _readingChecklistHandler.Handle(input, cancellationToken);
            return Ok(readingChecklist);
        }
    }
}
