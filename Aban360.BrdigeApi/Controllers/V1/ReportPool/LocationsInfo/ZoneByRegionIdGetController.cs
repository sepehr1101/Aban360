using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using ILocationLookupService = global::Aban360.LocationPool.Application.Features.MainHierarchy.Services.ILocationLookupService;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.LocationsInfo
{
    [Route("v1/zone")]
    public class ZoneByRegionIdGetController : BaseController
    {
        private readonly ILocationLookupService _locationLookupService;
        public ZoneByRegionIdGetController(ILocationLookupService locationLookupService)
        {
            _locationLookupService = locationLookupService;
            _locationLookupService.NotNull(nameof(locationLookupService));
        }

        [HttpPost]
        [Route("by-region")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] SearchByIdInput input, CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> zones = await _locationLookupService.ZonesByRegionIdGetAsync(input.Id, cancellationToken);
            return Ok(zones);
        }
    }
}
