using Aban360.ClaimPool.Application.Features.WasteWater.Services;




using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering
{
    public class WaterMeterTagController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWaterMeterTagService _service;

        public WaterMeterTagController(IUnitOfWork unitOfWork, IWaterMeterTagService service)
        {
            unitOfWork.NotNull(nameof(unitOfWork));
            _unitOfWork = unitOfWork;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("~/v1/water-meter-tag/create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterTagCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] WaterMeterTagCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpDelete]
        [Route("~/water-meter-tag/delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterTagDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] WaterMeterTagDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpPatch]
        [Route("~/v1/water-meter-tag/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterTagUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] WaterMeterTagUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpGet]
        [Route("~/water-meter-tag/search/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterTagGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search(string input, CancellationToken cancellationToken)
        {
            WaterMeterTagGetDto WaterMeterTag = await _service.GetBySearchAsync(input, cancellationToken);
            return Ok(WaterMeterTag);
        }

        [HttpPost, HttpGet]
        [Route("~/water-meter-tag/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WaterMeterTagGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            ICollection<WaterMeterTagGetDto> WaterMeterTag = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(WaterMeterTag);
        }
    }
}
