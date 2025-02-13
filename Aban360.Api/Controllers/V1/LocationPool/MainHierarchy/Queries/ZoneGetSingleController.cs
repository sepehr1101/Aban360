using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Queries
{
    [Route("v1/zone")]
    public class ZoneGetSingleController : BaseController
    {
        private readonly IZoneGetSingleHandler _zoneGetSingleHandler;
        public ZoneGetSingleController(IZoneGetSingleHandler zoneGetSingleHandler)
        {
            _zoneGetSingleHandler = zoneGetSingleHandler;
            _zoneGetSingleHandler.NotNull(nameof(zoneGetSingleHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZoneGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id,CancellationToken cancellationToken)
        {
            var zone = await _zoneGetSingleHandler.Handle(id,cancellationToken);
            return Ok(zone);
        }
    }
}
