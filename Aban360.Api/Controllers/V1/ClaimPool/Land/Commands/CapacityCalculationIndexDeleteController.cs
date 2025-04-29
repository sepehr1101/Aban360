using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/capacity-calculation-index")]
    public class CapacityCalculationIndexDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICapacityCalculationIndexDeleteHandler _capacityCalculationIndexDeleteHandler;
        public CapacityCalculationIndexDeleteController(
            IUnitOfWork uow,
            ICapacityCalculationIndexDeleteHandler capacityCalculationIndexDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _capacityCalculationIndexDeleteHandler = capacityCalculationIndexDeleteHandler;
            _capacityCalculationIndexDeleteHandler.NotNull(nameof(capacityCalculationIndexDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CapacityCalculationIndexDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] CapacityCalculationIndexDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _capacityCalculationIndexDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
