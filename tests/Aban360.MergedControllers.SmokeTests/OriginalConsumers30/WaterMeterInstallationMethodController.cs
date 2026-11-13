// Frozen consumer before application service consolidation; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.ClaimPool.Metering;
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

namespace BeforeServiceConsumers30.Aban360.Api.Controllers.V1.ClaimPool.Metering
{
    [Route("v1/watermeter-installation-method")]
    public class WaterMeterInstallationMethodController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterInstallationMethodCreateHandler _waterMeterInstallationMethodCreateHandler;
        private readonly IWaterMeterInstallationMethodUpdateHandler _waterMeterInstallationMethodUpdateHandler;
        private readonly IWaterMeterInstallationMethodDeleteHandler _waterMeterInstallationMethodDeleteHandler;
        private readonly IWaterMeterInstallationMethodGetSingleHandler _waterMeterInstallationMethodGetSingleHandler;
        private readonly IWaterMeterInstallationMethodGetAllHandler _waterMeterInstallationMethodGetAllHandler;

        public WaterMeterInstallationMethodController(
            IUnitOfWork uow,
            IWaterMeterInstallationMethodCreateHandler waterMeterInstallationMethodCreateHandler,
            IWaterMeterInstallationMethodUpdateHandler waterMeterInstallationMethodUpdateHandler,
            IWaterMeterInstallationMethodDeleteHandler waterMeterInstallationMethodDeleteHandler,
            IWaterMeterInstallationMethodGetSingleHandler waterMeterInstallationMethodGetSingleHandler,
            IWaterMeterInstallationMethodGetAllHandler waterMeterInstallationMethodGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterMeterInstallationMethodCreateHandler = waterMeterInstallationMethodCreateHandler;
            _waterMeterInstallationMethodCreateHandler.NotNull(nameof(waterMeterInstallationMethodCreateHandler));

            _waterMeterInstallationMethodUpdateHandler = waterMeterInstallationMethodUpdateHandler;
            _waterMeterInstallationMethodUpdateHandler.NotNull(nameof(waterMeterInstallationMethodUpdateHandler));

            _waterMeterInstallationMethodDeleteHandler = waterMeterInstallationMethodDeleteHandler;
            _waterMeterInstallationMethodDeleteHandler.NotNull(nameof(waterMeterInstallationMethodDeleteHandler));

            _waterMeterInstallationMethodGetSingleHandler = waterMeterInstallationMethodGetSingleHandler;
            _waterMeterInstallationMethodGetSingleHandler.NotNull(nameof(waterMeterInstallationMethodGetSingleHandler));

            _waterMeterInstallationMethodGetAllHandler = waterMeterInstallationMethodGetAllHandler;
            _waterMeterInstallationMethodGetAllHandler.NotNull(nameof(waterMeterInstallationMethodGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterInstallationMethodCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] WaterMeterInstallationMethodCreateDto createDto, CancellationToken cancellationToken)
        {
            await _waterMeterInstallationMethodCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterInstallationMethodUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] WaterMeterInstallationMethodUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _waterMeterInstallationMethodUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterInstallationMethodDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] WaterMeterInstallationMethodDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _waterMeterInstallationMethodDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterInstallationMethodGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var waterMeterInstallationMethods = await _waterMeterInstallationMethodGetSingleHandler.Handle(id, cancellationToken);
            return Ok(waterMeterInstallationMethods);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WaterMeterInstallationMethodGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var waterMeterInstallationMethods = await _waterMeterInstallationMethodGetAllHandler.Handle(cancellationToken);
            return Ok(waterMeterInstallationMethods);
        }
    }
}
