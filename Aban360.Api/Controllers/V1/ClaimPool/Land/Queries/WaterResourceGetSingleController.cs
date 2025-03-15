using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/water-resource")]
    public class WaterResourceGetSingleController : BaseController
    {
        private readonly IWaterResourceGetSingleHandler _waterResourceGetSingleHandler;
        public WaterResourceGetSingleController(IWaterResourceGetSingleHandler WaterResourceGetSingleHandler)
        {
            _waterResourceGetSingleHandler = WaterResourceGetSingleHandler;
            _waterResourceGetSingleHandler.NotNull(nameof(WaterResourceGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterResourceGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            WaterResourceGetDto waterResources = await _waterResourceGetSingleHandler.Handle(id, cancellationToken);
            return Ok(waterResources);
        }
    }
}
