using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/village")]
    public class VillageGetAllController : BaseController
    {
        private readonly IVillageGetAllHandler _villageHandler;
        public VillageGetAllController(IVillageGetAllHandler villageHandler)
        {
            _villageHandler = villageHandler;
            _villageHandler.NotNull(nameof(_villageHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<VillageGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<VillageGetDto> villages = await _villageHandler.Handle(cancellationToken);
            return Ok(villages);
        }
    }
}
