using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering
{
    [Route("v1/meter-use-type")]
    public class MeterUseTypeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterUseTypeCreateHandler _meterUseTypeCreateHandler;
        private readonly IMeterUseTypeUpdateHandler _meterUseTypeUpdateHandler;
        private readonly IMeterUseTypeDeleteHandler _meterUseTypeDeleteHandler;
        private readonly IMeterUseTypeGetSingleHandler _meterUseTypeGetSingleHandler;
        private readonly IMeterUseTypeGetAllHandler _meterUseTypeGetAllHandler;

        public MeterUseTypeController(
            IUnitOfWork uow,
            IMeterUseTypeCreateHandler meterUseTypeCreateHandler,
            IMeterUseTypeUpdateHandler meterUseTypeUpdateHandler,
            IMeterUseTypeDeleteHandler meterUseTypeDeleteHandler,
            IMeterUseTypeGetSingleHandler meterUseTypeGetSingleHandler,
            IMeterUseTypeGetAllHandler meterUseTypeGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterUseTypeCreateHandler = meterUseTypeCreateHandler;
            _meterUseTypeCreateHandler.NotNull(nameof(meterUseTypeCreateHandler));

            _meterUseTypeUpdateHandler = meterUseTypeUpdateHandler;
            _meterUseTypeUpdateHandler.NotNull(nameof(meterUseTypeUpdateHandler));

            _meterUseTypeDeleteHandler = meterUseTypeDeleteHandler;
            _meterUseTypeDeleteHandler.NotNull(nameof(meterUseTypeDeleteHandler));

            _meterUseTypeGetSingleHandler = meterUseTypeGetSingleHandler;
            _meterUseTypeGetSingleHandler.NotNull(nameof(meterUseTypeGetSingleHandler));

            _meterUseTypeGetAllHandler = meterUseTypeGetAllHandler;
            _meterUseTypeGetAllHandler.NotNull(nameof(meterUseTypeGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterUseTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] MeterUseTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _meterUseTypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken); 

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterUseTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] MeterUseTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _meterUseTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterUseTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] MeterUseTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _meterUseTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterUseTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(MeterUseTypeEnum id, CancellationToken cancellationToken)
        {
            MeterUseTypeGetDto meterUseType = await _meterUseTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(meterUseType);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<MeterUseTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<MeterUseTypeGetDto> meterUseTypes = await _meterUseTypeGetAllHandler.Handle(cancellationToken);
            return Ok(meterUseTypes);
        }
    }
}
