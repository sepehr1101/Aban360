using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.LocationsInfo
{
    [Route("v1/village")]
    public class VillageByZoneIdGetController : BaseController
    {
        private readonly IMunicipalityByZoneIdGetlHandler _villageByZoneIdGetHandler;
        public VillageByZoneIdGetController(IMunicipalityByZoneIdGetlHandler villageByZoneIdGetHandler)
        {
            _villageByZoneIdGetHandler = villageByZoneIdGetHandler;
            _villageByZoneIdGetHandler.NotNull(nameof(villageByZoneIdGetHandler));
        }

        [HttpPost]
        [Route("by-zone")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] SearchByIdInput input, CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> villages = await _villageByZoneIdGetHandler.Handle(input.Id, cancellationToken);
            return Ok(villages);
        }
    }
}
