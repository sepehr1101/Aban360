using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Queries
{
    [Route("v1/reading-bound")]
    public class ReadingBoundeGetByZoneIdController : BaseController
    {
        private readonly IReadingBoundGetByZoneIdHandler _readingBoundGetByZoneIdHandler;
        public ReadingBoundeGetByZoneIdController(IReadingBoundGetByZoneIdHandler readingBoundGetByZoneIdHandler)
        {
            _readingBoundGetByZoneIdHandler = readingBoundGetByZoneIdHandler;
            _readingBoundGetByZoneIdHandler.NotNull(nameof(readingBoundGetByZoneIdHandler));
        }

        [HttpGet, HttpPost]
        [Route("get-by-zone/{zoneId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ReadingBoundGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByZoneId(int zoneId, CancellationToken cancellationToken)
        {
            ICollection<ReadingBoundGetDto> readingBound = await _readingBoundGetByZoneIdHandler.Handle(zoneId, cancellationToken);
            return Ok(readingBound);
        }
    }
}
