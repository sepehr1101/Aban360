using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering
{
    [Route("v1/water-meter-tag-definition")]
    public class WaterMeterTagDefinitionController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterTagDefinitionCreateHandler _waterMeterTagDefinitionCreateHandler;
        private readonly IWaterMeterTagDefinitionUpdateHandler _waterMeterTagDefinitionUpdateHandler;
        private readonly IWaterMeterTagDefinitionDeleteHandler _waterMeterTagDefinitionDeleteHandler;
        private readonly IWaterMeterTagDefinitionGetSingleHandler _waterMeterTagDefinitionGetSingleHandler;
        private readonly IWaterMeterTagDefinitionGetAllHandler _waterMeterTagDefinitionGetAllHandler;

        public WaterMeterTagDefinitionController(
            IUnitOfWork uow,
            IWaterMeterTagDefinitionCreateHandler waterMeterTagDefinitionCreateHandler,
            IWaterMeterTagDefinitionUpdateHandler waterMeterTagDefinitionUpdateHandler,
            IWaterMeterTagDefinitionDeleteHandler waterMeterTagDefinitionDeleteHandler,
            IWaterMeterTagDefinitionGetSingleHandler waterMeterTagDefinitionGetSingleHandler,
            IWaterMeterTagDefinitionGetAllHandler waterMeterTagDefinitionGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterMeterTagDefinitionCreateHandler = waterMeterTagDefinitionCreateHandler;
            _waterMeterTagDefinitionCreateHandler.NotNull(nameof(waterMeterTagDefinitionCreateHandler));

            _waterMeterTagDefinitionUpdateHandler = waterMeterTagDefinitionUpdateHandler;
            _waterMeterTagDefinitionUpdateHandler.NotNull(nameof(waterMeterTagDefinitionUpdateHandler));

            _waterMeterTagDefinitionDeleteHandler = waterMeterTagDefinitionDeleteHandler;
            _waterMeterTagDefinitionDeleteHandler.NotNull(nameof(waterMeterTagDefinitionDeleteHandler));

            _waterMeterTagDefinitionGetSingleHandler = waterMeterTagDefinitionGetSingleHandler;
            _waterMeterTagDefinitionGetSingleHandler.NotNull(nameof(waterMeterTagDefinitionGetSingleHandler));

            _waterMeterTagDefinitionGetAllHandler = waterMeterTagDefinitionGetAllHandler;
            _waterMeterTagDefinitionGetAllHandler.NotNull(nameof(waterMeterTagDefinitionGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterTagDefinitionCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] WaterMeterTagDefinitionCreateDto createDto, CancellationToken cancellationToken)
        {
            await _waterMeterTagDefinitionCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterTagDefinitionUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] WaterMeterTagDefinitionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _waterMeterTagDefinitionUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterTagDefinitionDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] WaterMeterTagDefinitionDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _waterMeterTagDefinitionDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterTagDefinitionGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            WaterMeterTagDefinitionGetDto waterMeterTagDefinition = await _waterMeterTagDefinitionGetSingleHandler.Handle(id, cancellationToken);
            return Ok(waterMeterTagDefinition);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WaterMeterTagDefinitionGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<WaterMeterTagDefinitionGetDto> waterMeterTagDefinition = await _waterMeterTagDefinitionGetAllHandler.Handle(cancellationToken);
            return Ok(waterMeterTagDefinition);
        }
    }
}
