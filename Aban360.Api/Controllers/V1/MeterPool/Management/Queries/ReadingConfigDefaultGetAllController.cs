using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Queries
{
    [Route("v1/reading-config-default")]
    public class ReadingConfigDefaultGetAllController : BaseController
    {
        private readonly IReadingConfigDefaultGetAllHandler _readingConfigDefaultGetAllHandler;
        public ReadingConfigDefaultGetAllController(IReadingConfigDefaultGetAllHandler readingConfigDefaultGetAllHandler)
        {
            _readingConfigDefaultGetAllHandler = readingConfigDefaultGetAllHandler;
            _readingConfigDefaultGetAllHandler.NotNull(nameof(readingConfigDefaultGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ReadingConfigDefaultGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var readingConfigDefaults = await _readingConfigDefaultGetAllHandler.Handle(cancellationToken);
            return Ok(readingConfigDefaults);
        }
    }
}
