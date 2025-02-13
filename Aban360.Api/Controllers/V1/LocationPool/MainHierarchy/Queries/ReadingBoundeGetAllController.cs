using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Queries
{
    [Route("v1/reading-bound")]
    public class ReadingBoundeGetAllController : BaseController
    {
        private readonly IReadingBoundGetAllHandler _readingBoundGetAllHandler;
        public ReadingBoundeGetAllController(IReadingBoundGetAllHandler readingBoundGetAllHandler)
        {
            _readingBoundGetAllHandler = readingBoundGetAllHandler;
            _readingBoundGetAllHandler.NotNull(nameof(readingBoundGetAllHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var readingBound = await _readingBoundGetAllHandler.Handle(cancellationToken);
            return Ok(readingBound);
        }
    }
}
