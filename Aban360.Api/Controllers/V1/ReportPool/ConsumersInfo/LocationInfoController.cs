using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/location")]
    public class LocationInfoController : BaseController
    {
        private readonly ILocationInfoGetHandler _locationInfoHandle;
        public LocationInfoController(
            ILocationInfoGetHandler locationInfoHandle)
        {
            _locationInfoHandle = locationInfoHandle;
            _locationInfoHandle.NotNull(nameof(locationInfoHandle));
        }

        [HttpPost]
        [Route("info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LocationInfoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> info( SearchInput searchInput,CancellationToken cancellationToken)
        {
            LocationInfoDto summary = await _locationInfoHandle.Handle(searchInput.Input, cancellationToken);
            return Ok(summary);
        }

    }
}
