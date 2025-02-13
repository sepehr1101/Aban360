using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1._CombinedPools.Implementations
{
    [Route("location-and-access")]
    [ApiController]
    public class LocationAndAccesTreeQueryController : BaseController
    {
        private readonly ILocationValueKeyQueryHandler _locationQueryHandler;
        public LocationAndAccesTreeQueryController(
            ILocationValueKeyQueryHandler locationValueKeyQueryHandler)
        {
            _locationQueryHandler = locationValueKeyQueryHandler;
            _locationQueryHandler.NotNull(nameof(locationValueKeyQueryHandler));
        }

        [HttpGet, HttpPost]
        [Route("value-keys")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LocationValueKeyDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetValueKeys(CancellationToken cancellationToken)
        {
            var locationValueKeyDto= await _locationQueryHandler.Handle(cancellationToken);
            return Ok(locationValueKeyDto);
        }
    }
}
