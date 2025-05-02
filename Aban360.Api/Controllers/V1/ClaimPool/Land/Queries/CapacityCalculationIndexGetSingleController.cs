using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/capacity-calculation-index")]
    public class CapacityCalculationIndexGetSingleController : BaseController
    {
        private readonly ICapacityCalculationIndexGetSingleHandler _capacityCalculationIndexGetSingleHandler;
        public CapacityCalculationIndexGetSingleController(ICapacityCalculationIndexGetSingleHandler capacityCalculationIndexGetSingleHandler)
        {
            _capacityCalculationIndexGetSingleHandler = capacityCalculationIndexGetSingleHandler;
            _capacityCalculationIndexGetSingleHandler.NotNull(nameof(capacityCalculationIndexGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CapacityCalculationIndexGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var capacityCalculationIndexs = await _capacityCalculationIndexGetSingleHandler.Handle(id, cancellationToken);
            return Ok(capacityCalculationIndexs);
        }
    }
}
