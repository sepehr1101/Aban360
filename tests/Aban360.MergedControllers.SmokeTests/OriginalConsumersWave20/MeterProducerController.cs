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
    [Route("v1/meter-producer")]
    public class MeterProducerController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterProducerCreateHandler _meterProducerCreateHandler;
        private readonly IMeterProducerUpdateHandler _meterProducerUpdateHandler;
        private readonly IMeterProducerDeleteHandler _meterProducerDeleteHandler;
        private readonly IMeterProducerGetSingleHandler _meterProducerGetSingleHandler;
        private readonly IMeterProducerGetAllHandler _meterProducerGetAllHandler;

        public MeterProducerController(
            IUnitOfWork uow,
            IMeterProducerCreateHandler meterProducerCreateHandler,
            IMeterProducerUpdateHandler meterProducerUpdateHandler,
            IMeterProducerDeleteHandler meterProducerDeleteHandler,
            IMeterProducerGetSingleHandler meterProducerGetSingleHandler,
            IMeterProducerGetAllHandler meterProducerGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterProducerCreateHandler = meterProducerCreateHandler;
            _meterProducerCreateHandler.NotNull(nameof(meterProducerCreateHandler));

            _meterProducerUpdateHandler = meterProducerUpdateHandler;
            _meterProducerUpdateHandler.NotNull(nameof(meterProducerUpdateHandler));

            _meterProducerDeleteHandler = meterProducerDeleteHandler;
            _meterProducerDeleteHandler.NotNull(nameof(meterProducerDeleteHandler));

            _meterProducerGetSingleHandler = meterProducerGetSingleHandler;
            _meterProducerGetSingleHandler.NotNull(nameof(meterProducerGetSingleHandler));

            _meterProducerGetAllHandler = meterProducerGetAllHandler;
            _meterProducerGetAllHandler.NotNull(nameof(meterProducerGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterProducerCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] MeterProducerCreateDto createDto, CancellationToken cancellationToken)
        {
            await _meterProducerCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterProducerUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] MeterProducerUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _meterProducerUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterProducerDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] MeterProducerDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _meterProducerDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterProducerGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            MeterProducerGetDto meterProducer = await _meterProducerGetSingleHandler.Handle(id, cancellationToken);
            return Ok(meterProducer);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<MeterProducerGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<MeterProducerGetDto> meterProducer = await _meterProducerGetAllHandler.Handle(cancellationToken);
            return Ok(meterProducer);
        }
    }
}
