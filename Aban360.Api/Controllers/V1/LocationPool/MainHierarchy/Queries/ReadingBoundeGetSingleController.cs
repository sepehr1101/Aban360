using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Queries
{
    [Route("v1/reading-bound")]
    public class ReadingBoundeGetSingleController : BaseController
    {
        private readonly IReadingBoundGetSingleHandler _readingBoundGetSingleHandler;
        public ReadingBoundeGetSingleController(IReadingBoundGetSingleHandler readingBoundGetSingleHandler)
        {
            _readingBoundGetSingleHandler = readingBoundGetSingleHandler;
            _readingBoundGetSingleHandler.NotNull(nameof(readingBoundGetSingleHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ReadingBoundGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            ReadingBoundGetDto readingBound = await _readingBoundGetSingleHandler.Handle(id, cancellationToken);
            return Ok(readingBound);
        }
    }
}
