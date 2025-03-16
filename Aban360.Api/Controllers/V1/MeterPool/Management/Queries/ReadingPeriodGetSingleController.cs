using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Queries
{
    [Route("v1/reading-period")]
    public class ReadingPeriodGetSingleController : BaseController
    {
        private readonly IReadingPeriodGetSingleHandler _readingPeriodGetSingleHandler;
        public ReadingPeriodGetSingleController(IReadingPeriodGetSingleHandler readingPeriodGetSingleHandler)
        {
            _readingPeriodGetSingleHandler = readingPeriodGetSingleHandler;
            _readingPeriodGetSingleHandler.NotNull(nameof(readingPeriodGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingPeriodGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            ReadingPeriodGetDto readingPeriods = await _readingPeriodGetSingleHandler.Handle(id, cancellationToken);
            return Ok(readingPeriods);
        }
    }
}
