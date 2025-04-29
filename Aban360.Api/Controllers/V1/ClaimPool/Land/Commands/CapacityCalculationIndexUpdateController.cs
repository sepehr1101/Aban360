using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/capacity-calculation-index")]
    public class CapacityCalculationIndexUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICapacityCalculationIndexUpdateHandler _capacityCalculationIndexUpdateHandler;
        public CapacityCalculationIndexUpdateController(
            IUnitOfWork uow,
            ICapacityCalculationIndexUpdateHandler capacityCalculationIndexUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _capacityCalculationIndexUpdateHandler = capacityCalculationIndexUpdateHandler;
            _capacityCalculationIndexUpdateHandler.NotNull(nameof(capacityCalculationIndexUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CapacityCalculationIndexUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] CapacityCalculationIndexUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _capacityCalculationIndexUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
