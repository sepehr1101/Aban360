using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.LocationsInfo
{
    [Route("v1/zone")]
    public class ZoneByRegionIdGetController : BaseController
    {
        private readonly IZonesByRegionIdGetHandler _zoneByRegionIdGetHandler;
        public ZoneByRegionIdGetController(IZonesByRegionIdGetHandler zoneByRegionIdGetHandler)
        {
            _zoneByRegionIdGetHandler = zoneByRegionIdGetHandler;
            _zoneByRegionIdGetHandler.NotNull(nameof(zoneByRegionIdGetHandler));
        }

        [HttpPost]
        [Route("by-region")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] SearchByIdInput input, CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> zones = await _zoneByRegionIdGetHandler.Handle(input.Id, cancellationToken);
            return Ok(zones);
        }
    }
}
