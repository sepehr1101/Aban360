using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
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
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ReadingBoundGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ReadingBoundGetDto> readingBound = await _readingBoundGetAllHandler.Handle(cancellationToken);
            return Ok(readingBound);
        }
    }
}
