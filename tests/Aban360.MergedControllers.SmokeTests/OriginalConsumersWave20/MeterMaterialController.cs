using Aban360.Api.Controllers.V1;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumersWave20.Aban360.Api.Controllers.V1.ClaimPool.Metering
{
    [Route("v1/meter-material")]
    public class MeterMaterialController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterMaterialCreateHandler _meterMaterialCreateHandler;
        private readonly IMeterMaterialUpdateHandler _meterMaterialUpdateHandler;
        private readonly IMeterMaterialDeleteHandler _meterMaterialDeleteHandler;
        private readonly IMeterMaterialGetSingleHandler _meterMaterialGetSingleHandler;
        private readonly IMeterMaterialGetAllHandler _meterMaterialGetAllHandler;

        public MeterMaterialController(
            IUnitOfWork uow,
            IMeterMaterialCreateHandler meterMaterialCreateHandler,
            IMeterMaterialUpdateHandler meterMaterialUpdateHandler,
            IMeterMaterialDeleteHandler meterMaterialDeleteHandler,
            IMeterMaterialGetSingleHandler meterMaterialGetSingleHandler,
            IMeterMaterialGetAllHandler meterMaterialGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterMaterialCreateHandler = meterMaterialCreateHandler;
            _meterMaterialCreateHandler.NotNull(nameof(meterMaterialCreateHandler));

            _meterMaterialUpdateHandler = meterMaterialUpdateHandler;
            _meterMaterialUpdateHandler.NotNull(nameof(meterMaterialUpdateHandler));

            _meterMaterialDeleteHandler = meterMaterialDeleteHandler;
            _meterMaterialDeleteHandler.NotNull(nameof(meterMaterialDeleteHandler));

            _meterMaterialGetSingleHandler = meterMaterialGetSingleHandler;
            _meterMaterialGetSingleHandler.NotNull(nameof(meterMaterialGetSingleHandler));

            _meterMaterialGetAllHandler = meterMaterialGetAllHandler;
            _meterMaterialGetAllHandler.NotNull(nameof(meterMaterialGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterMaterialCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] MeterMaterialCreateDto createDto, CancellationToken cancellationToken)
        {
            await _meterMaterialCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterMaterialUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] MeterMaterialUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _meterMaterialUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterMaterialDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] MeterMaterialDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _meterMaterialDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterMaterialGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            MeterMaterialGetDto meterMaterial = await _meterMaterialGetSingleHandler.Handle(id, cancellationToken);
            return Ok(meterMaterial);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<MeterMaterialGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<MeterMaterialGetDto> meterMaterials = await _meterMaterialGetAllHandler.Handle(cancellationToken);
            return Ok(meterMaterials);
        }
    }
}
