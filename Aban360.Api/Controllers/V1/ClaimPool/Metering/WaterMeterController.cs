using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering
{
    [Route("v1/water-meter")]
    public class WaterMeterController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterCreateHandler _waterMeterCreateHandler;
        private readonly IWaterMeterUpdateHandler _waterMeterUpdateHandler;
        private readonly IWaterMeterDeleteHandler _waterMeterDeleteHandler;
        private readonly IWaterMeterGetSingleHandler _waterMeterGetSingleHandler;
        private readonly IWaterMeterGetAllHandler _waterMeterGetAllHandler;

        public WaterMeterController(
            IUnitOfWork uow,
            IWaterMeterCreateHandler waterMeterCreateHandler,
            IWaterMeterUpdateHandler waterMeterUpdateHandler,
            IWaterMeterDeleteHandler waterMeterDeleteHandler,
            IWaterMeterGetSingleHandler waterMeterGetSingleHandler,
            IWaterMeterGetAllHandler waterMeterGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterMeterCreateHandler = waterMeterCreateHandler;
            _waterMeterCreateHandler.NotNull(nameof(waterMeterCreateHandler));

            _waterMeterUpdateHandler = waterMeterUpdateHandler;
            _waterMeterUpdateHandler.NotNull(nameof(waterMeterUpdateHandler));

            _waterMeterDeleteHandler = waterMeterDeleteHandler;
            _waterMeterDeleteHandler.NotNull(nameof(waterMeterDeleteHandler));

            _waterMeterGetSingleHandler = waterMeterGetSingleHandler;
            _waterMeterGetSingleHandler.NotNull(nameof(waterMeterGetSingleHandler));

            _waterMeterGetAllHandler = waterMeterGetAllHandler;
            _waterMeterGetAllHandler.NotNull(nameof(waterMeterGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] WaterMeterCreateDto createDto, CancellationToken cancellationToken)
        {
            await _waterMeterCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] WaterMeterUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _waterMeterUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] WaterMeterDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _waterMeterDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet,HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            WaterMeterGetDto waterMeter = await _waterMeterGetSingleHandler.Handle(id,cancellationToken);
            return Ok(waterMeter);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WaterMeterGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<WaterMeterGetDto> waterMeter = await _waterMeterGetAllHandler.Handle(cancellationToken);
            return Ok(waterMeter);
        }
    }
}
