using Aban360.Api.Controllers.V1;
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

namespace BeforeServiceConsumersWave20.Aban360.Api.Controllers.V1.ClaimPool.Metering
{
    [Route("v1/meter-type")]
    public class MeterTypeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterTypeCreateHandler _meterTypeCreateHandler;
        private readonly IMeterTypeUpdateHandler _meterTypeUpdateHandler;
        private readonly IMeterTypeDeleteHandler _meterTypeDeleteHandler;
        private readonly IMeterTypeGetSingleHandler _meterTypeGetSingleHandler;
        private readonly IMeterTypeGetAllHandler _meterTypeGetAllHandler;

        public MeterTypeController(
            IUnitOfWork uow,
            IMeterTypeCreateHandler meterTypeCreateHandler,
            IMeterTypeUpdateHandler meterTypeUpdateHandler,
            IMeterTypeDeleteHandler meterTypeDeleteHandler,
            IMeterTypeGetSingleHandler meterTypeGetSingleHandler,
            IMeterTypeGetAllHandler meterTypeGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterTypeCreateHandler = meterTypeCreateHandler;
            _meterTypeCreateHandler.NotNull(nameof(meterTypeCreateHandler));

            _meterTypeUpdateHandler = meterTypeUpdateHandler;
            _meterTypeUpdateHandler.NotNull(nameof(meterTypeUpdateHandler));

            _meterTypeDeleteHandler = meterTypeDeleteHandler;
            _meterTypeDeleteHandler.NotNull(nameof(meterTypeDeleteHandler));

            _meterTypeGetSingleHandler = meterTypeGetSingleHandler;
            _meterTypeGetSingleHandler.NotNull(nameof(meterTypeGetSingleHandler));

            _meterTypeGetAllHandler = meterTypeGetAllHandler;
            _meterTypeGetAllHandler.NotNull(nameof(meterTypeGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] MeterTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _meterTypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            
            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] MeterTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _meterTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterTagDefinitionGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] MeterTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _meterTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            MeterTypeGetDto meterType = await _meterTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(meterType);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<MeterTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<MeterTypeGetDto> meterTypes = await _meterTypeGetAllHandler.Handle(cancellationToken);
            return Ok(meterTypes);
        }
    }
}
