using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/water-resource")]
    public class WaterResourceGetAllController : BaseController
    {
        private readonly IWaterResourceGetAllHandler _waterResourceGetAllHandler;
        public WaterResourceGetAllController(IWaterResourceGetAllHandler waterResourceGetAllHandler)
        {
            _waterResourceGetAllHandler = waterResourceGetAllHandler;
            _waterResourceGetAllHandler.NotNull(nameof(waterResourceGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WaterResourceGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<WaterResourceGetDto> waterResources = await _waterResourceGetAllHandler.Handle(cancellationToken);
            return Ok(waterResources);
        }
    }
}
