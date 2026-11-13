using Aban360.LocationPool.Application.Features.MainHierarchy.Services;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.LocationsInfo
{
    [Route("v1/village")]
    public class VillageByZoneIdGetController : BaseController
    {
        private readonly IMunicipalityService _service;

        public VillageByZoneIdGetController(IMunicipalityService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("by-zone")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] SearchByIdInput input, CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> villages = await _service.GetByZoneIdAsync(input.Id, cancellationToken);
            return Ok(villages);
        }
    }
}
