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
    [Route("v1/meter-diameter")]
    public class MeterDiameterController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterDiameterCreateHandler _meterDiameterCreateHandler;
        private readonly IMeterDiameterUpdateHandler _meterDiameterUpdateHandler;
        private readonly IMeterDiameterDeleteHandler _meterDiameterDeleteHandler;
        private readonly IMeterDiameterGetSingleHandler _meterDiameterGetSingleHandler;
        private readonly IMeterDiameterGetAllHandler _meterDiameterGetAllHandler;

        public MeterDiameterController(
            IUnitOfWork uow,
            IMeterDiameterCreateHandler meterDiameterCreateHandler,
            IMeterDiameterUpdateHandler meterDiameterUpdateHandler,
            IMeterDiameterDeleteHandler meterDiameterDeleteHandler,
            IMeterDiameterGetSingleHandler meterDiameterGetSingleHandler,
            IMeterDiameterGetAllHandler meterDiameterGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterDiameterCreateHandler = meterDiameterCreateHandler;
            _meterDiameterCreateHandler.NotNull(nameof(meterDiameterCreateHandler));

            _meterDiameterUpdateHandler = meterDiameterUpdateHandler;
            _meterDiameterUpdateHandler.NotNull(nameof(meterDiameterUpdateHandler));

            _meterDiameterDeleteHandler = meterDiameterDeleteHandler;
            _meterDiameterDeleteHandler.NotNull(nameof(meterDiameterDeleteHandler));

            _meterDiameterGetSingleHandler = meterDiameterGetSingleHandler;
            _meterDiameterGetSingleHandler.NotNull(nameof(meterDiameterGetSingleHandler));

            _meterDiameterGetAllHandler = meterDiameterGetAllHandler;
            _meterDiameterGetAllHandler.NotNull(nameof(meterDiameterGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterDiameterCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] MeterDiameterCreateDto createDto, CancellationToken cancellationToken)
        {
            await _meterDiameterCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterDiameterUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] MeterDiameterUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _meterDiameterUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterDiameterDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] MeterDiameterDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _meterDiameterDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterDiameterGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(short id, CancellationToken cancellationToken)
        {
            MeterDiameterGetDto meterDiameter = await _meterDiameterGetSingleHandler.Handle(id, cancellationToken);
            return Ok(meterDiameter);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<MeterDiameterGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<MeterDiameterGetDto> meterDiameters = await _meterDiameterGetAllHandler.Handle(cancellationToken);
            return Ok(meterDiameters);
        }
    }
}
