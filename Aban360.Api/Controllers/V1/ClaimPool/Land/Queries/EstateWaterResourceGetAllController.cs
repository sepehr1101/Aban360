using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/estate-water-resource")]
    public class EstateWaterResourceGetAllController : BaseController
    {
        private readonly IEstateWaterResourceGetAllHandler _estateWaterResourceGetAllHandler;
        public EstateWaterResourceGetAllController(IEstateWaterResourceGetAllHandler estateWaterResourceGetAllHandler)
        {
            _estateWaterResourceGetAllHandler = estateWaterResourceGetAllHandler;
            _estateWaterResourceGetAllHandler.NotNull(nameof(estateWaterResourceGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<EstateWaterResourceGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<EstateWaterResourceGetDto> estateWaterResources = await _estateWaterResourceGetAllHandler.Handle(cancellationToken);
            return Ok(estateWaterResources);
        }
    }
}
