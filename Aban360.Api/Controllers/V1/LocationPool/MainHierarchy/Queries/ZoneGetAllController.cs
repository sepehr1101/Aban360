using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Queries
{
    [Route("v1/zone")]
    public class ZoneGetAllController : BaseController
    {
        private readonly IZoneGetAllHandler _zoneGetAllHandler;
        public ZoneGetAllController(IZoneGetAllHandler zoneGetAllHandler)
        {
            _zoneGetAllHandler = zoneGetAllHandler;
            _zoneGetAllHandler.NotNull(nameof(zoneGetAllHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ZoneGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var zone = await _zoneGetAllHandler.Handle(cancellationToken);
            return Ok(zone);
        }
    }
}
