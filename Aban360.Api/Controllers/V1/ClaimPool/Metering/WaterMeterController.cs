using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.Metering.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering
{
    [Route("v1/water-meter")]
    public class WaterMeterController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterService _waterMeterService;
        public WaterMeterController(IUnitOfWork uow, IWaterMeterService waterMeterService)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
            _waterMeterService = waterMeterService;
            _waterMeterService.NotNull(nameof(waterMeterService));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] WaterMeterCreateDto createDto, CancellationToken cancellationToken)
        {
            await _waterMeterService.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] WaterMeterUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _waterMeterService.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] WaterMeterDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _waterMeterService.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            WaterMeterGetDto waterMeter = await _waterMeterService.GetSingleAsync(id, cancellationToken);
            return Ok(waterMeter);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WaterMeterGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<WaterMeterGetDto> waterMeter = await _waterMeterService.GetAllAsync(cancellationToken);
            return Ok(waterMeter);
        }
    }
}
