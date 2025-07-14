using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/reading-daily-statement")]
    public class ReadingDailyStatementController : BaseController
    {
        private readonly IReadingDailyStatementHandler _readingDailyStatement;
        public ReadingDailyStatementController(IReadingDailyStatementHandler readingDailyStatement)
        {
            _readingDailyStatement = readingDailyStatement;
            _readingDailyStatement.NotNull(nameof(_readingDailyStatement));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ReadingDailyStatementHeaderOutputDto, ReadingDailyStatementDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ReadingDailyStatementInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ReadingDailyStatementHeaderOutputDto, ReadingDailyStatementDataOutputDto> dailyStatement = await _readingDailyStatement.Handle(inputDto, cancellationToken);
            return Ok(dailyStatement);
        }
    }
}
