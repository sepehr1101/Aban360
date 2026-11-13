using Aban360.ClaimPool.Application.Features.Metering.Services;
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
    [Route("v1/watermeter-installation-method")]
    public class WaterMeterInstallationMethodController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterInstallationMethodService _service;

        public WaterMeterInstallationMethodController(IUnitOfWork uow, IWaterMeterInstallationMethodService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterInstallationMethodCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] WaterMeterInstallationMethodCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterInstallationMethodUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] WaterMeterInstallationMethodUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterInstallationMethodDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] WaterMeterInstallationMethodDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterInstallationMethodGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var waterMeterInstallationMethods = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(waterMeterInstallationMethods);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WaterMeterInstallationMethodGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var waterMeterInstallationMethods = await _service.GetAllAsync(cancellationToken);
            return Ok(waterMeterInstallationMethods);
        }
    }
}
