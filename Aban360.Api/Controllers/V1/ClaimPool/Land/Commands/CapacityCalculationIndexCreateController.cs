using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/capacity-calculation-index")]
    public class CapacityCalculationIndexCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICapacityCalculationIndexCreateHandler _capacityCalculationIndexCreateHandler;
        public CapacityCalculationIndexCreateController(
            IUnitOfWork uow,
            ICapacityCalculationIndexCreateHandler capacityCalculationIndexCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _capacityCalculationIndexCreateHandler = capacityCalculationIndexCreateHandler;
            _capacityCalculationIndexCreateHandler.NotNull(nameof(capacityCalculationIndexCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CapacityCalculationIndexCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CapacityCalculationIndexCreateDto createDto, CancellationToken cancellationToken)
        {
            await _capacityCalculationIndexCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
