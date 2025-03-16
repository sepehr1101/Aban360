using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Queries
{
    [Route("v1/reading-period-type")]
    public class ReadingPeriodTypeGetSingleController : BaseController
    {
        private readonly IReadingPeriodTypeGetSingleHandler _readingPeriodTypeGetSingleHandler;
        public ReadingPeriodTypeGetSingleController(IReadingPeriodTypeGetSingleHandler readingPeriodTypeGetSingleHandler)
        {
            _readingPeriodTypeGetSingleHandler = readingPeriodTypeGetSingleHandler;
            _readingPeriodTypeGetSingleHandler.NotNull(nameof(readingPeriodTypeGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingPeriodTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            ReadingPeriodTypeGetDto readingPeriodTypes = await _readingPeriodTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(readingPeriodTypes);
        }
    }
}
