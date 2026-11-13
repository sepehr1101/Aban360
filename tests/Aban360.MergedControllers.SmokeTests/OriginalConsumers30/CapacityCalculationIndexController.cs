// Frozen consumer before application service consolidation; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.ClaimPool.Land;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers30.Aban360.Api.Controllers.V1.ClaimPool.Land
{
    [Route("v1/capacity-calculation-index")]
    public class CapacityCalculationIndexController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICapacityCalculationIndexCreateHandler _capacityCalculationIndexCreateHandler;
        private readonly ICapacityCalculationIndexUpdateHandler _capacityCalculationIndexUpdateHandler;
        private readonly ICapacityCalculationIndexDeleteHandler _capacityCalculationIndexDeleteHandler;
        private readonly ICapacityCalculationIndexGetSingleHandler _capacityCalculationIndexGetSingleHandler;
        private readonly ICapacityCalculationIndexGetAllHandler _capacityCalculationIndexGetAllHandler;

        public CapacityCalculationIndexController(
            IUnitOfWork uow,
            ICapacityCalculationIndexCreateHandler capacityCalculationIndexCreateHandler,
            ICapacityCalculationIndexUpdateHandler capacityCalculationIndexUpdateHandler,
            ICapacityCalculationIndexDeleteHandler capacityCalculationIndexDeleteHandler,
            ICapacityCalculationIndexGetSingleHandler capacityCalculationIndexGetSingleHandler,
            ICapacityCalculationIndexGetAllHandler capacityCalculationIndexGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _capacityCalculationIndexCreateHandler = capacityCalculationIndexCreateHandler;
            _capacityCalculationIndexCreateHandler.NotNull(nameof(capacityCalculationIndexCreateHandler));

            _capacityCalculationIndexUpdateHandler = capacityCalculationIndexUpdateHandler;
            _capacityCalculationIndexUpdateHandler.NotNull(nameof(capacityCalculationIndexUpdateHandler));

            _capacityCalculationIndexDeleteHandler = capacityCalculationIndexDeleteHandler;
            _capacityCalculationIndexDeleteHandler.NotNull(nameof(capacityCalculationIndexDeleteHandler));

            _capacityCalculationIndexGetSingleHandler = capacityCalculationIndexGetSingleHandler;
            _capacityCalculationIndexGetSingleHandler.NotNull(nameof(capacityCalculationIndexGetSingleHandler));

            _capacityCalculationIndexGetAllHandler = capacityCalculationIndexGetAllHandler;
            _capacityCalculationIndexGetAllHandler.NotNull(nameof(capacityCalculationIndexGetAllHandler));
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

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CapacityCalculationIndexUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] CapacityCalculationIndexUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _capacityCalculationIndexUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
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

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CapacityCalculationIndexGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var capacityCalculationIndexs = await _capacityCalculationIndexGetSingleHandler.Handle(id, cancellationToken);
            return Ok(capacityCalculationIndexs);
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
