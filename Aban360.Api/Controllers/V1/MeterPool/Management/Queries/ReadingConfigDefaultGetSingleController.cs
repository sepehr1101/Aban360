using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Queries
{
    [Route("v1/reading-config-default")]
    public class ReadingConfigDefaultGetSingleController : BaseController
    {
        private readonly IReadingConfigDefaultGetSingleHandler _readingConfigDefaultGetSingleHandler;
        public ReadingConfigDefaultGetSingleController(IReadingConfigDefaultGetSingleHandler readingConfigDefaultGetSingleHandler)
        {
            _readingConfigDefaultGetSingleHandler = readingConfigDefaultGetSingleHandler;
            _readingConfigDefaultGetSingleHandler.NotNull(nameof(readingConfigDefaultGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingConfigDefaultGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var readingConfigDefaults = await _readingConfigDefaultGetSingleHandler.Handle(id, cancellationToken);
            return Ok(readingConfigDefaults);
        }
    }
}
