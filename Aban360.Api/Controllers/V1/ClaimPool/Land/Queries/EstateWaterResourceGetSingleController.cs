using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/estate-water-resource")]
    public class EstateWaterResourceGetSingleController : BaseController
    {
        private readonly IEstateWaterResourceGetSingleHandler _estateWaterResourceGetSingleHandler;
        public EstateWaterResourceGetSingleController(IEstateWaterResourceGetSingleHandler estateWaterResourceGetSingleHandler)
        {
            _estateWaterResourceGetSingleHandler = estateWaterResourceGetSingleHandler;
            _estateWaterResourceGetSingleHandler.NotNull(nameof(estateWaterResourceGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateWaterResourceGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            EstateWaterResourceGetDto estateWaterResources = await _estateWaterResourceGetSingleHandler.Handle(id, cancellationToken);
            return Ok(estateWaterResources);
        }
    }
}
