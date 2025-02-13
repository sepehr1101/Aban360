using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
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
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var readingBound = await _readingBoundGetSingleHandler.Handle(id, cancellationToken);
            return Ok(readingBound);
        }
    }
}
