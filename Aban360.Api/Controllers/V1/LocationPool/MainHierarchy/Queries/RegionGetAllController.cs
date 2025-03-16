using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Queries
{
    [Route("v1/region")]
    public class RegionGetAllController : BaseController
    {
        private readonly IRegionGetAllHandler _regionGetAllHandler;
        public RegionGetAllController(IRegionGetAllHandler regionGetAllHandler)
        {
            _regionGetAllHandler = regionGetAllHandler;
            _regionGetAllHandler.NotNull(nameof(regionGetAllHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<RegionGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<RegionGetDto> region = await _regionGetAllHandler.Handle(cancellationToken);
            return Ok(region);
        }
    }
}
