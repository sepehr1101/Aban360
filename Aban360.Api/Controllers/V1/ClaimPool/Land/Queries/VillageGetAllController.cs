using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.Land.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/village")]
    public class VillageGetAllController : BaseController
    {
        private readonly IVillageLookupService _villageHandler;
        public VillageGetAllController(IVillageLookupService villageHandler)
        {
            _villageHandler = villageHandler;
            _villageHandler.NotNull(nameof(_villageHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<VillageGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<VillageGetDto> villages = await _villageHandler.VillageGetAllAsync(cancellationToken);
            return Ok(villages);
        }
    }
}
