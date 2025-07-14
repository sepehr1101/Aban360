using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/reading-status-statement")]
    public class ReadingStatusStatementController : BaseController
    {
        private readonly IReadingStatusStatementHandler _readingStatusStatement;
        public ReadingStatusStatementController(IReadingStatusStatementHandler readingStatusStatement)
        {
            _readingStatusStatement = readingStatusStatement;
            _readingStatusStatement.NotNull(nameof(_readingStatusStatement));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ReadingStatusStatementInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementDataOutputDto> statusStatement = await _readingStatusStatement.Handle(inputDto, cancellationToken);
            return Ok(statusStatement);
        }
    }
}
