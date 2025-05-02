using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/capacity-calculation-index")]
    public class CapacityCalculationIndexGetAllController : BaseController
    {
        private readonly ICapacityCalculationIndexGetAllHandler _capacityCalculationIndexGetAllHandler;
        public CapacityCalculationIndexGetAllController(ICapacityCalculationIndexGetAllHandler capacityCalculationIndexGetAllHandler)
        {
            _capacityCalculationIndexGetAllHandler = capacityCalculationIndexGetAllHandler;
            _capacityCalculationIndexGetAllHandler.NotNull(nameof(capacityCalculationIndexGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CapacityCalculationIndexGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var capacityCalculationIndexs = await _capacityCalculationIndexGetAllHandler.Handle(cancellationToken);
            return Ok(capacityCalculationIndexs);
        }
    }
}
